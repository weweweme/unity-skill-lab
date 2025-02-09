using System;
using UniRx;

namespace InGame.Cases.TowerDefense.System.Model
{
    /// <summary>
    /// 타워 디펜스 게임에서 타워와 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_Tower
    {
        // 빌드를 위해 선택된 타워와 관련된 Rx
        private readonly Subject<ETowerType> _towerToBuild = new Subject<ETowerType>();
        public IObservable<ETowerType> TowerToBuild => _towerToBuild;
        public void SetTowerToBuild(ETowerType type) => _towerToBuild.OnNext(type);
    }
}
