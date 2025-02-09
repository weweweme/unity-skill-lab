using InGame.Cases.TowerDefense.System.Managers;

namespace InGame.Cases.TowerDefense.System
{
    /// <summary>
    /// 타워 디펜스 게임의 라운드를 관리하는 클래스.
    /// 라운드 진행 흐름을 제어하며, 웨이브 시작 및 종료 등의 동작을 수행한다.
    /// </summary>
    public class RoundController
    {
        private readonly TowerDefensePoolManager _poolManager;
        
        public RoundController(TowerDefenseManager towerDefenseManager)
        {
            _poolManager = towerDefenseManager.PoolManager;
        }

        /// <summary>
        /// 라운드의 루프를 시작한다.
        /// </summary>
        public void StartLoop()
        {
            
        }
    }
}
