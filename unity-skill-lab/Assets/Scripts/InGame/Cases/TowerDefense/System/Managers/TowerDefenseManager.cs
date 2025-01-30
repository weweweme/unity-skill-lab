using InGame.System;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.System.Managers
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
            
            AssertHelper.NotNull(typeof(TowerDefenseManager), pathManager);
        }

        protected override void SetDataManager() => _towerDefenseDataManager = dataManager as TowerDefenseDataManager;
    }
}
