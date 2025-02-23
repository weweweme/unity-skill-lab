using System;
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
        
        // 라운드 시작과 관련된 Rx
        private readonly Subject<Unit> _onRoundStart = new Subject<Unit>();
        public IObservable<Unit> OnRoundStart => _onRoundStart;
        public void StartRound() => _onRoundStart.OnNext(Unit.Default);
        
        // 라운드 종료와 관련된 Rx
        private readonly Subject<Unit> _onRoundEnd = new Subject<Unit>();
        public IObservable<Unit> OnRoundEnd => _onRoundEnd;
        public void EndRound() => _onRoundEnd.OnNext(Unit.Default);
    }
}
