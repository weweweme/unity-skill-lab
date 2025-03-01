using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using InGame.Cases.TowerDefense.System;
using InGame.Cases.TowerDefense.Tower.Pool;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Tower
{
    /// <summary>
    /// 타워가 발사하는 투사체(Projectile) 클래스입니다.
    /// 투사체는 특정 방향으로 이동하며, 충돌 시 비활성화됩니다.
    /// </summary>
    public sealed class TowerProjectileBase : MonoBehaviourBase
    {
        /// <summary>
        /// 총알 궤적을 표현하는 TrailRenderer입니다.
        /// </summary>
        [SerializeField] private TrailRenderer trailRenderer;
        
        /// <summary>
        /// 투사체가 현재 활성화 상태인지 여부를 나타냅니다.
        /// 활성화된 경우만 이동 및 충돌 판정을 수행합니다.
        /// </summary>
        private bool _isActive;

        /// <summary>
        /// 투사체가 충돌하여 더 이상 유효하지 않은 상태인지 나타냅니다.
        /// 충돌 시 이동을 멈추고 추가적인 처리를 방지하기 위해 사용됩니다.
        /// </summary>
        private bool _isHit;
        
        /// <summary>
        /// 투사체가 지속적으로 이동하도록 속도를 설정하는 데 사용됩니다.
        /// </summary>
        private Rigidbody2D _rb;

        /// <summary>
        /// 투사체가 이동하는 방향을 나타내는 단위 벡터입니다.
        /// 이 방향을 기준으로 속도를 적용하여 투사체를 이동시킵니다.
        /// </summary>
        private Vector2 _direction;

        /// <summary>
        /// 투사체의 이동 속도를 결정하는 값입니다.
        /// </summary>
        private readonly float _speed = 30f;

        /// <summary>
        /// 투사체가 속한 풀(Pool)을 참조하는 변수입니다.
        /// </summary>
        private TowerProjectileBasePool _pool;

        /// <summary>
        /// 투사체의 목표 타겟을 저장하는 변수입니다.
        /// 유도형 투사체나 특정 타겟을 추적하는 경우 사용됩니다.
        /// </summary>
        private Transform _target;

        /// <summary>
        /// 투사체가 가하는 피해량입니다.
        /// 이 값은 발사될 때 설정되며, 목표에 도달하면 피해를 적용합니다.
        /// </summary>
        private int _damage;
        
        /// <summary>
        /// 게임이 종료됐을 경우 UniTask를 중단시키기 위한 토큰입니다.
        /// </summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        /// <summary>
        /// 총알의 GradientAlphaKey를 백업하는 변수입니다.
        /// </summary>
        private GradientAlphaKey[] _originAlphaKeys;

        private void Awake()
        {
            _rb = gameObject.GetComponentOrAssert<Rigidbody2D>();
            
            // 초기 Gradient값 백업
            _originAlphaKeys = trailRenderer.colorGradient.alphaKeys;
        }

        /// <summary>
        /// 투사체가 속한 풀 객체를 설정합니다.
        /// </summary>
        /// <param name="pool">이 투사체가 속한 TowerProjectileBasePool 객체</param>
        public void SetPoolRef(in TowerProjectileBasePool pool) => _pool = pool;
        
        /// <summary>
        /// 투사체가 발사될 때 필요한 초기 데이터를 설정합니다.
        /// 이동 방향, 타겟, 활성화 상태 등을 지정하여 투사체가 정상적으로 동작하도록 합니다.
        /// </summary>
        /// <param name="fireData">투사체의 이동 방향, 목표, 피해량 등의 데이터를 포함하는 구조체</param>
        public void SetFireData(in SProjectileFireData fireData)
        {
            _direction = fireData.Direction;
            _target = fireData.Target;
            _damage = fireData.Damage;
            ResetTrail();
            
            _isHit = false;
            _isActive = true;
        }

        /// <summary>
        /// FixedUpdate에서 투사체의 속도를 지속적으로 유지합니다.
        /// 목표를 향해 유도탄처럼 날아갑니다
        /// </summary>
        private void FixedUpdate()
        {
            if (!_isActive) return;
            if (_isHit) return;
            
            // 목표 방향을 실시간으로 업데이트하여 유도 효과 적용
            _direction = (_target.position - transform.position).normalized;
            
            // 목표 방향으로 지속적으로 이동
            _rb.velocity = _direction * _speed;
        }
        
        /// <summary>
        /// 투사체가 충돌했을 때의 처리를 수행합니다.
        /// </summary>
        /// <param name="other">충돌한 대상의 Collider</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive) return;
            if (_isHit) return;

            // 충돌한 객체가 IDamageable을 구현하고 있는지 확인
            if (!other.TryGetComponent(out IDamageable target)) return;
            _isHit = true;
            
            // 타겟에게 데미지 적용
            target.TakeDamage(_damage);
            HandleCollision();
        }

        private void HandleCollision()
        {
            // 충돌 처리 후 비활성화
            _rb.velocity = Vector2.zero;
            
            DOTween.To(() => 0.0f, ReduceTrailAlpha, 0.9f, trailRenderer.time)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _isActive = false;
                    RecycleProjectile().Forget();
                });
        }
        
        /// <summary>
        /// TrailRenderer의 알파 값을 서서히 줄여서 궤적이 사라지도록 만듭니다.
        /// </summary>
        /// <param name="progress">시간에 따른 알파 값 변화 정도 (0~1)</param>
        private void ReduceTrailAlpha(float progress)
        {
            Gradient gradient = trailRenderer.colorGradient;
            GradientAlphaKey[] alphaKeys = gradient.alphaKeys;

            for (int i = 0; i < alphaKeys.Length; i++)
            {
                alphaKeys[i].alpha = Mathf.Lerp(alphaKeys[i].alpha, 0.0f, progress);
            }

            gradient.alphaKeys = alphaKeys;
            trailRenderer.colorGradient = gradient;
        }
        
        /// <summary>
        /// 일정 시간이 지난 후 투사체를 오브젝트 풀에 반환합니다.
        /// TrailRenderer를 초기 상태로 복구하고, 오브젝트를 재사용 가능하게 설정합니다.
        /// </summary>
        private async UniTask RecycleProjectile()
        {
            const int RETURN_DELAY_MS = 500;
            await UniTask.Delay(RETURN_DELAY_MS, cancellationToken: _cts.Token);

            ResetTrail();
            _pool.ReturnObject(this);
        }

        /// <summary>
        /// TrailRenderer의 상태를 초기화하여 다음 발사를 준비합니다.
        /// </summary>
        private void ResetTrail()
        {
            Gradient gradient = trailRenderer.colorGradient; 
            gradient.alphaKeys = _originAlphaKeys;
            trailRenderer.colorGradient = gradient;
            trailRenderer.Clear();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            CancelTokenHelper.ClearToken(_cts);
        }
    }
}
