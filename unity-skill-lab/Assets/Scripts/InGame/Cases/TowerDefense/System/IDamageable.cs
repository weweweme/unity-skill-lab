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

        /// <summary>
        /// 현재 대상이 공격을 받을 수 있는 상태인지 확인합니다.
        /// </summary>
        /// <returns>true면 공격 가능, false면 공격 불가 상태입니다.</returns>
        bool CanTakeDamage();
    }
}
