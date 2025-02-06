using InGame.Cases.TowerDefense.Enemy.Pool;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// 에너미 스폰을 관리하는 매니저 클래스입니다.
    /// </summary>
    public sealed class EnemySpawnManager : MonoBehaviourBase
    {
        private EnemyBasePool _enemyBasePool;
        private Transform _enemySpawnPoint; 
        
        public void Init(EnemyBasePool enemyBasePool, EnemyPathManager pathManager)
        {
            _enemyBasePool = enemyBasePool;
            
            const int START_IDX = 0;
            _enemySpawnPoint = pathManager.PathNodes[START_IDX];
        }
        }
    }
}
