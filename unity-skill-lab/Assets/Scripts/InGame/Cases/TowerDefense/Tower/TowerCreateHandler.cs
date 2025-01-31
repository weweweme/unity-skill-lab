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
        private const float Z_COORD = 0f;
        private const float SMOOTH_SPEED = 10f;
        
        private readonly TowerDefenseDataManager _dataManager;
        private readonly TowerBasePool _pool;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly Camera mainCam;
        private bool _isUpdateActive;
        private Vector2 _viewportMousePos;

        private TowerRoot _pendingTower;

        public TowerCreateHandler(TowerDefenseManager tdManager)
        {
            _dataManager = tdManager.DataManager;
            _pool = tdManager.PoolManager.TowerBasePool;
            
            // TODO: 추후 CamaraManager에서 MainCam을 가져오도록 변경
            mainCam = Camera.main;

            _dataManager.MainPanel.SelectedTower
                .Subscribe(CreateTower)
                .AddTo(_disposable);
        }

        public void InitRx(TowerDefenseInputEventHandler eventHandler)
        {
            Observable.EveryUpdate()
                .Where(_ => _isUpdateActive)
                .Subscribe(_ => OnUpdate())
                .AddTo(_disposable);

            eventHandler.OnMouseScreenPositionEvent -= SetCursorPosition;
            eventHandler.OnMouseScreenPositionEvent += SetCursorPosition;
        }

        private void SetCursorPosition(Vector2 screenMousePos)
        {
            Debug.Log($"SetCursorPosition: {screenMousePos}");
            
            // 마우스의 스크린 좌표를 직접 월드 좌표로 변환
            Vector3 worldMousePos = mainCam.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Z_COORD));
            worldMousePos.z = Z_COORD; // Z 좌표 고정

            _viewportMousePos = worldMousePos; // 이제 _viewportMousePos는 실제 월드 좌표를 저장
        }

        private void CreateTower(ETowerType type)
        {
            if (type == ETowerType.None) return;

            TowerRoot root = _pool.GetObject();
            _pendingTower = root;

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
            if (!_isUpdateActive) return;
            if (_pendingTower == null) return;

            // 현재 위치에서 목표 위치로 부드럽게 이동 (Viewport 변환 불필요)
            _pendingTower.transform.position = Vector3.Lerp(_pendingTower.transform.position, _viewportMousePos, SMOOTH_SPEED * Time.deltaTime);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
