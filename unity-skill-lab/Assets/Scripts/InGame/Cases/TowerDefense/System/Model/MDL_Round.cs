using UniRx;

namespace InGame.Cases.TowerDefense.System.Model
{
    /// <summary>
    /// 타워 디펜스 게임에서 라운드와 관련된 데이터를 관리하는 모델 클래스
    /// </summary>
    public class MDL_Round
    {
        // 현재 몇 라운드인지 나타내는 Rx
        private readonly ReactiveProperty<uint> _currentRound = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> CurrentRound => _currentRound;
        public void SetCurrentRound(uint round) => _currentRound.Value = round;
        
        // 현재 라운드의 상태를 나타내는 Rx
        private readonly ReactiveProperty<ERoundStates> _roundState = new ReactiveProperty<ERoundStates>(ERoundStates.None);
        public IReadOnlyReactiveProperty<ERoundStates> RoundState => _roundState;
        public void SetRoundState(ERoundStates state) => _roundState.Value = state;
        
        // 다음 라운드까지의 카운트다운을 나타내는 Rx
        private readonly ReactiveProperty<uint> _nextRoundCountDown = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> NextRoundCountDown => _nextRoundCountDown;
        public void SetNextRoundCountDown(uint countDown) => _nextRoundCountDown.Value = countDown;
    }
}
