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
        
        [SerializeField] private EnemySpawnManager spawnManager;
        public EnemySpawnManager SpawnManager => spawnManager;
        
        [SerializeField] private TowerDefensePoolManager poolManager;
        public TowerDefensePoolManager PoolManager => poolManager;
        
        private TowerDefenseDataManager _towerDefenseDataManager;
        public TowerDefenseDataManager DataManager => _towerDefenseDataManager;
        
        private TowerDefenseSequenceManager _towerDefenseSequenceManager;
        public TowerDefenseSequenceManager TowerDefenseSequenceManager => _towerDefenseSequenceManager;

        private TowerCreateHandler _createHandler;
        
        private TowerDefenseInputEventHandler _inputHandler;
        public TowerDefenseInputEventHandler InputHandler => _inputHandler;

        protected override void Awake()
        {
            base.Awake();
            
            AssertHelper.NotNull(typeof(TowerDefenseManager), pathManager);
            AssertHelper.NotNull(typeof(TowerDefenseManager), spawnManager);
            AssertHelper.NotNull(typeof(TowerDefenseManager), poolManager);
        }

        protected override void Init()
        {
            _inputHandler = new TowerDefenseInputEventHandler();
            _createHandler = new TowerCreateHandler(this);
            
            _towerDefenseSequenceManager.Init(this);
            spawnManager.Init(this);
        }
        
        protected override void StartSequence()
        {
            _towerDefenseSequenceManager.StartSequence();
        }

        protected override void SetDataManager() => _towerDefenseDataManager = dataManager as TowerDefenseDataManager;
        protected override void SetSequenceManager() => _towerDefenseSequenceManager = sequenceManager as TowerDefenseSequenceManager;

        protected override void OnDispose()
        {
            base.OnDispose();

            _inputHandler?.Dispose();
            _createHandler.Dispose();
        }
    }
}
