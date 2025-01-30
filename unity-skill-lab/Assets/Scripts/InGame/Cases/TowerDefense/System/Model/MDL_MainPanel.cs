using UniRx;

namespace InGame.Cases.TowerDefense.System.Model
{
    /// <summary>
    /// 타워 디펜스 게임의 메인 패널 모델 클래스입니다.
    /// </summary>
    public class MDL_MainPanel
    {
        /// <summary>
        /// 현재 선택된 타워 상태 (Rx)
        /// </summary>
        private readonly ReactiveProperty<ETowerType> _selectedTower = new(ETowerType.None);
        public IReadOnlyReactiveProperty<ETowerType> SelectedTower => _selectedTower;
        public void SetSelectedTower(ETowerType newState) => _selectedTower.Value = newState;
    }
}
