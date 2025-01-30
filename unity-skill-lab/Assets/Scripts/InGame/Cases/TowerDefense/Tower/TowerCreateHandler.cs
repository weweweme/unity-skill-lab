using System;
using InGame.Cases.TowerDefense.System;
using InGame.Cases.TowerDefense.System.Managers;
using UniRx;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Tower
{
    /// <summary>
    /// 타워 생성과 관련된 비즈니스 로직을 담당하는 핸들러 클래스입니다.
    /// </summary>
    public sealed class TowerCreateHandler : IDisposable
    {
        private readonly TowerDefenseDataManager _dataManager;
        private readonly TowerBasePool _pool;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public TowerCreateHandler(TowerDefenseManager tdManager)
        {
            _dataManager = tdManager.DataManager;
            _pool = tdManager.PoolManager.TowerBasePool;
            
            _dataManager.MainPanel.SelectedTower
                .Subscribe(CreateTower)
                .AddTo(_disposable);
        }

        private void CreateTower(ETowerType type)
        {
            // 유효하지 않은 감시값일 경우 return
            if (type == ETowerType.None) return;
            
            // TODO: 타워 타입에 따라 DataManager에서 데이터를 가져와 셋업하는 코드 구현.
            TowerRoot root = _pool.GetObject();
            Debug.Log($"CreateTower 호출");
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
