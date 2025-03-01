using InGame.Cases.TowerDefense.System.Managers;
using Root.Util;

namespace InGame.Cases.TowerDefense.Enemy.Pool
{
    /// <summary>
    /// 적 오브젝트 베이스를 관리하는 풀링 클래스입니다.
    /// </summary>
    public sealed class EnemyBasePool : ObjectPoolBase<EnemyRoot>
    {
        private EnemyDependencyContainer _dependencyContainer;
        
        public void Init(TowerDefenseManager rootManager)
        {
            _dependencyContainer = new EnemyDependencyContainer(this, rootManager.DataManager.Enemy);
        }

        protected override EnemyRoot CreatePooledItem()
        {
            var enemy = base.CreatePooledItem();
            enemy.Init(_dependencyContainer);

            return enemy;
        }
        
        protected override void OnTakeFromPool(EnemyRoot item)
        {
            item.StatController.OnRetrieveFromPool();
            item.gameObject.SetActive(true);
        }
    }
}
