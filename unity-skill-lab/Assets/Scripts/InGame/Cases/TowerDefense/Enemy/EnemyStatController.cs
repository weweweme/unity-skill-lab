using InGame.Cases.TowerDefense.System;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Enemy
{
    /// <summary>
    /// 적(Enemy)의 체력(Stat)을 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class EnemyStatController : MonoBehaviourBase, IDamageable
    {
        /// <summary>
        /// 적의 최대 체력입니다.
        /// </summary>
        [SerializeField] private float _maxHP = 100f;

        /// <summary>
        /// 적의 현재 체력입니다.
        /// </summary>
        private float _currentHP;
        
        /// <summary>
        /// 피격 시 깜빡임 효과를 재생하는 컨트롤러입니다.
        /// </summary>
        private EnemyHitEffectController _hitEffectController;

        private void Awake()
        {
            _hitEffectController = gameObject.GetComponentOrAssert<EnemyHitEffectController>();
            
            // 적 생성 시, 현재 체력을 최대 체력으로 초기화합니다.
            _currentHP = _maxHP;
        }

        /// <summary>
        /// 데미지를 받아 체력을 감소시킵니다.
        /// 체력이 0 이하가 되면 적을 제거합니다.
        /// </summary>
        /// <param name="damage">적용할 피해량</param>
        public void TakeDamage(float damage)
        {
            _currentHP = Mathf.Max(0, _currentHP - damage);
            Debug.Log($"Enemy took {damage} damage. Remaining HP: {_currentHP}");
            _hitEffectController.PlayHitFlash();

            if (_currentHP > 0) return;
            
            Die();
        }

        /// <summary>
        /// 적이 사망했을 때 호출되는 메서드입니다.
        /// </summary>
        private void Die()
        {
            Debug.Log("Enemy has died.");
            Destroy(gameObject);
        }
    }
}
