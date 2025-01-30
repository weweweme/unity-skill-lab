using Root.Util;
using UnityEngine;
using UnityEngine.Assertions;

namespace InGame.System
{
    /// <summary>
    /// InGame에서 사용하는 루트 매니저 클래스입니다.
    /// 각 씬에서 사용되는 매니저들의 참조를 관리합니다.
    /// </summary>
    public abstract class InGameManager : Singleton<InGameManager>
    {
        /// <summary>
        /// 현재 씬에서 사용되는 데이터 매니저입니다.
        /// 모든 InGameManager는 반드시 이를 참조해야 합니다.
        /// </summary>
        [SerializeField] private DataManager dataManager;
        public DataManager DataManager => dataManager;
        
        protected override void Awake()
        {
            base.Awake();
            
            Assert.IsNotNull(dataManager, "[InGameManager] DataManager가 할당되지 않았습니다.");
        }
    }
}
