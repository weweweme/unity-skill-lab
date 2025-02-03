using System;
using System.Threading;
using CleverCrow.Fluid.BTs.Tasks;
using Cysharp.Threading.Tasks;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Tower
{
    /// <summary>
    /// 타워의 공격을 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class TowerAttackController : MonoBehaviourBase
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
        private float _fireRate = 0.5f;

        /// <summary>
        /// 현재 발사까지 남은 쿨다운 시간(초)입니다.
        /// </summary>
        private float _fireCooldown;

        /// <summary>
        /// 타워가 현재 공격 중인지 여부입니다.
        /// </summary>
        private bool _isAttacking;

        /// <summary>
        /// 총알의 속도입니다.
        /// </summary>
        private float _bulletSpeed = 10f;

        private void Awake()
        {
            _attackRange = gameObject.GetComponentOrAssert<CircleCollider2D>();
            AssertHelper.NotNull(typeof(TowerAttackController), firePoint);
        }
        
        public void Init()
        {
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

                // 현재 타겟이 없거나 비활성화되었으면 다음 루프로 이동
                if (_currentTarget == null || !_currentTarget.activeInHierarchy)
                {
                    continue;
                }

                // 쿨다운이 남아 있으면 감소시키고 다음 루프로 이동
                if (_fireCooldown > 0)
                {
                    _fireCooldown = Mathf.Max(_fireCooldown - Time.fixedDeltaTime, 0f);
                    continue;
                }

                // 공격 수행
                Attack();
            }
        }

        private void Attack()
        {
        }
    }
}
