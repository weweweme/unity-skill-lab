using Root.UI;
using Root.Util;
using TMPro;
using UnityEngine;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Current Wave Count UI의 View입니다.
    /// </summary>
    public sealed class VW_CurrentWaveCount : View
    {
        [SerializeField] private TextMeshProUGUI textWaveCount;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_CurrentWaveCount), textWaveCount);
        }
    }
}
