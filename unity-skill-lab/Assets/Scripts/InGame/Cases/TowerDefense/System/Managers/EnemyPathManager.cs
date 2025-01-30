using System.Collections.Generic;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.System.Managers
{
    /// <summary>
    /// 관리하는 적의 이동 경로를 저장하고 제공하는 클래스.
    /// </summary>
    /// <remarks>
    /// 이 클래스는 적이 이동해야 할 경로(패스포인트)들을 저장하며, 
    /// 참조를 제공한다.
    /// </remarks>
    public sealed class EnemyPathManager : MonoBehaviourBase
    {
        [SerializeField] private List<Transform> pathNodes;
        public List<Transform> PathNodes => pathNodes;
    }
}
