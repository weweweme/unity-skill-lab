using UnityEngine;
using UnityEngine.Pool;

namespace Root.Util
{
    public abstract class ObjectPoolBase<T> : MonoBehaviourBase where T : Component
    {
        /// <summary>
        /// 풀링할 오브젝트의 프리팹입니다.
        /// </summary>
        [SerializeField] private T prefab;

        /// <summary>
        /// 오브젝트 풀링 시스템을 관리하는 유니티의 ObjectPool 클래스입니다.
        /// </summary>
        private ObjectPool<T> objectPool;

        /// <summary>
        /// 풀의 최소 사이즈입니다.
        /// 풀 초기화 시 이 값이 사용됩니다.
        /// </summary>
        [SerializeField] private int defaultCapacity = 10;

        /// <summary>
        /// 풀의 최대 사이즈입니다.
        /// 오브젝트가 이 수치를 넘으면 자동으로 반환되거나 파괴됩니다.
        /// </summary>
        [SerializeField] private int maxSize = 20;

        /// <summary>
        /// 오브젝트 풀을 초기화하는 메서드입니다.
        /// Unity의 Awake 이벤트에서 호출됩니다.
        /// </summary>
        protected virtual void Awake()
        {
            objectPool = new ObjectPool<T>(
                CreatePooledItem, 
                OnTakeFromPool, 
                OnReturnedToPool, 
                OnDestroyPoolObject, 
                true, 
                defaultCapacity, 
                maxSize);
        }

        /// <summary>
        /// 풀에 새로운 오브젝트를 생성하는 메서드입니다.
        /// </summary>
        /// <returns>새로 생성된 오브젝트</returns>
        protected virtual T CreatePooledItem()
        {
            return Instantiate(prefab);
        }

        /// <summary>
        /// 풀에서 오브젝트를 가져올 때 호출되는 메서드입니다.
        /// 오브젝트를 활성화합니다.
        /// </summary>
        /// <param name="item">풀에서 가져온 오브젝트</param>
        protected virtual void OnTakeFromPool(T item)
        {
            item.gameObject.SetActive(true);
        }

        /// <summary>
        /// 오브젝트가 풀로 반환될 때 호출되는 메서드입니다.
        /// 오브젝트를 비활성화합니다.
        /// </summary>
        /// <param name="item">풀로 반환된 오브젝트</param>
        protected virtual void OnReturnedToPool(T item)
        {
            item.gameObject.SetActive(false);
        }

        /// <summary>
        /// 풀에서 제거된 오브젝트를 파괴하는 메서드입니다.
        /// </summary>
        /// <param name="item">파괴할 오브젝트</param>
        protected virtual void OnDestroyPoolObject(T item)
        {
            Destroy(item.gameObject);
        }

        /// <summary>
        /// 풀에서 오브젝트를 가져옵니다.
        /// </summary>
        /// <returns>풀에서 가져온 오브젝트</returns>
        public T GetObject()
        {
            return objectPool.Get();
        }

        /// <summary>
        /// 사용한 오브젝트를 풀로 반환합니다.
        /// </summary>
        /// <param name="item">풀로 반환할 오브젝트</param>
        public void ReturnObject(T item)
        {
            objectPool.Release(item);
        }
    }
}
