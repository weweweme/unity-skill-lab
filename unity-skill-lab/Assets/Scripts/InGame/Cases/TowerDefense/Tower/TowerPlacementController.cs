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
        /// 현재 알파값
        /// </summary>
        private float _currentAlpha = 1.0f;
        
        /// <summary>
        /// 현재 충돌 중인 오브젝트들의 목록
        /// </summary>
        private readonly HashSet<Collider2D> _collisions = new HashSet<Collider2D>();
        
        /// <summary>
        /// 타워가 배치되었는지 여부
        /// </summary>
        private bool _isPlaced;
        
        private bool _canBePlaced;
        
        /// <summary>
        /// 타워를 현재 위치에 배치합니다.
        /// 배치 성공 여부를 반환합니다.
        /// </summary>
        /// <returns>배치가 성공하면 true를 반환합니다.</returns>
        public bool Place()
        {
            if (_canBePlaced)
            {
                _isPlaced = true;
                SetOpacity(1.0f);  // 배치 성공 시 투명도를 1.0으로 설정
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

            // TODO: 매직 넘버 Layers 스태틱 헬퍼 사용하도록 변경
            bool isInValidArea = col.gameObject.layer == LayerMask.GetMask("InValidArea");
            if (!isInValidArea) return;
            
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

            // TODO: 매직 넘버 Layers 스태틱 헬퍼 사용하도록 변경
            bool isWall = col.gameObject.layer == LayerMask.GetMask("InValidArea");
            if (!isWall) return;
            
            _collisions.Remove(col);
        }
        
        private void Update()
        {
            if (_isPlaced) return;  // 이미 설치된 상태라면 설치 가능 여부 체크 생략

            // 충돌 목록이 비어 있을 때 스킬 사용이 가능해집니다.
            if (_collisions.Count == 0)
            {
                _canBePlaced = true;
                SetColor(Color.green);  // 설치 가능 시 Color를 Green으로 셋팅
            }
            else
            {
                _canBePlaced = false;
                SetColor(Color.red);  // 설치 불가능 시 Color를 Red로 셋팅
            }
        }
        
        /// <summary>
        /// 타워의 투명도를 설정합니다.
        /// </summary>
        /// <param name="alpha">설정할 알파(투명도) 값</param>
        private void SetOpacity(float alpha)
        {
            // 현재 알파값을 저장하고, 알파값만 변경
            _currentAlpha = alpha;
            
            Color color = _spriteRenderer.color;
            color.a = alpha;
            _spriteRenderer.color = color;
        }

        /// <summary>
        /// 타워의 색상을 변경합니다. 기존의 알파(투명도) 값은 유지됩니다.
        /// </summary>
        /// <param name="color">설정할 색상 값</param>
        private void SetColor(Color color)
        {
            // 기존의 알파값을 유지하면서 색상을 변경
            color.a = _currentAlpha;
            _spriteRenderer.color = color;
        }
    }
}
