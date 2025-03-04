using Root.Managers;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// 타워 디펜스 게임의 특정 시퀀스를 관리하는 클래스.
    /// </summary>
    public sealed class TowerDefenseSequenceManager : SequenceManager
    {
        private RoundController _roundController;
        
        public void Init(TowerDefenseManager towerDefenseManager)
        {
            _roundController = new RoundController(towerDefenseManager.DataManager);
        }

        public override void StartSequence()
        {
            _roundController.StartLoop();
        }

        public override void Clear()
        {
            _roundController.Dispose();
        }
    }
}
