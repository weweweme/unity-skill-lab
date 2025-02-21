using InGame.Cases.TowerDefense.System.Managers;
using InGame.System;
using Root.Util;
using UniRx;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Current Wave Count UI의 Presenter입니다.
    /// </summary>
    public sealed class PR_CurrentWaveCount : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            TowerDefenseDataManager tdDataManager = dataManager as TowerDefenseDataManager;
            AssertHelper.NotNull(typeof(PR_CurrentWaveCount), tdDataManager);
            
            VW_CurrentWaveCount currentWaveCount =  view as VW_CurrentWaveCount;
            AssertHelper.NotNull(typeof(PR_CurrentWaveCount), currentWaveCount);
            
            tdDataManager!.Round.CurrentRound
                .Subscribe(currentWaveCount!.SetWaveCount)
                .AddTo(disposable);
        }
    }
}
