using System.Collections.Generic;
using InGame.Cases.TowerDefense.System;
using InGame.System;
using Root.Util;
using TMPro;
using UnityEngine;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Round Info UI의 View입니다.
    /// </summary>
    public sealed class VW_RoundInfo : View
    {
        [SerializeField] private TextMeshProUGUI roundInfo;

        private static readonly Dictionary<ERoundStates, string> RoundStateTexts = new()
        {
            { ERoundStates.Spawning, "적들이 몰려오고 있습니다!" },
            { ERoundStates.InProgress, "방어선 유지 중!" },
            { ERoundStates.Waiting, "적이 물러서고 있습니다!" }
        };

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_RoundInfo), roundInfo);
        }

        /// <summary>
        /// 현재 라운드 상태에 해당하는 정보를 UI에 표시합니다.
        /// 상태별로 지정된 문구를 출력하며, 상태 값은 반드시 유효한 값이어야 합니다.
        /// </summary>
        /// <param name="states">적용할 라운드 상태</param>
        public void ApplyRoundStates(ERoundStates states)
        {
            Debug.Assert(states != ERoundStates.None, "states != ERoundStates.None");

            roundInfo.SetText(RoundStateTexts[states]);
        }
        
        /// <summary>
        /// 다음 라운드 시작까지 남은 시간을 UI에 표시합니다.
        /// </summary>
        /// <param name="countdown">다음 라운드 시작까지 남은 시간(초)</param>
        public void ApplyNextRoundCountDown(uint countdown)
        {
            roundInfo.SetText("적 재정비중... : {0}", countdown);
        }
    }
}
