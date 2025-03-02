using InGame.System;
using Root.Util;
using TMPro;
using UnityEngine;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Current Wave Count UI의 View입니다.
    /// </summary>
    public sealed class VW_CurrentRoundCount : View
    {
        [SerializeField] private TextMeshProUGUI roundCount;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_CurrentRoundCount), roundCount);
        }
        
        public void SetWaveCount(uint waveCount) => roundCount.SetText(waveCount.ToString());
    }
}
