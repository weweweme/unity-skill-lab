using InGame.System;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// 적 통계 패널의 뷰와 프레젠터를 관리하는 ViewController입니다. 
    /// </summary>
    public sealed class VC_EnemyStatsPanel : ViewController
    {
        [SerializeField] private VW_CurrentRoundCount _vwCurrentRoundCount;
        private readonly PR_CurrentRoundCount _prCurrentRoundCount = new PR_CurrentRoundCount();
        
        [SerializeField] private VW_EnemyCountDisplay _vwEnemyCountDisplay;
        private readonly PR_EnemyCountDisplay _prEnemyCountDisplay = new PR_EnemyCountDisplay();

        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_EnemyStatsPanel), _vwCurrentRoundCount);
            AssertHelper.NotNull(typeof(VC_EnemyStatsPanel), _vwEnemyCountDisplay);
        }

        public override void Init(DataManager dataManager)
        {
            _prCurrentRoundCount.Init(dataManager, _vwCurrentRoundCount);
            _prEnemyCountDisplay.Init(dataManager, _vwEnemyCountDisplay);
        }

        protected override void ReleasePresenter()
        {
            _prCurrentRoundCount.Dispose();
            _prEnemyCountDisplay.Dispose();
        }
    }
}
