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
        
        [SerializeField] private TowerDefensePoolManager poolManager;
        public TowerDefensePoolManager PoolManager => poolManager;

        private TowerDefenseDataManager _towerDefenseDataManager;
        public TowerDefenseDataManager DataManager => _towerDefenseDataManager;

        private TowerCreateHandler _createHandler;
        private TowerDefenseInputEventHandler _inputHandler;

        protected override void Awake()
        {
            base.Awake();
            
            AssertHelper.NotNull(typeof(TowerDefenseManager), pathManager);
            AssertHelper.NotNull(typeof(TowerDefenseManager), poolManager);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _inputHandler = new TowerDefenseInputEventHandler();
            _createHandler = new TowerCreateHandler(this);
        }

        protected override void SetDataManager() => _towerDefenseDataManager = dataManager as TowerDefenseDataManager;

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _inputHandler.Dispose();
            _createHandler.Dispose();
        }
    }
}
