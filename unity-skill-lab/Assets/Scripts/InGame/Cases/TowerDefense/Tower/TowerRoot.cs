using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Tower
{
    /// <summary>
    /// Tower의 최상위 루트 클래스입니다.
    /// 각 모듈의 참조를 관리하고, 초기화 과정을 수행합니다.
    /// </summary>
    public sealed class TowerRoot : MonoBehaviourBase
    {
        [SerializeField] private TowerPlacementController placementController;
        public TowerPlacementController PlacementController => placementController;
        
        [SerializeField] private TowerAttackController attackController;
        public TowerAttackController AttackController => attackController;

        private TowerBTController _btController;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(TowerRoot), placementController);
            AssertHelper.NotNull(typeof(TowerRoot), attackController);
        }

        private void Start()
        {
            placementController.OnPlaced -= Init;
            placementController.OnPlaced += Init;
        }

        private void Init()
        {
            _btController = new TowerBTController(gameObject, attackController);
            attackController.Init();
        }
    }
}
