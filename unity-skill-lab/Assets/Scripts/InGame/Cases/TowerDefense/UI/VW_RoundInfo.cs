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
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_RoundInfo), roundInfo);
        }
    }
}
