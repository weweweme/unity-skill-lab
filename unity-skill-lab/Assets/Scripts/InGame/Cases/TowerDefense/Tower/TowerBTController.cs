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
        private readonly TowerAttackController _attackController;
        
        public TowerBTController(GameObject owner, TowerAttackController attackController)
        {
            _attackController = attackController;
            Init(owner);
        }
        
        /// <summary>
        /// 타워의 행동 트리를 생성하는 메서드입니다.
        /// </summary>
        /// <returns>구성된 BehaviorTree 인스턴스 (현재 null 반환)</returns>
        protected override BehaviorTree CreateTree(GameObject owner)
        {
            BehaviorTree bt = new BehaviorTreeBuilder(owner)
                .Selector("한 가지 행동 선택")
                
                    // 1. 현재 공격 상태인지 확인 후 타겟 탐색
                    .Condition("현재 공격 중인가?", _attackController.HasTarget)
                    .Do("적 탐색", _attackController.FindTarget)
                
                .End()
            .Build();
            
            return bt;
        }
    }
}
