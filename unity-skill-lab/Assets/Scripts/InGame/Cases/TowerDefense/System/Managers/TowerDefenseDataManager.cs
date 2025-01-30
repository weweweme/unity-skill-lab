using InGame.Cases.TowerDefense.System.Model;
using InGame.System;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// Tower Defense 씬에서 사용할 데이터 매니저입니다.
    /// </summary>
    public sealed class TowerDefenseDataManager : DataManager
    {
        private readonly MDL_MainPanel _mainPanel = new MDL_MainPanel();
        public MDL_MainPanel MainPanel => _mainPanel;
    }
}
