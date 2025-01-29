using UnityEngine;

namespace Root.Util
{
    public class Singleton<T> : MonoBehaviourBase where T : Singleton<T>
    {
        /// <summary>
        /// 제네릭 싱글톤 패턴을 구현한 클래스.
        /// 이 클래스를 상속하여 싱글톤 패턴을 구현할 수 있습니다.
        /// </summary>
        /// <typeparam name="T">싱글톤으로 사용할 클래스 타입.</typeparam>
        [Tooltip("씬 전환 시 객체를 삭제할지 여부를 결정합니다.")] 
        [SerializeField] private bool _isDestroyOnLoad;

        /// <summary>
        /// 싱글톤 인스턴스를 저장하는 정적 변수.
        /// </summary>
        private static T _ins;
    
        /// <summary>
        /// 싱글톤 인스턴스에 접근하기 위한 프로퍼티.
        /// </summary>
        public static T Ins => _ins;

        /// <summary>
        /// 싱글톤 인스턴스를 초기화하는 메서드.
        /// 이미 인스턴스가 존재하면 객체를 파괴하고, 존재하지 않으면 인스턴스를 설정합니다.
        /// </summary>
        protected virtual void InitSingleton()
        {
            if (_ins == null)
            {
                _ins = this as T;

                if (!_isDestroyOnLoad)
                {
                    DontDestroyOnLoad(this);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        protected virtual void Awake()
        {
            InitSingleton();
        }
    
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnDispose();
        }

        /// <summary>
        /// 인스턴스가 파괴될 때 추가로 처리할 작업을 정의하는 메서드.
        /// 여기서 싱글톤 인스턴스를 null로 설정합니다.
        /// </summary>
        protected virtual void OnDispose()
        {
            _ins = null;
        }
    }
}
