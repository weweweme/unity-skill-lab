using System;
using Root.Util;

namespace InGame.System
{
    /// <summary>
    /// Presenter와 View의 초기화를 담당하는 ViewController 클래스 입니다.
    /// Presenter와 View는 한 세트로 묶여 1:1로 대응됩니다. ViewController는 이를 여러 쌍 가지고 있을 수 있습니다.
    /// 두 모듈의 참조를 가지고 초기화만 수행합니다. 비즈니스 로직은 절대 담당하지 않습니다.
    /// ViewController를 상속받는 모든 클래스는 접두사 VC가 붙습니다.
    /// </summary>
    public abstract class ViewController : MonoBehaviourBase, IDisposable
    {
        private void Awake()
        {
            ValidateReferences();
            RegisterToUIManager();
        }
        
        /// <summary>
        /// 필수 참조가 정상적으로 설정되었는지 검증합니다.
        /// </summary>
        protected abstract void ValidateReferences();

        /// <summary>
        /// UIManager에 ViewController를 등록하는 역할을 수행합니다.
        /// 각 ViewController가 적절한 UI 관리 시스템과 연결될 수 있도록 설정합니다.
        /// </summary>
        private void RegisterToUIManager()
        {
            InGameManager.Ins.UIBindManager.AddViewController(this);
        }
        
        /// <summary>
        /// DataManager의 참조를 받아와 필요한 모델의 참조를 통해 Presenter의 초기화를 수행합니다.
        /// Presenter가 데이터에 접근할 수 있도록 설정하는 역할을 합니다.
        /// </summary>
        public abstract void Init(DataManager dataManager);
        
        /// <summary>
        /// 프레젠터와 뷰의 자원을 정리할 때 사용합니다.
        /// TowerDefenseUIManager에서 Dispose를 호출됩니다.
        /// </summary>
        public abstract void Dispose();
    }
}
