using InGame.Cases.TowerDefense.System.Managers;
using InGame.System;
using Root.Util;
using UniRx;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Current Wave Count UI의 Presenter입니다.
    /// </summary>
    public sealed class PR_CurrentRoundCount : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            TowerDefenseDataManager tdDataManager = dataManager as TowerDefenseDataManager;
            AssertHelper.NotNull(typeof(PR_CurrentRoundCount), tdDataManager);
            
            VW_CurrentRoundCount currentRoundCount =  view as VW_CurrentRoundCount;
            AssertHelper.NotNull(typeof(PR_CurrentRoundCount), currentRoundCount);
            
            tdDataManager!.Round.CurrentRound
                .Subscribe(currentRoundCount!.SetWaveCount)
                .AddTo(disposable);
        }
    }
}
