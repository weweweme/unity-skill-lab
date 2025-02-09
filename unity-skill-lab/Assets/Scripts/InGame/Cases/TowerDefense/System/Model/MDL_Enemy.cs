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
        private readonly Subject<ETowerType> _onEnemySpawn = new Subject<ETowerType>();
        public IObservable<ETowerType> OnEnemySpawn => _onEnemySpawn;
        public void SpawnEnemy(ETowerType type) => _onEnemySpawn.OnNext(type);
    }
}
