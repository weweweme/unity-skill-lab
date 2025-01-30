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

        // TODO: TurretInstantiate를 담당하는 클래스에서 수행될 예정입니다
        public void InstantiateTurret()
        {
            Instantiate(turret);
        }
    }
}
