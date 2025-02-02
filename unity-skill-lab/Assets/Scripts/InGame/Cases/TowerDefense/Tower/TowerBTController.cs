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
                
                    // 1. 현재 공격 중인지 확인
                    .Sequence("공격 중일 때")
                        .Condition("현재 공격 중인가?", _attackController.HasTarget)
                        .Selector("타겟 유지 or 해제")
                            .Condition("타겟이 여전히 사거리 안에 있는가?", _attackController.IsTargetInRange)
                            .Do("타겟 해제", _attackController.ClearTarget)
                        .End()
                    .End()
                
                    // 2. 타겟이 없으면 새 적 탐색
                    .Do("적 탐색", _attackController.FindTarget)
                
                .End()
            .Build();
            
            return bt;
        }
    }
}
