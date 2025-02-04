namespace InGame.Cases.TowerDefense.System
{
    /// <summary>
    /// 공격받을 수 있는 객체가 구현해야 하는 인터페이스입니다.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// 공격을 받아 데미지를 적용합니다.
        /// </summary>
        /// <param name="damage">적용할 피해량</param>
        void TakeDamage(float damage);
    }
}
