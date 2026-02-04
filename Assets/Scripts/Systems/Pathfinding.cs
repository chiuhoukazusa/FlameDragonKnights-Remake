using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace FlameDragon.Systems
{
    /// <summary>
    /// A* 寻路算法 - 计算移动路径
    /// </summary>
    public class Pathfinding : MonoBehaviour
    {
        public static Pathfinding Instance { get; private set; }

        private HexGrid grid;

        private void Awake()
        {
            Instance = this;
            grid = GetComponent<HexGrid>();
        }

        /// <summary>
        /// 计算可移动范围内的所有格子
        /// </summary>
        public List<Vector2Int> GetMovableArea(Vector2Int start, int moveRange, UnitFaction faction = UnitFaction.Player)
        {
            List<Vector2Int> movableArea = new List<Vector2Int>();
            Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();
            Queue<Vector2Int> frontier = new Queue<Vector2Int>();

            frontier.Enqueue(start);
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                Vector2Int current = frontier.Dequeue();
                HexTile currentTile = grid.GetTile(current);

                foreach (Vector2Int next in grid.GetNeighbors(current))
                {
                    HexTile nextTile = grid.GetTile(next);
                    if (nextTile == null) continue;

                    // 检查格子是否被其他单位占据
                    if (nextTile.OccupyingUnit != null && next != start)
                    {
                        // 如果是敌方单位，不能通过
                        if (nextTile.OccupyingUnit.Faction != faction)
                            continue;
                    }

                    int newCost = costSoFar[current] + nextTile.GetMoveCost();

                    if (newCost <= moveRange && (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]))
                    {
                        costSoFar[next] = newCost;
                        movableArea.Add(next);
                        frontier.Enqueue(next);
                    }
                }
            }

            return movableArea;
        }

        /// <summary>
        /// A* 寻路：找到从起点到终点的最短路径
        /// </summary>
        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        {
            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();
            
            PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
            frontier.Enqueue(start, 0);
            
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                Vector2Int current = frontier.Dequeue();

                if (current == goal)
                    break;

                foreach (Vector2Int next in grid.GetNeighbors(current))
                {
                    HexTile nextTile = grid.GetTile(next);
                    if (nextTile == null) continue;

                    int newCost = costSoFar[current] + nextTile.GetMoveCost();

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        int priority = newCost + grid.GetDistance(next, goal);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            // 重建路径
            return ReconstructPath(cameFrom, start, goal);
        }

        private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int goal)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int current = goal;

            if (!cameFrom.ContainsKey(goal))
                return path; // 无法到达

            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// 计算攻击范围内的所有格子
        /// </summary>
        public List<Vector2Int> GetAttackRange(Vector2Int position, int attackRange)
        {
            List<Vector2Int> attackArea = new List<Vector2Int>();

            for (int q = -attackRange; q <= attackRange; q++)
            {
                for (int r = -attackRange; r <= attackRange; r++)
                {
                    Vector2Int coord = position + new Vector2Int(q, r);
                    if (grid.GetDistance(position, coord) <= attackRange && grid.GetTile(coord) != null)
                    {
                        attackArea.Add(coord);
                    }
                }
            }

            return attackArea;
        }
    }

    /// <summary>
    /// 简单优先队列实现（用于 A*）
    /// </summary>
    public class PriorityQueue<T>
    {
        private List<(T item, int priority)> elements = new List<(T, int)>();

        public int Count => elements.Count;

        public void Enqueue(T item, int priority)
        {
            elements.Add((item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;
            for (int i = 1; i < elements.Count; i++)
            {
                if (elements[i].priority < elements[bestIndex].priority)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].item;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }
}
