using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using InGame.Cases.TowerDefense.Managers;
using InGame.System;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Enemy
{
    public sealed class EnemyBTBase : MonoBehaviourBase
    {
        private int currentIndex; // 시작 인덱스 (0)
        private int _maxIndex; // 마지막 노드 인덱스
        private List<Transform> pathNodes;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            var tdManager = InGameManager.Ins as TowerDefenseManager;
            Debug.Assert(tdManager);
            
            _maxIndex = tdManager.PathManager.PathNodes.Count - 1;
            pathNodes = tdManager.PathManager.PathNodes;
            
            var bt = new BehaviorTreeBuilder(this.gameObject)
                .Selector("한 가지 행동 선택")

                    // 1. 현재 목적지 노드 체크
                    .Sequence("현재 목적지 확인")
                        .Condition("목적지 도착 여부 확인", HasReachedDestination)
                        .Do("적 제거", DestroyEnemy)
                    .End()

                    // 2. 아직 다음 노드에 도착하지 않았다면 이동
                    .Sequence("이동 시퀀스")
                        .Condition("다음 노드에 도착하지 않음", () => !HasReachedNextNode())
                        .Do("다음 노드로 이동", MoveToNextNode)
                    .End()

                    // 3. 다음 노드에 도착했다면 다음 노드 설정
                    .Sequence("다음 노드 도착 처리")
                        .Condition("다음 노드에 도착했는가?", HasReachedNextNode)
                        .Do("다음 노드 설정", SetNextNode)
                    .End()

                .End()
            .Build();
        }

        /// <summary>
        /// 현재 노드가 최종 목적지인지 확인
        /// </summary>
        private bool HasReachedDestination()
        {
            return currentIndex >= _maxIndex;
        }

        /// <summary>
        /// 현재 노드에서 다음 노드로 이동
        /// </summary>
        private TaskStatus MoveToNextNode()
        {
            Debug.Log($"[Enemy] 현재 노드 {currentIndex} -> {currentIndex + 1} 이동 중...");

            if (currentIndex >= _maxIndex)
                return TaskStatus.Failure; // 더 이상 이동할 수 없으면 Failure 반환

            Transform targetNode = pathNodes[currentIndex + 1];

            // 이동 방향 설정 (이동 수행 X, FixedUpdate에서 적용)
            Vector2 direction = ((Vector2)targetNode.position - (Vector2)transform.position).normalized;
            // _moveController.SetMovement(direction, moveSpeed);

            return TaskStatus.Continue; // 이동 중
        }

        /// <summary>
        /// 다음 노드에 도착했는지 확인
        /// </summary>
        private bool HasReachedNextNode()
        {
            if (currentIndex >= _maxIndex) return false;

            float distance = Vector3.Distance(transform.position, pathNodes[currentIndex + 1].position);
            return distance < 0.1f; // 도착 거리 기준 설정
        }

        /// <summary>
        /// 다음 노드 설정
        /// </summary>
        private TaskStatus SetNextNode()
        {
            if (currentIndex < _maxIndex)
            {
                currentIndex++;
                Debug.Log($"[Enemy] 새로운 목표: 노드 {currentIndex}");
                return TaskStatus.Success; // 노드 변경 성공
            }

            return TaskStatus.Failure; // 변경할 노드 없음 (정상적이라면 발생하지 않음)
        }

        /// <summary>
        /// 적을 제거
        /// </summary>
        private TaskStatus DestroyEnemy()
        {
            Debug.Log($"[Enemy] 목적지 도착! Destroy 실행");
            Destroy(gameObject);
            return TaskStatus.Success;
        }
    }
}
