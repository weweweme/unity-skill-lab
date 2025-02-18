using InGame.Cases.TowerDefense.System.Managers;
using InGame.System;
using Root.Util;

namespace InGame.Cases.TowerDefense.System
{
    /// <summary>
    /// TowerDefense 게임 모드에서 사용되는 ViewController의 기본 클래스입니다.
    /// ViewController를 상속받으며, TowerDefenseManager를 통해 UIManager에 ViewController를 등록하는 역할을 수행합니다.
    /// </summary>
    public abstract class TowerDefenseViewController : ViewController
    {
        protected override void RegisterToUIManager()
        {
            TowerDefenseManager rootManager = InGameManager.Ins as TowerDefenseManager;
            AssertHelper.NotNull(typeof(ViewController), rootManager);
            rootManager!.UIManager.AddViewController(this);
        }
    }
}
