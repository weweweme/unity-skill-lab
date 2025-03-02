namespace InGame.Cases.TowerDefense.System
{
    /// <summary>
    /// 타워의 종류를 나타내는 열거형입니다.
    /// </summary>
    public enum ETowerType
    {
        /// <summary>
        /// 선택된 타워가 없음.
        /// </summary>
        None,

        /// <summary>
        /// 기본 타워.
        /// </summary>
        Default,
    }
    
    /// <summary>
    /// 적의 종류를 나타내는 열거형입니다.
    /// </summary>
    public enum EEnemyType
    {
        /// <summary>
        /// 선택된 적 없음.
        /// </summary>
        None,
        
        /// <summary>
        /// 기본 적.
        /// </summary>
        Default,
    }
    
    /// <summary>
    /// 적의 상태를 나타내는 열거형입니다.
    /// </summary>
    public enum EEnemyState
    {
        /// <summary>
        /// 상태 없음.
        /// </summary>
        None,

        /// <summary>
        /// 생존 상태.
        /// </summary>
        Alive,

        /// <summary>
        /// 사망 상태.
        /// </summary>
        Dead,
    }

    /// <summary>
    /// 라운드의 진행 상태를 나타내는 열거형입니다.
    /// </summary>
    public enum ERoundStates
    {
        /// <summary>
        /// 상태 없음.
        /// </summary>
        None,

        /// <summary>
        /// 적 소환 중.
        /// </summary>
        Spawning,
        
        /// <summary>
        /// 라운드 진행 중.
        /// </summary>
        InProgress,

        /// <summary>
        /// 다음 라운드 대기 중.
        /// </summary>
        Waiting,
    }
}
