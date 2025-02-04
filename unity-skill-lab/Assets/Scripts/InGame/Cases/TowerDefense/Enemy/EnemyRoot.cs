using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Enemy
{
    /// <summary>
    /// Enemy의 최상위 루트 클래스입니다.
    /// 각 모듈의 참조를 관리하고, 초기화 과정을 수행합니다.
    /// </summary>
    public sealed class EnemyRoot : MonoBehaviourBase
    {
        [SerializeField] private EnemyBTBase btBase;
        public EnemyBTBase BTBase => btBase;
        
        [SerializeField] private EnemyStatController statController;
        public EnemyStatController StatController => statController;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(EnemyRoot), btBase);
            AssertHelper.NotNull(typeof(EnemyRoot), statController);
        }
    }
}
