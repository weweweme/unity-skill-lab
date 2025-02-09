using InGame.Cases.TowerDefense.System.Managers;

namespace InGame.Cases.TowerDefense.System
{
    public class RoundController
    {
        private readonly TowerDefensePoolManager _poolManager;
        
        public RoundController(TowerDefenseManager towerDefenseManager)
        {
            _poolManager = towerDefenseManager.PoolManager;
        }
        
        public void StartLoop()
        {
            
        }
    }
}
