using UnityEngine;
using UnityEngine.Assertions;

namespace Root.Util
{
    public static class ComponentHelper
    {
        /// <summary>
        /// GameObject에서 특정 컴포넌트를 가져오되, 없으면 Assert를 발생시킵니다.
        /// </summary>
        /// <typeparam name="T">찾을 컴포넌트 타입</typeparam>
        /// <param name="gameObject">대상 GameObject</param>
        /// <returns>찾은 컴포넌트</returns>
        public static T GetComponentOrAssert<T>(this GameObject gameObject) where T : Component
        {
            Assert.IsNotNull(gameObject, "[GetComponentOrAssert] 대상 GameObject가 존재하지 않습니다.");

            if (gameObject.TryGetComponent<T>(out var component))
            {
                return component;
            }

            Assert.IsNotNull(component, $"[GetComponentOrAssert] {typeof(T).Name} 컴포넌트가 {gameObject.name}에 없습니다.");
            return null;
        }
    }
}
