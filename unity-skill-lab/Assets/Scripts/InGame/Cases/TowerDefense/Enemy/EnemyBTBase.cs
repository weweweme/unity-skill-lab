using System.Collections.Generic;
using System.Threading;
using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using Cysharp.Threading.Tasks;
using InGame.Cases.TowerDefense.Managers;
using InGame.System;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Enemy
{
    // TODO: 추후 각 로직 모듈로 분리
    public sealed class EnemyBTBase : MonoBehaviourBase
    {
        private BehaviorTree _bt;
        private int currentIndex; // 시작 인덱스 (0)
        private int _maxIndex; // 마지막 노드 인덱스
        private List<Transform> pathNodes;

        private CancellationTokenSource _cts;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _moveDirection;
        
        [Header("이동 관련 설정")]
        [SerializeField] private float moveSpeed = 25f;
        [SerializeField] private float nodeArrivalThreshold = 0.5f;
        
        [Header("BT 틱 설정")]
        [SerializeField] private int btTickInterval = 50;

        private void Start()
        {
            _rigidbody2D = gameObject.GetComponentOrAssert<Rigidbody2D>();
            
            Init();
        }

        private void Init()
        {
            var tdManager = InGameManager.Ins as TowerDefenseManager;
            Debug.Assert(tdManager);
            
            _maxIndex = tdManager.PathManager.PathNodes.Count - 1;
            pathNodes = tdManager.PathManager.PathNodes;
            
            _bt = new BehaviorTreeBuilder(this.gameObject)
                .Selector("한 가지 행동 선택")

                    // 1. 목적지 도착 여부 확인 (최종 노드 도착 시 제거)
                    .Sequence("현재 목적지 확인")
                        .Condition("목적지 도착 여부 확인", HasReachedDestination)
                        .Do("적 제거", DestroyEnemy)
                    .End()

                    // 2. 이동 먼저 수행
                    .Sequence("이동 시퀀스")
                        .Do("다음 노드로 이동", MoveToNextNode) // 이동 먼저 수행
                        .Condition("다음 노드에 도착했는가?", HasReachedNextNode) // 이동 후 도착 체크
                        .Do("다음 노드 설정", SetNextNode) // 도착 후 갱신
                    .End()

                .End()
            .Build();

            StartBtTick();
        }

        private void FixedUpdate()
        {
            if (_moveDirection == Vector2.zero) return;
            
            Vector2 currentPos = _rigidbody2D.position;
            Vector2 targetPos = currentPos + _moveDirection * (moveSpeed * Time.fixedDeltaTime);

            _rigidbody2D.MovePosition(targetPos);
        }

        /// <summary>
        /// 현재 노드가 최종 목적지인지 확인
        /// </summary>
        private bool HasReachedDestination()
        {
            return currentIndex >= _maxIndex;
        }

        /// <summary>
        /// 현재 노드에서 다음 노드로 이동 (이동 방향만 설정)
        /// </summary>
        private TaskStatus MoveToNextNode()
        {
            if (currentIndex >= _maxIndex)
                return TaskStatus.Failure; // 더 이상 이동할 수 없으면 Failure 반환

            Transform targetNode = pathNodes[currentIndex + 1];

            // 이동 방향 설정 (이동 수행 X, FixedUpdate에서 적용)
            _moveDirection = ((Vector2)targetNode.position - (Vector2)transform.position).normalized;

            return TaskStatus.Success;
        }

        /// <summary>
        /// 다음 노드에 도착했는지 확인
        /// </summary>
        private bool HasReachedNextNode()
        {
            if (currentIndex >= _maxIndex) return false;

            float distance = Vector3.Distance(transform.position, pathNodes[currentIndex + 1].position);
            return distance < nodeArrivalThreshold; // 도착 거리 기준 설정
        }

        /// <summary>
        /// 다음 노드 설정
        /// </summary>
        private TaskStatus SetNextNode()
        {
            if (currentIndex >= _maxIndex) return TaskStatus.Failure; // 변경할 노드 없음 (정상적이라면 발생하지 않음)
            
            ++currentIndex;
            _moveDirection = Vector2.zero; // 노드 도착 후 정지
            
            return TaskStatus.Success; // 노드 변경 성공

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
        
        /// <summary>
        /// BT Tick을 시작합니다.
        /// </summary>
        private void StartBtTick()
        {
            CancelTokenHelper.GetToken(ref _cts);
            TickBtAsync(_cts.Token).Forget();
        }

        private void StopBtTick()
        {
            CancelTokenHelper.ClearToken(in _cts);
        }
        
        /// <summary>
        /// 행동트리 틱을 특정 시간 간격으로 반복합니다.
        /// </summary>
        /// <param name="token"></param>
        private async UniTask TickBtAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(btTickInterval, cancellationToken: token);

                bool isCanceled = token.IsCancellationRequested;
                if (isCanceled)
                {
                    break;
                }

                _bt.Tick();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            StopBtTick();
        }
    }
}
