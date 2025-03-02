using InGame.Cases.TowerDefense.System.Managers;
using InGame.System;
using Root.Util;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Round Info UI의 Presenter입니다.
    /// </summary>
    public sealed class PR_RoundInfo : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            TowerDefenseDataManager tdDataManager = dataManager as TowerDefenseDataManager;
            AssertHelper.NotNull(typeof(PR_RoundInfo), tdDataManager);
            
            VW_RoundInfo currentRoundCount =  view as VW_RoundInfo;
            AssertHelper.NotNull(typeof(PR_RoundInfo), currentRoundCount);
        }
    }
}
