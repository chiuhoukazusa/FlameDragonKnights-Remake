using UnityEngine;
using System.Collections.Generic;

namespace FlameDragon.Systems
{
    /// <summary>
    /// 回合制管理器 - 控制玩家/敌人回合切换
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }

        [Header("Turn State")]
        public TurnPhase CurrentPhase = TurnPhase.PlayerTurn;
        public int TurnNumber = 1;

        [Header("Units")]
        public List<GameUnit> PlayerUnits = new List<GameUnit>();
        public List<GameUnit> EnemyUnits = new List<GameUnit>();

        public System.Action<TurnPhase> OnTurnChanged;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartPlayerTurn();
        }

        public void StartPlayerTurn()
        {
            CurrentPhase = TurnPhase.PlayerTurn;
            Debug.Log($"=== Turn {TurnNumber}: Player Phase ===");
            
            // 重置所有玩家单位的行动状态
            foreach (var unit in PlayerUnits)
            {
                unit.HasActed = false;
            }
            
            OnTurnChanged?.Invoke(CurrentPhase);
        }

        public void StartEnemyTurn()
        {
            CurrentPhase = TurnPhase.EnemyTurn;
            Debug.Log($"=== Turn {TurnNumber}: Enemy Phase ===");
            
            // 重置所有敌方单位的行动状态
            foreach (var unit in EnemyUnits)
            {
                unit.HasActed = false;
            }
            
            OnTurnChanged?.Invoke(CurrentPhase);
            
            // TODO: 启动敌人AI
            Invoke(nameof(EndEnemyTurn), 2f); // 临时：2秒后结束敌人回合
        }

        public void EndPlayerTurn()
        {
            Debug.Log("Player turn ended");
            StartEnemyTurn();
        }

        public void EndEnemyTurn()
        {
            Debug.Log("Enemy turn ended");
            TurnNumber++;
            StartPlayerTurn();
        }

        public bool CanUnitAct(GameUnit unit)
        {
            if (unit.Faction == UnitFaction.Player && CurrentPhase != TurnPhase.PlayerTurn)
                return false;
            if (unit.Faction == UnitFaction.Enemy && CurrentPhase != TurnPhase.EnemyTurn)
                return false;
            
            return !unit.HasActed;
        }

        public void RegisterUnit(GameUnit unit)
        {
            if (unit.Faction == UnitFaction.Player)
            {
                if (!PlayerUnits.Contains(unit))
                    PlayerUnits.Add(unit);
            }
            else if (unit.Faction == UnitFaction.Enemy)
            {
                if (!EnemyUnits.Contains(unit))
                    EnemyUnits.Add(unit);
            }
        }

        public void UnregisterUnit(GameUnit unit)
        {
            PlayerUnits.Remove(unit);
            EnemyUnits.Remove(unit);
        }
    }

    public enum TurnPhase
    {
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }
}
