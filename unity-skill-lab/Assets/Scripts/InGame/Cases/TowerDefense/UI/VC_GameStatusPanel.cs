using InGame.System;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// 현재 게임 상태를 나타내는 패널의 뷰와 프레젠터를 관리하는 ViewController입니다. 
    /// </summary>
    public sealed class VC_GameStatusPanel : ViewController
    {
        [SerializeField] private VW_RoundInfo _vwRoundInfo;
        private readonly PR_RoundInfo _prRoundInfo = new PR_RoundInfo();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_GameStatusPanel), _vwRoundInfo);
        }

        public override void Init(DataManager dataManager)
        {
            _prRoundInfo.Init(dataManager, _vwRoundInfo);
        }

        protected override void ReleasePresenter()
        {
            _prRoundInfo.Dispose();
        }
    }
}
