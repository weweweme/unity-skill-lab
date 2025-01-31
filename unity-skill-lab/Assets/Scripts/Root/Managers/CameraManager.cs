using Root.Util;
using UnityEngine;

namespace Root.Managers
{
    /// <summary>
    /// 게임의 주요 카메라를 관리하는 매니저 클래스입니다.
    /// </summary>
    public sealed class CameraManager : MonoBehaviourBase
    {
        [SerializeField] private Camera mainCam;
        public Camera MainCam => mainCam;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(CameraManager), mainCam);
        }
    }
}
