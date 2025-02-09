using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InGame.Cases.TowerDefense.System.Managers;
using InGame.Cases.TowerDefense.System.Model;
using Root.Util;

namespace InGame.Cases.TowerDefense.System
{
    /// <summary>
    /// 타워 디펜스 게임의 라운드를 관리하는 클래스.
    /// 라운드 진행 흐름을 제어하며, 웨이브 시작 및 종료 등의 동작을 수행한다.
    /// </summary>
    public class RoundController : IDisposable
    {
        private readonly MDL_Enemy _enemyModel;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public RoundController(TowerDefenseDataManager dataManager)
        {
            _enemyModel = dataManager.Enemy;
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
            for (int i = 0; i < 5; i++)
            {
                if (token.IsCancellationRequested) return;
                
                _enemyModel.SpawnEnemy(EEnemyType.Default);
                
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }
        }

        /// <summary>
        /// 라운드가 종료될 때까지 기다린다.
        /// </summary>
        private async UniTask EndRound(CancellationToken token)
        {
            // TODO: 추후 Round와 관련된 모델 클래스를 추가하고, 이벤트를 활용하여 종료 조건을 설정하기
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
            await UniTask.Delay(TimeSpan.FromSeconds(10), cancellationToken: token);
        }

        /// <summary>
        /// 현재 라운드가 종료되었는지 확인한다.
        /// </summary>
        private bool IsRoundOver()
        {
            // TODO: 적이 모두 처치되거나 EndPoint까지 도달했는지 확인하는 로직 추가
            return false;
        }

        /// <summary>
        /// 루프를 중단하고 리소스를 정리한다.
        /// </summary>
        public void Dispose()
        {
            CancelTokenHelper.ClearToken(in _cts);
        }
    }
}
