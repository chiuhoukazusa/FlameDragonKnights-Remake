using UnityEngine;

namespace FlameDragon.Data
{
    /// <summary>
    /// 单位数据配置 - 可在 Unity Inspector 中配置
    /// </summary>
    [CreateAssetMenu(fileName = "UnitData", menuName = "FlameDragon/Unit Data")]
    public class UnitData : ScriptableObject
    {
        [Header("Identity")]
        public string Name = "New Unit";
        public Systems.UnitClass Class = Systems.UnitClass.Infantry;
        public Sprite Portrait;
        public GameObject Prefab;

        [Header("Level & Stats")]
        public int Level = 1;
        public int MaxHP = 100;
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

        [Header("Growth Rates (%)")]
        [Tooltip("成长率：升级时属性增长概率")]
        public int HPGrowth = 80;
        public int AttackGrowth = 50;
        public int DefenseGrowth = 40;
        public int SpeedGrowth = 30;

        [Header("Description")]
        [TextArea(3, 5)]
        public string Description = "";
    }
}
