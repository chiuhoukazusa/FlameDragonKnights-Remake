using UnityEngine;

namespace FlameDragon.Systems
{
    /// <summary>
    /// 游戏单位基类 - 角色/敌人的核心组件
    /// </summary>
    public class GameUnit : MonoBehaviour
    {
        [Header("Unit Identity")]
        public string UnitName = "Unknown";
        public UnitClass Class = UnitClass.Infantry;
        public UnitFaction Faction = UnitFaction.Player;

        [Header("Stats")]
        public int Level = 1;
        public int CurrentHP = 100;
        public int MaxHP = 100;
        public int CurrentMP = 20;
        public int MaxMP = 20;
        
        [Header("Combat Stats")]
        public int Attack = 20;
        public int Defense = 15;
        public int MagicAttack = 10;
        public int MagicDefense = 10;
        public int Speed = 10;
        public int Luck = 5;
        
        [Header("Movement")]
        public int MoveRange = 3;
        public int AttackRange = 1;

        [Header("Position")]
        public Vector2Int GridPosition;
        
        private HexTile currentTile;
        public bool HasActed { get; set; }

        public void Initialize(UnitData data)
        {
            UnitName = data.Name;
            Class = data.Class;
            Level = data.Level;
            
            MaxHP = data.MaxHP;
            CurrentHP = MaxHP;
            MaxMP = data.MaxMP;
            CurrentMP = MaxMP;
            
            Attack = data.Attack;
            Defense = data.Defense;
            MagicAttack = data.MagicAttack;
            MagicDefense = data.MagicDefense;
            Speed = data.Speed;
            Luck = data.Luck;
            
            MoveRange = data.MoveRange;
            AttackRange = data.AttackRange;
        }

        public void PlaceOnTile(HexTile tile)
        {
            if (currentTile != null)
                currentTile.OccupyingUnit = null;
            
            currentTile = tile;
            GridPosition = tile.Coordinate;
            tile.OccupyingUnit = this;
            
            transform.position = tile.transform.position + Vector3.up * 0.5f;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP = Mathf.Max(0, CurrentHP - damage);
            Debug.Log($"{UnitName} takes {damage} damage. HP: {CurrentHP}/{MaxHP}");
            
            if (CurrentHP <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{UnitName} has been defeated!");
            if (currentTile != null)
                currentTile.OccupyingUnit = null;
            
            // TODO: Play death animation
            gameObject.SetActive(false);
        }

        public int CalculateDamage(GameUnit target, bool isMagic = false)
        {
            int attackStat = isMagic ? MagicAttack : Attack;
            int defenseStat = isMagic ? target.MagicDefense : target.Defense;
            
            // 简单伤害公式：攻击 - 防御/2
            int baseDamage = Mathf.Max(1, attackStat - defenseStat / 2);
            
            // 地形防御加成
            if (target.currentTile != null)
            {
                int terrainBonus = target.currentTile.GetDefenseBonus();
                baseDamage = Mathf.Max(1, baseDamage - terrainBonus / 10);
            }
            
            return baseDamage;
        }
    }

    public enum UnitClass
    {
        Infantry,       // 步兵
        Cavalry,        // 骑兵
        Archer,         // 弓箭手
        Mage,           // 魔法师
        Priest,         // 牧师
        Knight,         // 骑士
        Dragon          // 龙骑士
    }

    public enum UnitFaction
    {
        Player,         // 玩家
        Enemy,          // 敌人
        Ally,           // 友军
        Neutral         // 中立
    }
}
