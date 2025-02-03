using Root.Util;

namespace InGame.Cases.TowerDefense.Tower.Pool
{
    /// <summary>
    /// 타워 투사체를 관리하는 풀링 클래스입니다.
    /// </summary>
    public sealed class TowerProjectileBasePool : ObjectPoolBase<TowerProjectileBase>
    {
        /// <summary>
        /// 새로운 투사체 인스턴스를 생성하여 풀에 추가합니다.
        /// </summary>
        /// <returns>새롭게 생성된 TowerProjectileBase 객체</returns>
        protected override TowerProjectileBase CreatePooledItem()
        {
            TowerProjectileBase projectile = base.CreatePooledItem();
            projectile.SetPoolRef(this);
            
            return projectile;
        }
    }
}
