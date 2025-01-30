using Root.Util;

namespace InGame.System
{
    /// <summary>
    /// 게임 내 데이터 관리를 위한 추상 클래스입니다.
    /// 각 Case 씬(예: TowerDefense, 다른 게임 모드 등)의 데이터 매니저가 이를 상속받아 구현됩니다.
    /// 모든 데이터 매니저는 InGameManager에 등록되어 사용됩니다.
    /// </summary>
    public abstract class DataManager : MonoBehaviourBase
    {
    }
}
