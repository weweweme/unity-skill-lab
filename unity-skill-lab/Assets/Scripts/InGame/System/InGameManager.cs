using Root.Util;

namespace InGame.System
{
    /// <summary>
    /// InGame에서 사용하는 루트 매니저 클래스입니다.
    /// 각 씬에서 사용되는 매니저들의 참조를 관리합니다.
    /// </summary>
    public abstract class InGameManager : Singleton<InGameManager>
    {
    }
}
