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

        private void Awake()
        {
            _rb = gameObject.GetComponentOrAssert<Rigidbody2D>();
        }

        /// <summary>
        /// FixedUpdate에서 투사체의 속도를 지속적으로 유지합니다.
        /// 물리 연산과 동기화되며, 활성 상태이고 충돌하지 않은 경우에만 이동합니다.
        /// </summary>
        private void FixedUpdate()
        {
            if (!_isActive) return; // 비활성화된 투사체는 이동하지 않음
            if (_isHit) return; // 충돌한 투사체는 더 이상 이동하지 않음
            
            // FixedUpdate에서 지속적으로 속도를 유지하도록 보장합니다.
            _rb.velocity = _direction * _speed;
        }
    }
}
