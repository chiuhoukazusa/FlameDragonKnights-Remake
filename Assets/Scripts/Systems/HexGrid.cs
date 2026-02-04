using UnityEngine;
using System.Collections.Generic;

namespace FlameDragon.Systems
{
    /// <summary>
    /// 六边形网格系统 - 战棋地图的核心
    /// 使用 Axial Coordinates (q, r) 坐标系统
    /// </summary>
    public class HexGrid : MonoBehaviour
    {
        [Header("Grid Settings")]
        public int Width = 10;
        public int Height = 10;
        public float HexSize = 1f;
        
        [Header("Prefabs")]
        public GameObject HexTilePrefab;

        private Dictionary<Vector2Int, HexTile> tiles = new Dictionary<Vector2Int, HexTile>();

        private void Start()
        {
            GenerateGrid();
        }

        /// <summary>
        /// 生成六边形网格
        /// </summary>
        public void GenerateGrid()
        {
            for (int q = 0; q < Width; q++)
            {
                for (int r = 0; r < Height; r++)
                {
                    Vector2Int coord = new Vector2Int(q, r);
                    Vector3 worldPos = HexToWorldPosition(coord);
                    
                    GameObject hexObj = Instantiate(HexTilePrefab, worldPos, Quaternion.identity, transform);
                    hexObj.name = $"Hex_{q}_{r}";
                    
                    HexTile tile = hexObj.GetComponent<HexTile>();
                    if (tile == null)
                        tile = hexObj.AddComponent<HexTile>();
                    
                    tile.Initialize(coord, TerrainType.Grass);
                    tiles[coord] = tile;
                }
            }
            
            Debug.Log($"Generated {tiles.Count} hex tiles");
        }

        /// <summary>
        /// 六边形坐标转世界坐标
        /// Flat-top hexagon layout
        /// </summary>
        public Vector3 HexToWorldPosition(Vector2Int hexCoord)
        {
            float x = HexSize * (3f / 2f * hexCoord.x);
            float z = HexSize * (Mathf.Sqrt(3f) / 2f * hexCoord.x + Mathf.Sqrt(3f) * hexCoord.y);
            return new Vector3(x, 0, z);
        }

        /// <summary>
        /// 获取六边形邻居（6个方向）
        /// </summary>
        public List<Vector2Int> GetNeighbors(Vector2Int coord)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(1, 0),   // East
                new Vector2Int(1, -1),  // Southeast
                new Vector2Int(0, -1),  // Southwest
                new Vector2Int(-1, 0),  // West
                new Vector2Int(-1, 1),  // Northwest
                new Vector2Int(0, 1)    // Northeast
            };

            foreach (var dir in directions)
            {
                Vector2Int neighbor = coord + dir;
                if (tiles.ContainsKey(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        /// <summary>
        /// 获取指定格子
        /// </summary>
        public HexTile GetTile(Vector2Int coord)
        {
            return tiles.ContainsKey(coord) ? tiles[coord] : null;
        }

        /// <summary>
        /// 计算两个格子之间的距离（六边形距离）
        /// </summary>
        public int GetDistance(Vector2Int a, Vector2Int b)
        {
            return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.x + a.y - b.x - b.y) + Mathf.Abs(a.y - b.y)) / 2;
        }
    }

    /// <summary>
    /// 单个六边形格子
    /// </summary>
    public class HexTile : MonoBehaviour
    {
        public Vector2Int Coordinate { get; private set; }
        public TerrainType Terrain { get; private set; }
        public GameUnit OccupyingUnit { get; set; }
        
        private Renderer tileRenderer;

        public void Initialize(Vector2Int coord, TerrainType terrain)
        {
            Coordinate = coord;
            Terrain = terrain;
            tileRenderer = GetComponent<Renderer>();
            UpdateVisual();
        }

        public void UpdateVisual()
        {
            if (tileRenderer != null)
            {
                // 根据地形类型设置颜色（占位符）
                Color color = Terrain switch
                {
                    TerrainType.Grass => new Color(0.3f, 0.8f, 0.3f),
                    TerrainType.Forest => new Color(0.2f, 0.6f, 0.2f),
                    TerrainType.Mountain => new Color(0.6f, 0.6f, 0.6f),
                    TerrainType.Castle => new Color(0.8f, 0.7f, 0.5f),
                    _ => Color.white
                };
                tileRenderer.material.color = color;
            }
        }

        public void Highlight(bool enabled)
        {
            if (tileRenderer != null)
            {
                tileRenderer.material.color = enabled ? Color.yellow : Color.white;
                UpdateVisual();
            }
        }

        /// <summary>
        /// 获取地形移动消耗
        /// </summary>
        public int GetMoveCost()
        {
            return Terrain switch
            {
                TerrainType.Grass => 1,
                TerrainType.Forest => 2,
                TerrainType.Mountain => 3,
                TerrainType.Castle => 1,
                _ => 1
            };
        }

        /// <summary>
        /// 获取地形防御加成
        /// </summary>
        public int GetDefenseBonus()
        {
            return Terrain switch
            {
                TerrainType.Forest => 10,
                TerrainType.Mountain => 20,
                TerrainType.Castle => 30,
                _ => 0
            };
        }
    }

    public enum TerrainType
    {
        Grass,      // 草地
        Forest,     // 森林
        Mountain,   // 山地
        Castle,     // 城堡
        Water       // 水域（不可通行）
    }
}
