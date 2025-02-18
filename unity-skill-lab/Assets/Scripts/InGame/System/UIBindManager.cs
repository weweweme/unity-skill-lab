using System;
using System.Collections.Generic;
using InGame.Cases.TowerDefense.System.Managers;

namespace InGame.System
{
    /// <summary>
    /// 씬에서 사용되는 모델 - 프레젠터 - 뷰의 바인딩을 수행하는 클래스입니다.
    /// ViewController를 등록하고, 초기화하며, 필요 시 해제하는 역할을 합니다.
    /// </summary>
    public class UIBindManager : IDisposable
    {
        private readonly List<ViewController> _viewControllers = new List<ViewController>();
        
        /// <summary>
        /// TowerDefenseManager의 DataManager를 이용하여 등록된 ViewController들을 초기화합니다.
        /// </summary>
        public void Init(TowerDefenseManager towerDefenseManager)
        {
            TowerDefenseDataManager dataManager = towerDefenseManager.DataManager;

            foreach (var elem in _viewControllers)
            {
                elem.InitRx(dataManager);
            }
        }
        
        /// <summary>
        /// ViewController를 리스트에 추가하여 관리합니다.
        /// </summary>
        public void AddViewController(ViewController viewController)
        {
            _viewControllers.Add(viewController);
        }
        
        /// <summary>
        /// 모든 ViewController의 Dispose를 호출하여 등록된 ViewController들을 정리합니다.
        /// </summary>
        public void Dispose()
        {
            foreach (var elem in _viewControllers)
            {
                elem.Dispose();
            }
        }
    }
}
