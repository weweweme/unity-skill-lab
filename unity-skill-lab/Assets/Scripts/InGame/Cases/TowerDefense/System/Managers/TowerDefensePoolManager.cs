using InGame.Cases.TowerDefense.Tower.Pool;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// 타워 디펜스 게임에서 사용되는 풀들의 참조를 클래스입니다.
    /// </summary>
    public sealed class TowerDefensePoolManager : MonoBehaviourBase
    {
        [SerializeField] private TowerBasePool towerBasePool;
        public TowerBasePool TowerBasePool => towerBasePool;
        
        [SerializeField] private TowerProjectileBasePool towerProjectileBasePool;
        public TowerProjectileBasePool TowerProjectileBasePool => towerProjectileBasePool;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(TowerDefensePoolManager), towerBasePool);
            AssertHelper.NotNull(typeof(TowerDefensePoolManager), towerProjectileBasePool);
        }
    }
}
