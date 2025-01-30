using InGame.Cases.TowerDefense.Tower;
using InGame.System;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// TowerDefense에서 사용되는 루트 매니저 클래스입니다.
    /// </summary>
    public sealed class TowerDefenseManager : InGameManager
    {
        [SerializeField] private EnemyPathManager pathManager;
        public EnemyPathManager PathManager => pathManager;

        private TowerDefenseDataManager _towerDefenseDataManager;
        public TowerDefenseDataManager DataManager => _towerDefenseDataManager;

        private TowerCreateHandler createHandler;

        protected override void Awake()
        {
            base.Awake();
            
            AssertHelper.NotNull(typeof(TowerDefenseManager), pathManager);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            createHandler = new TowerCreateHandler(this);
        }

        protected override void SetDataManager() => _towerDefenseDataManager = dataManager as TowerDefenseDataManager;

        protected override void OnDispose()
        {
            base.OnDispose();
            
            createHandler.Dispose();
        }
    }
}
