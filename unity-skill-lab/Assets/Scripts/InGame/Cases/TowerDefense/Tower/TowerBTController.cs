using CleverCrow.Fluid.BTs.Trees;
using Root;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Tower
{
    /// <summary>
    /// 타워의 AI 행동 트리를 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class TowerBTController : BehaviourTreeBase
    {
        public TowerBTController(GameObject owner)
        {
            Init(owner);
        }
        
        /// <summary>
        /// 타워의 행동 트리를 생성하는 메서드입니다.
        /// </summary>
        /// <returns>구성된 BehaviorTree 인스턴스 (현재 null 반환)</returns>
        protected override BehaviorTree CreateTree(GameObject owner)
        {
            return null;
        }
    }
}
