using InGame.System;
using UnityEngine;

namespace InGame.Cases.TowerDefense
{
    public sealed class TowerDefenseManager : InGameManager
    {
        [SerializeField] private EnemyPathManager pathManager;
        public EnemyPathManager PathManager => pathManager;

        protected override void Awake()
        {
            base.Awake();
            
            Debug.Assert(pathManager);
        }
    }
}
