using System;
using System.Threading;
using CleverCrow.Fluid.BTs.Tasks;
using Cysharp.Threading.Tasks;
using InGame.Cases.TowerDefense.System.Managers;
using InGame.Cases.TowerDefense.Tower.Pool;
using InGame.System;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Tower
{
    /// <summary>
    /// 타워의 공격을 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class TowerAttackController : MonoBehaviourBase, IDisposable
    {
        /// <summary>
        /// 타워가 감지할 대상의 레이어 마스크입니다.
        /// </summary>
        private readonly int TARGET_LAYER_MASK = Layers.GetLayerMask(Layers.Enemy);
        
        /// <summary>
        /// 총알이 발사되는 위치입니다.
        /// </summary>
        [SerializeField] private Transform firePoint;

        /// <summary>
        /// 타워의 공격을 제어하는 CancellationTokenSource입니다.
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// 현재 타겟으로 설정된 게임 오브젝트입니다.
        /// 공격 대상이 존재하지 않으면 null입니다.
        /// </summary>
        private GameObject _currentTarget;

        /// <summary>
        /// 타워의 공격 범위를 결정하는 CircleCollider2D입니다.
        /// </summary>
        private CircleCollider2D _attackRange;

        /// <summary>
        /// OverlapCircleNonAlloc에서 사용될 충돌체 버퍼입니다.
        /// </summary>
        private readonly Collider2D[] _hitColsBuffer = new Collider2D[sizeof(int) * 2];
        
        /// <summary>
        /// 타워의 공격 속도 (초당 공격 횟수)입니다.
        /// </summary>
        private readonly float _fireRate = 0.5f;

        /// <summary>
        /// 현재 발사까지 남은 쿨다운 시간(초)입니다.
        /// </summary>
        private float _fireCooldown;

        /// <summary>
        /// 타워가 현재 공격 중인지 여부입니다.
        /// </summary>
        private bool _isAttacking;
        
        /// <summary>
        /// 투사체를 가져올 풀을 참조하는 변수입니다.
        /// </summary>
        private TowerProjectileBasePool _pool;

        private void Awake()
        {
            _attackRange = gameObject.GetComponentOrAssert<CircleCollider2D>();
            AssertHelper.NotNull(typeof(TowerAttackController), firePoint);
        }
        
        public void Init()
        {
            var tdManager = InGameManager.Ins as TowerDefenseManager;
            AssertHelper.NotNull(typeof(TowerAttackController), tdManager);
            
            _pool = tdManager!.PoolManager.TowerProjectileBasePool;
            
            CancelTokenHelper.GetToken(ref _cts);
            StartAttacking(_cts.Token).Forget();
        }

        /// <summary>
        /// 현재 타겟이 존재하는지 여부를 반환합니다.
        /// </summary>
        public bool HasTarget() => _currentTarget != null;

        /// <summary>
        /// 현재 타겟이 사거리 안에 있는지 확인합니다.
        /// </summary>
        /// <returns>타겟이 사거리 안에 있으면 true, 아니면 false</returns>
        public bool IsTargetInRange()
        {
            // 현재 타겟이 없으면 사거리 안에 있을 수 없으므로 false 반환
            if (_currentTarget == null) return false;

            // 타워와 타겟 간의 제곱거리 계산
            Vector3 towerPos = transform.position;
            Vector3 targetPos = _currentTarget.transform.position;
            float distance = Vector3.SqrMagnitude(targetPos - towerPos);

            // 탐색 범위의 제곱과 비교
            float detectRange = _attackRange.radius;
            return distance <= detectRange * detectRange;
        }

        /// <summary>
        /// 공격 범위 내에서 Enemy 레이어에 속한 타겟을 찾습니다.
        /// </summary>
        /// <returns>타겟을 찾으면 TaskStatus.Success, 없으면 TaskStatus.Failure 반환</returns>
        public TaskStatus FindTarget()
        {
            Vector3 towerPos = transform.position;
            float detectRange = _attackRange.radius;

            int hitCount = Physics2D.OverlapCircleNonAlloc(towerPos, detectRange, _hitColsBuffer, TARGET_LAYER_MASK);

            if (hitCount == 0)
            {
                _currentTarget = null;
                return TaskStatus.Failure;
            }

            float closestDistance = float.MaxValue;
            Collider2D closestTarget = null;

            for (int i = 0; i < hitCount; ++i)
            {
                Collider2D targetCol = _hitColsBuffer[i];

                float distance = Vector3.SqrMagnitude(targetCol.transform.position - towerPos);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = targetCol;
                }
            }

            if (closestTarget != null)
            {
                _currentTarget = closestTarget.gameObject;
                return TaskStatus.Success;
            }

            _currentTarget = null;
            return TaskStatus.Failure;
        }

        /// <summary>
        /// 현재 타겟을 해제합니다.
        /// </summary>
        public TaskStatus ClearTarget()
        {
            _currentTarget = null;
            return TaskStatus.Failure;
        }
        
        /// <summary>
        /// 타워가 지속적으로 공격을 수행하는 루틴입니다.
        /// 타겟이 존재하면 일정 간격(fireRate)마다 공격하며, FixedUpdate 타이밍에 실행됩니다.
        /// 타워가 배치되면 루프를 시작하고, 비활성화되면 루프를 종료합니다.
        /// </summary>
        /// <param name="token">비동기 작업을 취소할 수 있는 CancellationToken</param>
        private async UniTaskVoid StartAttacking(CancellationToken token)
        {
            while (!token.IsCancellationRequested) // 취소 요청이 들어오면 루프 종료
            {
                // FixedUpdate 타이밍에서 실행되도록 대기
                await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate, token); 

                // 현재 타겟이 없다면 다음 루프로 이동
                if (_currentTarget == null)
                {
                    continue;
                }

                // 쿨다운이 남아 있으면 감소시키고 다음 루프로 이동
                if (_fireCooldown > 0)
                {
                    _fireCooldown = Mathf.Max(_fireCooldown - Time.fixedDeltaTime, 0f);
                    continue;
                }

                Vector3 targetDir = (_currentTarget.transform.position - firePoint.position).normalized;
                SetMuzzleRotation(targetDir);
                
                // 총구가 타겟을 충분히 향하지 않았다면 리턴
                if (!IsMuzzleFacingTarget(targetDir))
                {
                    continue;
                }
                
                // 공격 수행
                Attack(targetDir, _currentTarget);
                
                // 발사 후 쿨다운 리셋
                _fireCooldown = _fireRate;
            }
        }

        /// <summary>
        /// 타워에서 투사체를 발사하는 로직을 처리합니다.
        /// 풀에서 투사체를 가져와 초기 위치를 설정하고, 목표 데이터를 적용합니다.
        /// </summary>
        /// <param name="dir">투사체가 이동할 방향</param>
        /// <param name="target">공격 대상</param>
        private void Attack(Vector3 dir, GameObject target)
        {
            TowerProjectileBase projectile = _pool.GetObject();
            
            projectile.transform.position = firePoint.position;
            projectile.SetFireData(dir, target.transform);
        }
        
        /// <summary>
        /// 총구를 타겟 방향으로 회전시킵니다.
        /// 타겟의 위치를 기준으로 방향 벡터를 사용하여 Z축 회전을 조정합니다.
        /// </summary>
        /// <param name="dir">타겟 방향을 나타내는 정규화된 벡터</param>
        private void SetMuzzleRotation(Vector3 dir)
        {
            /// <remarks>
            /// [구현 원리]
            /// 1. Atan2(y, x) 함수는 직교 좌표계에서 
            ///    주어진 벡터 (x, y)의 방향(각도)을 라디안(Radian) 단위로 반환합니다.
            ///    - 일반적인 Atan(y/x)와 달리 Atan2(y, x)는 x가 0일 때도 안전하게 계산할 수 있음.
            /// 
            /// 2. Atan2는 반시계 방향을 양수(+), 시계 방향을 음수(-)로 취급합니다.
            ///    즉, 벡터의 방향을 360도 전 범위에서 올바르게 계산할 수 있습니다.
            /// 
            /// 3. 라디안 값은 `-π ~ π (-180° ~ 180°)` 범위를 가지므로, 
            ///    이를 도(degree) 단위로 변환하기 위해 `Mathf.Rad2Deg`(180/π)를 곱합니다.**
            ///
            /// [동작 예시]
            /// 
            /// (1, 0)  → 0°  (오른쪽)
            /// (0, 1)  → 90° (위쪽)
            /// (-1, 0) → 180° (왼쪽)
            /// (0, -1) → -90° (아래쪽)
            /// (1, 1)  → 45° (오른쪽 위 대각선)
            /// (-1, -1) → -135° (왼쪽 아래 대각선)
            ///
            /// 예제) dir = (0.5, 0.5) → atan2(0.5, 0.5) ≈ 0.7854 rad → 0.7854 * (180 / π) ≈ 45°
            ///                                    
            /// </remarks>
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            // 각도에 RotationOffset를 더하여 손이 올바른 방향으로 회전하도록 합니다.
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // 현재 회전 상태와 목표 회전 상태 사이를 부드럽게 선형 보간
            const float ROTATION_LERP_SPEED = 10f;
            transform.rotation = Quaternion.Lerp(
                transform.rotation, // 현재 회전 값
                targetRotation, // 목표 회전 값
                Time.deltaTime * ROTATION_LERP_SPEED // 보간 속도 (직렬화된 값)
            );
        }
        
        /// <summary>
        /// 총구가 타겟을 충분히 향하고 있는지 확인합니다.
        /// 총구의 현재 방향과 타겟 방향 사이의 각도 차이를 계산하여,
        /// 일정 범위(기본값: 5도) 이내에 있으면 조준된 것으로 간주합니다.
        /// </summary>
        /// <param name="targetDir">타겟의 위치를 가리키는 방향 벡터</param>
        /// <returns>총구가 타겟을 조준하고 있으면 true, 그렇지 않으면 false</returns>
        private bool IsMuzzleFacingTarget(Vector3 targetDir)
        {
            /// <remarks>
            /// [구현 원리]
            /// 
            /// 1. 현재 총구의 방향(`transform.right`)과 목표 방향(`targetDir`) 사이의 각도를 계산합니다.
            ///    - transform.right는 오브젝트의 오른쪽(기본 방향)을 나타냅니다.
            /// 
            /// 2. `Vector2.SignedAngle(from, to)`를 사용하여 두 벡터 사이의 회전 각도를 구합니다.
            ///    - 결과 값은 `-180° ~ 180°` 범위의 각도를 반환합니다.
            /// 
            /// 3. `Mathf.Abs(angleToTarget) <= angleThreshold`를 검사하여,
            ///    - 총구와 타겟 방향의 각도 차이가 일정 값(기본: 5도) 이하일 경우 **조준된 상태로 간주**합니다.
            ///
            /// [동작 예시]
            /// 
            /// - 총구가 정확히 타겟을 향할 경우 → `angleToTarget = 0°`
            /// - 총구가 타겟보다 10도 왼쪽을 향할 경우 → `angleToTarget = -10°`
            /// - 총구가 타겟보다 10도 오른쪽을 향할 경우 → `angleToTarget = 10°`
            /// - 위의 예에서 `angleThreshold = 5°`이면, -5° ~ 5° 사이에서만 조준된 것으로 간주됨.
            /// 
            /// </remarks>
            float angleToTarget = Vector2.SignedAngle(transform.right, targetDir); // Z축 회전을 기준으로 각도를 계산
            const float angleThreshold = 5f; // 각도 차이가 5도 이내일 때만 발사 가능
            
            return Mathf.Abs(angleToTarget) <= angleThreshold;
        }
        
        public void Dispose()
        {
            CancelTokenHelper.ClearToken(in _cts);
        }
    }
}
