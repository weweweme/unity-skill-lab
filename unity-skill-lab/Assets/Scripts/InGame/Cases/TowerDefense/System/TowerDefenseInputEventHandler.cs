using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Cases.TowerDefense.System
{
    /// <summary>
    /// Tower Defense 게임의 입력 이벤트를 처리하는 클래스입니다.
    /// </summary>
    public class TowerDefenseInputEventHandler : IDisposable, TowerDefenseInputActions.IMousePositionActions
    {
        private readonly TowerDefenseInputActions _inputActions;
        
        /// <summary>
        /// 마우스의 화면 좌표를 전달하는 이벤트입니다.
        /// </summary>
        public event Action<Vector2> OnMouseScreenPositionEvent;

        public TowerDefenseInputEventHandler()
        {
            _inputActions = new TowerDefenseInputActions();
            _inputActions.MousePosition.SetCallbacks(this);
            _inputActions.Enable();
        }

        /// <summary>
        /// 마우스 위치 입력을 감지하고 이벤트를 통해 전달하는 메서드입니다.
        /// </summary>
        /// <param name="context">입력 액션의 컨텍스트 정보</param>
        public void OnMousePosition(InputAction.CallbackContext context)
        {
            Vector2 mouseScreenPosition = context.ReadValue<Vector2>();
            OnMouseScreenPositionEvent?.Invoke(mouseScreenPosition);
        }
        
        public void Dispose()
        {
            _inputActions.Disable();
        }
    }
}
