using InGame.Cases.TowerDefense.System.Model;

namespace InGame.Cases.TowerDefense.Enemy.Pool
{
    /// <summary>
    /// Enemy에서 사용될 외부 모듈의 참조를 관리하는 컨테이너 클래스입니다.
    /// </summary>
    public class EnemyDependencyContainer
    {
        private readonly EnemyBasePool _enemyBasePool;
        public EnemyBasePool EnemyBasePool => _enemyBasePool;
        
        private readonly MDL_Enemy _mdlEnemy;
        public MDL_Enemy MDLEnemy => _mdlEnemy;

        public EnemyDependencyContainer(EnemyBasePool enemyBasePool, MDL_Enemy mdlEnemy)
        {
            _enemyBasePool = enemyBasePool;
            _mdlEnemy = mdlEnemy;
        }
    }
}
