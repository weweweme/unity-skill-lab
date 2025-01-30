using InGame.Cases.TowerDefense.System;
using InGame.Cases.TowerDefense.System.Managers;
using InGame.System;
using Root.Util;
using UnityEngine;
using UnityEngine.Assertions;

namespace InGame.Cases.TowerDefense
{
    // TODO: 추후 시스템이 구현되면 사라질 클래스입니다
    public sealed class Tmp : MonoBehaviourBase
    {
        // TODO: DataManager에서 데이터를 가져오게 만들 예정입니다
        [SerializeField] private GameObject turret;

        private void Awake()
        {
            Assert.IsNotNull(turret);
        }

        public void InstantiateTurret()
        {
            TowerDefenseManager manager = InGameManager.Ins as TowerDefenseManager;
            manager.DataManager.MainPanel.SetSelectedTower(ETowerType.Default);
        }
    }
}
