using System;
using System.Threading;
using CleverCrow.Fluid.BTs.Trees;
using Cysharp.Threading.Tasks;
using Root.Util;

namespace Root
{
    /// <summary>
    /// 행동 트리(Behavior Tree)의 기본 구조를 제공하는 추상 클래스입니다.
    /// </summary>
    public abstract class BehaviourTreeBase : IDisposable
    {
        /// <summary>
        /// 행동 트리 틱(Tick)의 실행 간격 (밀리초 단위)
        /// </summary>
        private const int TICK_INTERVAL = 50;
        
        /// <summary>
        /// 행동 트리의 실행을 제어하는 CancellationTokenSource입니다.
        /// </summary>
        private CancellationTokenSource _cts;
        
        /// <summary>
        /// CleverCrow Fluid Behavior Tree 인스턴스
        /// </summary>
        private BehaviorTree _bt;
        
        /// <summary>
        /// 행동 트리를 생성하는 추상 메서드입니다.
        /// 상속받은 클래스에서 구체적인 행동 트리를 구현해야 합니다.
        /// </summary>
        /// <returns>구성된 BehaviorTree 인스턴스</returns>
        protected abstract BehaviorTree CreateTree();
        
        public void Init()
        {
            _bt = CreateTree();
            StartBtTick();
        }
        
        private void StartBtTick()
        {
            CancelTokenHelper.GetToken(ref _cts);
            TickBtAsync(_cts.Token).Forget();
        }
        
        private void StopBtTick()
        {
            CancelTokenHelper.ClearToken(in _cts);
        }
        
        /// <summary>
        /// 행동 트리 틱을 특정 시간 간격으로 반복 실행합니다.
        /// </summary>
        /// <param name="token">취소 가능성을 가진 CancellationToken</param>
        private async UniTask TickBtAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TICK_INTERVAL, cancellationToken: token);

                bool isCanceled = token.IsCancellationRequested;
                if (isCanceled)
                {
                    break;
                }

                _bt.Tick();
            }
        }

        /// <summary>
        /// 객체가 해제될 때 행동 트리의 실행을 중지합니다.
        /// </summary>
        public void Dispose()
        {
            StopBtTick();
        }
    }
}
