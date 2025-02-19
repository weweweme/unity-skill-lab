using System;
using UniRx;

namespace Root.UI
{
    /// <summary>
    /// 프로젝트의 MVP UI 구조에서 사용되는 Presenter 추상 클래스입니다.
    /// 각 UI Presenter는 이 클래스를 상속받아 구체적인 UI 로직을 구현합니다.
    /// </summary>
    public abstract class Presenter : IDisposable
    {
        /// <summary>
        /// UniRx의 CompositeDisposable을 사용하여 Rx 관련 구독을 관리합니다.
        /// Dispose 시 모든 구독을 해제하여 메모리 누수를 방지합니다.
        /// </summary>
        protected readonly CompositeDisposable disposable = new CompositeDisposable();

        /// <summary>
        /// Presenter의 초기화를 수행하는 추상 메서드입니다.
        /// 해당 Presenter가 관리할 View를 전달받아 초기 설정을 진행합니다.
        /// </summary>
        /// <param name="view">초기화할 View 객체</param>
        public abstract void Init(View view);
       
        /// <summary>
        /// CompositeDisposable의 리소스를 정리하여 메모리 누수를 방지하는 Dispose 메서드입니다.
        /// Presenter가 더 이상 사용되지 않을 때 호출해야 합니다.
        /// </summary>
        public virtual void Dispose()
        {
            disposable.Dispose();
        }
    }
}
