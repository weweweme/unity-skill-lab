using Root.Util;

namespace Root.Managers
{
    /// <summary>
    /// 게임 내 특정 시퀀스를 관리하는 추상 클래스.
    /// 이를 상속받아 각 시퀀스의 구체적인 동작을 구현합니다.
    /// </summary>
    public abstract class SequenceManager : MonoBehaviourBase
    {
        public abstract void StartSequence(); // 시퀀스 시작
        public abstract void Clear();         // 데이터 정리
    }
}
