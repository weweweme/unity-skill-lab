using UnityEngine;

namespace Root.Util
{
    /// <summary>
    /// 프로젝트 전역에서 사용할 레이어 정의 및 관련 유틸리티 메서드를 포함하는 클래스입니다.
    /// </summary>
    public static class Layers
    {
        public const int InValidArea = 6;  
        public const int Player = 31;

        /// <summary>
        /// 특정 GameObject가 여러 개의 지정된 레이어 중 하나에 속하는지 확인합니다.
        /// </summary>
        /// <param name="obj">확인할 GameObject</param>
        /// <param name="layers">비교할 레이어 목록</param>
        /// <returns>GameObject가 하나라도 해당 레이어에 속하면 true</returns>
        public static bool CompareLayer(GameObject obj, params int[] layers)
        {
            foreach (int layer in layers)
            {
                if (obj.layer == layer)
                    return true;
            }
            
            return false;
        }
    }
}
