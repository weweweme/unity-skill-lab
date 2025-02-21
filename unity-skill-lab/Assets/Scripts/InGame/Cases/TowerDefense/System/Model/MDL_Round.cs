using UniRx;

namespace InGame.Cases.TowerDefense.System.Model
{
    /// <summary>
    /// 타워 디펜스 게임에서 라운드와 관련된 데이터를 관리하는 모델 클래스
    /// </summary>
    public class MDL_Round
    {
        private readonly ReactiveProperty<uint> _currentRound = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> CurrentRound => _currentRound;
        public void SetCurrentRound(uint round) => _currentRound.Value = round;
    }
}
