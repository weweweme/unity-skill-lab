using InGame.System;

namespace InGame.Cases.TowerDefense.UI
{
    /// <summary>
    /// Enemy Count Display UI를 관리하는 프레젠터 클래스입니다.
    /// </summary>
    public sealed class PR_EnemyCountDisplay : Presenter
    {
        private uint _aliveEnemyCount;
        
        public override void Init(DataManager dataManager, View view)
        {
        }
    }
}
