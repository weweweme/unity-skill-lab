using System;
using System.Collections.Generic;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// Tower Defense 씬에서 UI를 관리하는 클래스입니다.
    /// 씬에서 사용되는 모델 - 프레젠터 - 뷰의 바인딩을 수행합니다.
    /// </summary>
    public class TowerDefenseUIManager : IDisposable
    {
        private readonly List<ViewController> _viewControllers = new List<ViewController>();
        public IReadOnlyList<ViewController> ViewControllers => _viewControllers;
        
        public void Init(TowerDefenseManager towerDefenseManager)
        {
            TowerDefenseDataManager dataManager = towerDefenseManager.DataManager;

            foreach (var elem in _viewControllers)
            {
                elem.Init(dataManager);
            }
        }
        
        public void Dispose()
        {
            foreach (var elem in _viewControllers)
            {
                elem.Dispose();
            }
        }
    }
}
