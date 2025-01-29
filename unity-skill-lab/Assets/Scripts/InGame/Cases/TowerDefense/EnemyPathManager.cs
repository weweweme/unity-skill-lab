using System.Collections.Generic;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense
{
    public sealed class EnemyPathManager : MonoBehaviourBase
    {
        [SerializeField] private List<Transform> pathNodes;
        public List<Transform> PathNodes => pathNodes;
    }
}
