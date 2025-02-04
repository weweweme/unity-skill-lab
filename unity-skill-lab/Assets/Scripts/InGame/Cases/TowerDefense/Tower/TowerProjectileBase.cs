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
        private readonly float _speed = 10f;

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

        private void Awake()
        {
            _rb = gameObject.GetComponentOrAssert<Rigidbody2D>();
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
        public void SetFireData(in ProjectileFireData fireData)
        {
            _direction = fireData.Direction;
            _target = fireData.Target;
            _damage = fireData.Damage;
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
    }
}
