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

        /// <summary>
        /// 마우스 왼쪽 클릭 입력이 감지될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnMouseLeftClickEvent;

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

        /// <summary>
        /// 마우스 왼쪽 클릭이 수행되었을 때 호출됩니다.
        /// 이벤트를 발생시켜 클릭 입력을 처리할 수 있도록 합니다.
        /// </summary>
        /// <param name="context">입력 액션의 컨텍스트 정보</param>
        public void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            OnMouseLeftClickEvent?.Invoke();
        }

        public void Dispose()
        {
            _inputActions.Disable();
        }
    }
}
