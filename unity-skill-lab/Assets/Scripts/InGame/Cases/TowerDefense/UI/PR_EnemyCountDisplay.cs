using InGame.Cases.TowerDefense.System.Managers;
using InGame.Cases.TowerDefense.System.Model;
using InGame.System;
using UniRx;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Enemy Count Display UI를 관리하는 프레젠터 클래스입니다.
    /// </summary>
    public sealed class PR_EnemyCountDisplay : Presenter
    {
        private VW_EnemyCountDisplay _vwEnemyCountDisplay;
        private uint _aliveEnemyCount;

        public override void Init(DataManager dataManager, View view)
        {
            _vwEnemyCountDisplay = view as VW_EnemyCountDisplay;
            TowerDefenseDataManager tdDataManager = dataManager as TowerDefenseDataManager;
            MDL_Enemy enemy = tdDataManager!.Enemy;

            enemy.OnEnemySpawn
                .Subscribe(_ => AddAliveEnemy())
                .AddTo(disposable);
            
            enemy.OnEnemyDeath
                .Subscribe(_ => RemoveAliveEnemy())
                .AddTo(disposable);
        }

        private void AddAliveEnemy()
        {
            ++_aliveEnemyCount;
            _vwEnemyCountDisplay.SetAliveEnemyCount(_aliveEnemyCount);
        }

        private void RemoveAliveEnemy()
        {
            --_aliveEnemyCount;
            _vwEnemyCountDisplay.SetAliveEnemyCount(_aliveEnemyCount);
        }
    }
}
