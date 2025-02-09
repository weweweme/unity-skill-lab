using System;
using UniRx;

namespace InGame.Cases.TowerDefense.System.Model
{
    /// <summary>
    /// 타워 디펜스 게임에서 적 유닛과 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_Enemy
    {
        // 적 유닛 스폰과 관련된 Rx
        private readonly Subject<EEnemyType> _onEnemySpawn = new Subject<EEnemyType>();
        public IObservable<EEnemyType> OnEnemySpawn => _onEnemySpawn;
        public void SpawnEnemy(EEnemyType type) => _onEnemySpawn.OnNext(type);
    }
}
