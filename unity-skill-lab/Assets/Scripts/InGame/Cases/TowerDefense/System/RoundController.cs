using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InGame.Cases.TowerDefense.System.Managers;
using InGame.Cases.TowerDefense.System.Model;
using Root.Util;
using UniRx;

namespace InGame.Cases.TowerDefense.System
{
    /// <summary>
    /// 타워 디펜스 게임의 라운드를 관리하는 클래스.
    /// 라운드 진행 흐름을 제어하며, 웨이브 시작 및 종료 등의 동작을 수행한다.
    /// </summary>
    public sealed class RoundController : IDisposable
    {
        private readonly MDL_Enemy _enemyModel;
        private readonly MDL_Round _roundModel;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly Subject<Unit> _roundEndEvent = new Subject<Unit>();

        private uint _remainingEnemyCount;

        public RoundController(TowerDefenseDataManager dataManager)
        {
            _enemyModel = dataManager.Enemy;
            _roundModel = dataManager.Round;

            _enemyModel.OnEnemySpawn
                .Subscribe(_ => ++_remainingEnemyCount)
                .AddTo(_disposable);
            
            _enemyModel.OnEnemyDeath
                .Subscribe(_ =>
                {
                    --_remainingEnemyCount;
                    
                    // InProgress 상태가 아닐 경우, 종료 조건을 체크하지 않음
                    if (_roundModel.RoundState.Value != ERoundStates.InProgress) return;
                    // 활성화된 적이 남아있을 경우 종료 조건을 체크하지 않음
                    if (_remainingEnemyCount > 0) return;
                    
                    _roundEndEvent.OnNext(Unit.Default);
                })
                .AddTo(_disposable);
        }

        /// <summary>
        /// 라운드의 루프를 시작한다.
        /// </summary>
        public void StartLoop()
        {
            LoopAsync(_cts.Token).Forget();
        }

        /// <summary>
        /// 라운드 루프를 실행하는 비동기 메서드.
        /// </summary>
        private async UniTaskVoid LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await StartRound(token);
                await EndRound(token);
                await WaitForNextRound(token);
            }
        }

        /// <summary>
        /// 라운드를 시작하고 적을 소환한다.
        /// </summary>
        private async UniTask StartRound(CancellationToken token)
        {
            const int ROUND_INCREMENT = 1;
            _roundModel.SetCurrentRound(_roundModel.CurrentRound.Value + ROUND_INCREMENT);
            _roundModel.SetRoundState(ERoundStates.Spawning);
            
            for (int i = 0; i < 5; i++)
            {
                if (token.IsCancellationRequested) return;
                
                _enemyModel.SpawnEnemy(EEnemyType.Default);
                
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }
            
            _roundModel.SetRoundState(ERoundStates.InProgress);
        }

        /// <summary>
        /// 라운드가 종료될 때까지 기다린다.
        /// </summary>
        private async UniTask EndRound(CancellationToken token)
        {
            while (!IsRoundOver())
            {
                if (token.IsCancellationRequested) return;
                
                await UniTask.Yield();
            }
        }

        /// <summary>
        /// 라운드 종료 후 대기 시간.
        /// </summary>
        private async UniTask WaitForNextRound(CancellationToken token)
        {
            _roundModel.SetRoundState(ERoundStates.Waiting);
            
            await UniTask.Delay(TimeSpan.FromSeconds(10), cancellationToken: token);
        }

        /// <summary>
        /// 현재 라운드가 종료되었는지 확인한다.
        /// </summary>
        private bool IsRoundOver()
        {
            // 라운드 진행 중 상태가 아닐 경우, 종료 조건을 체크하지 않음
            if (_roundModel.RoundState.Value != ERoundStates.InProgress)
            {
                return false;
            }

            // 활성화된 적이 모두 사라졌을 경우, 라운드 종료
            return _remainingEnemyCount == 0;
        }

        /// <summary>
        /// 루프를 중단하고 리소스를 정리한다.
        /// </summary>
        public void Dispose()
        {
            CancelTokenHelper.ClearToken(in _cts);
            _disposable.Dispose();
        }
    }
}
