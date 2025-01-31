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
        private bool _isUpdateActive;

        public TowerCreateHandler(TowerDefenseManager tdManager)
        {
            _dataManager = tdManager.DataManager;
            _pool = tdManager.PoolManager.TowerBasePool;

            _dataManager.MainPanel.SelectedTower
                .Subscribe(CreateTower)
                .AddTo(_disposable);
        }

        public void InitRx(TowerDefenseInputEventHandler eventHandler)
        {
            Observable.EveryUpdate()
                .Where(_ => _isUpdateActive) // 활성화된 경우에만 실행
                .Subscribe(_ => OnUpdate())
                .AddTo(_disposable);

            eventHandler.OnMouseScreenPositionEvent -= SetCorsorPosition;
            eventHandler.OnMouseScreenPositionEvent += SetCorsorPosition;
        }

        private void SetCorsorPosition(Vector2 worldMousePos)
        {
        }

        private void CreateTower(ETowerType type)
        {
            if (type == ETowerType.None) return;

            TowerRoot root = _pool.GetObject();

            _isUpdateActive = true;
            Debug.Log($"Tower {type} created. Update logic enabled.");
        }

        private void ClearCreateProcess()
        {
            _isUpdateActive = false;
            Debug.Log("Tower update logic disabled.");
        }

        private void OnUpdate()
        {
            Debug.Log("Update logic running...");
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
