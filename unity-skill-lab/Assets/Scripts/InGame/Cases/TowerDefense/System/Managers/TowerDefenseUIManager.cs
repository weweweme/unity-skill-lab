namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// Tower Defense 씬에서 UI를 관리하는 클래스입니다.
    /// 씬에서 사용되는 프레젠터들의 참조와 초기화를 관리합니다.
    /// </summary>
    public class TowerDefenseUIManager
    {
        public TowerDefenseUIManager(TowerDefenseManager towerDefenseManager)
        {
            Init(towerDefenseManager.DataManager);
        }
        
        private void Init(TowerDefenseDataManager dataManager)
        {
            
        }
    }
}
