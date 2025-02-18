using InGame.Cases.TowerDefense.System.Model;
using InGame.System;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// Tower Defense 씬에서 사용할 데이터 매니저입니다.
    /// </summary>
    public sealed class TowerDefenseDataManager : DataManager
    {
        public MDL_Tower Tower { get; } = new MDL_Tower();
        public MDL_Enemy Enemy { get; } = new MDL_Enemy();
        public MDL_Round Round { get; } = new MDL_Round();
    }
}
