using UnityEngine;

namespace InGame.Cases.TowerDefense.System
{
    /// <summary>
    /// 투사체가 발사될 때 필요한 초기 데이터를 포함하는 구조체입니다.
    /// </summary>
    public readonly struct SProjectileFireData
    {
        /// <summary>
        /// 투사체의 이동 방향입니다.
        /// </summary>
        public Vector2 Direction { get; }

        /// <summary>
        /// 투사체가 타격할 목표입니다.
        /// </summary>
        public Transform Target { get; }

        /// <summary>
        /// 투사체가 가하는 피해량입니다.
        /// </summary>
        public int Damage { get; }

        /// <summary>
        /// ProjectileFireData 구조체를 생성합니다.
        /// </summary>
        /// <param name="direction">투사체의 이동 방향</param>
        /// <param name="target">공격 대상</param>
        /// <param name="damage">공격 피해량</param>
        public SProjectileFireData(Vector2 direction, Transform target, int damage)
        {
            Direction = direction;
            Target = target;
            Damage = damage;
        }
    }
}
