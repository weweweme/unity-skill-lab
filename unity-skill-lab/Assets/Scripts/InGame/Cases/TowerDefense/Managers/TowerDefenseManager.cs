using InGame.System;
using UnityEngine;
using UnityEngine.Assertions;

namespace InGame.Cases.TowerDefense.Managers
{
    public sealed class TowerDefenseManager : InGameManager
    {
        [SerializeField] private EnemyPathManager pathManager;
        public EnemyPathManager PathManager => pathManager;

        private TowerDefenseDataManager _towerDefenseDataManager;
        public TowerDefenseDataManager DataManager => _towerDefenseDataManager;

        protected override void Awake()
        {
            base.Awake();
            
            Assert.IsNotNull(pathManager, "[TowerDefenseManager] PathManager가 할당되지 않았습니다.");
        }

        protected override void SetDataManager() => _towerDefenseDataManager = dataManager as TowerDefenseDataManager;
    }
}
