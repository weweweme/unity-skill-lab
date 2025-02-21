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
        [SerializeField] private VW_CurrentWaveCount _vwCurrentWaveCount;
        private readonly PR_CurrentWaveCount _prCurrentWaveCount = new PR_CurrentWaveCount();

        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_EnemyStatsPanel), _vwCurrentWaveCount);
        }

        public override void Init(DataManager dataManager)
        {
            _prCurrentWaveCount.Init(dataManager, _vwCurrentWaveCount);
        }

        public override void Dispose()
        {
            _prCurrentWaveCount.Dispose();
        }
    }
}
