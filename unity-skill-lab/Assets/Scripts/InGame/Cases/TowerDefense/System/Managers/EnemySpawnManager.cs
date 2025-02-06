using InGame.Cases.TowerDefense.Enemy.Pool;
using Root.Util;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// 에너미 스폰을 관리하는 매니저 클래스입니다.
    /// </summary>
    public sealed class EnemySpawnManager : MonoBehaviourBase
    {
        private EnemyBasePool _enemyBasePool;
        
        public void Init(EnemyBasePool enemyBasePool)
        {
            _enemyBasePool = enemyBasePool;
        }
    }
}
