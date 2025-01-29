using InGame.System;
using UnityEngine;
using UnityEngine.Assertions;

namespace InGame.Cases.TowerDefense.Managers
{
    public sealed class TowerDefenseManager : InGameManager
    {
        [SerializeField] private EnemyPathManager pathManager;
        public EnemyPathManager PathManager => pathManager;

        protected override void Awake()
        {
            base.Awake();
            
            Assert.IsNotNull(pathManager);
        }
    }
}
