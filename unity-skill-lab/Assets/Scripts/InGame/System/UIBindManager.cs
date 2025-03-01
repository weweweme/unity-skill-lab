using System.Collections.Generic;

namespace InGame.System
{
    /// <summary>
    /// 씬에서 사용되는 모델 - 프레젠터 - 뷰의 바인딩을 수행하는 클래스입니다.
    /// ViewController를 등록하고, 초기화합니다.
    /// </summary>
    public class UIBindManager
    {
        private readonly List<ViewController> _viewControllers = new List<ViewController>();
        
        /// <summary>
        /// TowerDefenseManager의 DataManager를 이용하여 등록된 ViewController들을 초기화합니다.
        /// </summary>
        public void Init(DataManager dataManager)
        {
            foreach (var elem in _viewControllers)
            {
                elem.Init(dataManager);
            }
        }
        
        /// <summary>
        /// ViewController를 리스트에 추가하여 관리합니다.
        /// </summary>
        public void AddViewController(ViewController viewController)
        {
            _viewControllers.Add(viewController);
        }
    }
}
