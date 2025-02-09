using System;
using InGame.Cases.TowerDefense.System;
using InGame.Cases.TowerDefense.System.Managers;
using InGame.Cases.TowerDefense.Tower.Pool;
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
        private const float SMOOTH_SPEED = 150f;
        
        private readonly TowerDefenseDataManager _dataManager;
        private readonly TowerBasePool _pool;
        private readonly TowerDefenseInputEventHandler _eventHandler;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly Camera mainCam;
        
        /// <summary>
        /// 마우스의 Z 좌표 변환을 위한 깊이값
        /// </summary>
        private readonly float zDepth;
        
        /// <summary>
        /// 현재 타워 배치가 활성화되었는지 여부
        /// </summary>
        private bool _isUpdateActive;
        
        /// <summary>
        /// 타워가 이동할 월드 포지션
        /// </summary>
        private Vector2 _targetWorldPos;

        /// <summary>
        /// 배치 중인 타워 객체
        /// </summary>
        private TowerRoot _pendingTower;

        public TowerCreateHandler(TowerDefenseManager tdManager)
        {
            _dataManager = tdManager.DataManager;
            _pool = tdManager.PoolManager.TowerBasePool;
            _eventHandler = tdManager.InputHandler;
            
            mainCam = tdManager.CameraManager.MainCam;
            zDepth = -(mainCam.transform.position.z);

            _eventHandler.OnMouseLeftClickEvent -= TowerPlace;
            _eventHandler.OnMouseLeftClickEvent += TowerPlace;
            _eventHandler.OnMouseScreenPositionEvent -= SetCursorPosition;
            _eventHandler.OnMouseScreenPositionEvent += SetCursorPosition;

            _dataManager.Tower.TowerToBuild
                .Subscribe(CreateTower)
                .AddTo(_disposable);
            
            Observable.EveryUpdate()
                .Where(_ => _isUpdateActive)
                .Subscribe(_ => UpdateTowerPlacement())
                .AddTo(_disposable);
        }

        /// <summary>
        /// 마우스의 스크린 좌표를 월드 좌표로 변환하여 저장합니다.
        /// </summary>
        /// <param name="screenMousePos">스크린 좌표계에서의 마우스 위치</param>
        private void SetCursorPosition(Vector2 screenMousePos)
        {
            Vector3 worldMousePos = mainCam.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, zDepth));
            worldMousePos.z = Z_COORD;

            _targetWorldPos = worldMousePos;
        }

        private void CreateTower(ETowerType type)
        {
            if (type == ETowerType.None) return;

            TowerRoot root = _pool.GetObject();
            _pendingTower = root;
            _pendingTower.transform.position = _targetWorldPos;
            
            _isUpdateActive = true;
        }

        private void ClearCreateProcess()
        {
            _isUpdateActive = false;
            _pendingTower = null;
        }

        private void UpdateTowerPlacement()
        {
            if (!_isUpdateActive) return;
            if (_pendingTower == null) return;

            // 현재 위치에서 목표 위치로 부드럽게 이동 (Viewport 변환 불필요)
            _pendingTower.transform.position = Vector3.Lerp(_pendingTower.transform.position, _targetWorldPos, SMOOTH_SPEED * Time.deltaTime);
        }
        
        private void TowerPlace()
        {
            if (_pendingTower == null) return;
            
            bool isSuccess = _pendingTower.PlacementController.Place();
            if (!isSuccess) return;
            
            ClearCreateProcess();
        }

        public void Dispose()
        {
            _eventHandler.OnMouseScreenPositionEvent -= SetCursorPosition;
            _disposable.Dispose();
        }
    }
}
