namespace Root.Util
{
    /// <summary>
    /// 프로젝트 전역에서 사용할 레이어 정의 및 관련 유틸리티 메서드를 포함하는 클래스입니다.
    /// </summary>
    public static class Layers
    {
        public const int Enemy = 3;  
        public const int InValidArea = 6;  
        public const int Player = 31;

        /// <summary>
        /// 여러 개의 레이어를 받아서 LayerMask를 생성합니다.
        /// </summary>
        /// <param name="layers">LayerMask로 만들 레이어 목록</param>
        /// <returns>여러 레이어를 포함한 LayerMask</returns>
        public static int GetLayerMask(params int[] layers)
        {
            int mask = 0;
            foreach (int layer in layers)
            {
                mask |= (1 << layer); // 비트 연산을 통해 LayerMask 생성
            }
            return mask;
        }

        /// <summary>
        /// 특정 GameObject의 레이어가 지정된 레이어 중 하나인지 확인합니다.
        /// </summary>
        /// <param name="targetLayer">확인할 GameObject의 레이어</param>
        /// <param name="layers">비교할 레이어 목록</param>
        /// <returns>GameObject가 하나라도 해당 레이어에 속하면 true</returns>
        public static bool CompareLayer(int targetLayer, params int[] layers)
        {
            foreach (int layer in layers)
            {
                if (targetLayer == layer)
                    return true;
            }
            return false;
        }
    }
}
