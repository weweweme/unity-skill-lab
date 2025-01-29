using System.Threading;

namespace Root.Util
{
    /// <summary>
    /// CancellationTokenSource의 생성, 관리 및 해제를 돕는 헬퍼 클래스입니다.
    /// </summary>
    public static class CancelTokenHelper
    {
        /// <summary>
        /// CancellationTokenSource가 null이라면 새로 생성하여 반환하는 메서드.
        /// </summary>
        /// <param name="cts">기존 혹은 새로 생성된 CancellationTokenSource</param>
        public static void GetToken(ref CancellationTokenSource cts)
        {
            // null이거나 이미 취소되었으면 새로 생성
            if (cts == null || cts.IsCancellationRequested)
            {
                cts?.Dispose(); // 기존 토큰이 있으면 Dispose
                cts = new CancellationTokenSource(); // 새로 생성
            }
        }

        /// <summary>
        /// 주어진 CancellationTokenSource를 취소하고, Dispose까지 처리하는 메서드.
        /// </summary>
        /// <param name="cts">취소하고 Dispose할 CancellationTokenSource</param>
        public static void ClearToken(in CancellationTokenSource cts)
        {
            if (cts == null)
                return;

            if (!cts.IsCancellationRequested)
            {
                cts?.Cancel();   // 취소
            }
            
            cts?.Dispose();  // 리소스 해제
        }
    }
}
