using UnityEngine;

namespace Root.Util
{
    /// <summary>
    /// SpriteRenderer관련 헬퍼 클래스입니다.
    /// </summary>
    public static class SpriteRendererHelper
    {
        /// <summary>
        /// 지정된 SpriteRenderer의 투명도를 설정합니다.
        /// </summary>
        /// <param name="spriteRenderer">변경할 SpriteRenderer</param>
        /// <param name="alpha">설정할 알파(투명도) 값</param>
        public static void SetOpacity(SpriteRenderer spriteRenderer, float alpha)
        {
            AssertHelper.NotNull(typeof(SpriteRendererHelper), spriteRenderer);
            
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

        /// <summary>
        /// 지정된 SpriteRenderer의 색상을 변경합니다. 기존의 알파(투명도) 값은 유지됩니다.
        /// </summary>
        /// <param name="spriteRenderer">변경할 SpriteRenderer</param>
        /// <param name="color">설정할 색상 값</param>
        public static void SetColor(SpriteRenderer spriteRenderer, Color color)
        {
            AssertHelper.NotNull(typeof(SpriteRendererHelper), spriteRenderer);
            
            Color currentColor = spriteRenderer.color;
            color.a = currentColor.a; // 기존 알파값 유지
            spriteRenderer.color = color;
        }
    }
}
