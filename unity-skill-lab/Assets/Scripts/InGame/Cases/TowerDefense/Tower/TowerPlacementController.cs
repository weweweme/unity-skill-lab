using System;
using System.Collections.Generic;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Tower
{
    /// <summary>
    /// 타워 배치를 담당하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class TowerPlacementController : MonoBehaviourBase
    {
        /// <summary>
        /// 타워의 SpriteRenderer
        /// </summary>
        [SerializeField] private SpriteRenderer _spriteRenderer; 
        
        /// <summary>
        /// 현재 충돌 중인 오브젝트들의 목록
        /// </summary>
        private readonly HashSet<Collider2D> _collisions = new HashSet<Collider2D>();
        
        /// <summary>
        /// 타워가 배치되었는지 여부
        /// </summary>
        private bool _isPlaced;
        
        /// <summary>
        /// 타워가 배치될 수 있는지 여부
        /// </summary>
        private bool _canBePlaced;
        
        /// <summary>
        /// 타워 배치완료를 알리는 이벤트
        /// </summary>
        public event Action OnPlaced;

        private void Awake()
        {
            _isPlaced = false;
            _canBePlaced = false;
            _collisions.Clear();
            
            const float INITIAL_OPACITY = 0.5f;
            SpriteRendererHelper.SetOpacity(_spriteRenderer, INITIAL_OPACITY); // 기본적으로 반투명하게 설정
        }
        
        /// <summary>
        /// 타워를 현재 위치에 배치합니다.
        /// 배치 성공 여부를 반환합니다.
        /// </summary>
        /// <returns>배치가 성공하면 true를 반환합니다.</returns>
        public bool Place()
        {
            if (_canBePlaced)
            {
                const float PLACED_OPACITY = 1.0f;
                
                _isPlaced = true;
                SpriteRendererHelper.SetOpacity(_spriteRenderer, PLACED_OPACITY);  // 배치 성공 시 투명도를 정상화
                OnPlaced?.Invoke();
            }
            
            return _canBePlaced;
        }
        
        /// <summary>
        /// 충돌한 오브젝트를 기록합니다.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D col)
        {
            // 이미 설치된 상태라면 충돌 검사 생략
            if (_isPlaced) return;  
            if (!Layers.CompareLayer(col.gameObject.layer, Layers.InValidArea, Layers.Player)) return;
            
            _collisions.Add(col);
        }

        /// <summary>
        /// 충돌했던 오브젝트를 제거합니다.
        /// </summary>
        /// <param name="col">충돌이 끝난 Collider2D 객체</param>
        private void OnTriggerExit2D(Collider2D col)
        {
            // 이미 설치된 상태라면 충돌 검사 생략
            if (_isPlaced) return;  
            if (!Layers.CompareLayer(col.gameObject.layer, Layers.InValidArea, Layers.Player)) return;
            
            _collisions.Remove(col);
        }
        
        /// <summary>
        /// 설치 가능 여부를 확인합니다.
        /// </summary>
        private void Update()
        {
            // 이미 설치된 상태라면 설치 가능 여부 체크 생략
            if (_isPlaced) return;  

            // 충돌 목록이 비어 있을 때 스킬 사용이 가능해집니다.
            if (_collisions.Count == 0)
            {
                // 설치 가능할 경우                
                _canBePlaced = true;
                SpriteRendererHelper.SetColor(_spriteRenderer, Color.green);  
            }
            else
            {
                // 설치 불가능할 경우
                _canBePlaced = false;
                SpriteRendererHelper.SetColor(_spriteRenderer, Color.red);
            }
        }
    }
}
