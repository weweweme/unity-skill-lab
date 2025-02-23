using InGame.System;
using Root.Util;
using TMPro;
using UnityEngine;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Enemy Count Display UI를 관리하는 View 입니다.
    /// </summary>
    public sealed class VW_EnemyCountDisplay : View
    {
        [SerializeField] private TextMeshProUGUI currentAliveEnemyCount;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_EnemyCountDisplay), currentAliveEnemyCount);
        }
        
        public void SetAliveEnemyCount(uint aliveEnemyCount) => currentAliveEnemyCount.SetText(aliveEnemyCount.ToString());
    }
}
