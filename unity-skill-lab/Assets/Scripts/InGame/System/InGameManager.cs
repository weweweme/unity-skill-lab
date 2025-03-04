using Root.Managers;
using Root.Util;
using UnityEngine;

namespace InGame.System
{
    /// <summary>
    /// InGame에서 사용하는 루트 매니저 클래스입니다.
    /// 각 씬에서 사용되는 매니저들의 공통 참조를 관리하며, 
    /// 직렬화된 필드를 통해 다양한 매니저를 할당받아 사용합니다.
    /// 
    /// 사용 방법:
    /// 1. `[SerializeField] protected` 필드를 선언하여 Unity Inspector에서 할당할 수 있도록 설정합니다.
    /// 2. 하위 클래스에서 `SetXXXManager()` 메서드를 오버라이드하여 해당 필드를 올바른 타입으로 변환합니다.
    /// 3. `Awake()`에서 `SetXXXManager()`가 자동 호출되어, 필드가 정상적으로 할당되었는지 확인합니다.
    /// </summary>
    public abstract class InGameManager : Singleton<InGameManager>
    {
        [SerializeField] protected DataManager dataManager;
        [SerializeField] protected SequenceManager sequenceManager;
        [SerializeField] protected CameraManager cameraManager;
        public CameraManager CameraManager => cameraManager;
        
        private readonly UIBindManager _uiBindManager = new UIBindManager();
        public UIBindManager UIBindManager => _uiBindManager;
        
        protected override void Awake()
        {
            base.Awake();
            
            AssertHelper.NotNull(typeof(InGameManager), dataManager);
            SetDataManager();
            
            AssertHelper.NotNull(typeof(InGameManager), sequenceManager);
            SetSequenceManager();
            
            AssertHelper.NotNull(typeof(InGameManager), cameraManager);
        }
        
        private void Start()
        {
            Init();
            StartSequence();
        }

        protected virtual void Init()
        {
            _uiBindManager.Init(dataManager);
        }
        
        protected abstract void StartSequence();

        protected abstract void SetDataManager();
        protected abstract void SetSequenceManager();
    }
}
