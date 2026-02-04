# 炎龙骑士团：黄金城之谜 - Unity 复刻项目

> Flame Dragon Knights: Legend of Golden Castle - Unity Remake

经典战棋 SRPG 游戏的现代化重制版本。本项目旨在使用 Unity 引擎完整复刻 1995 年汉堂国际的经典作品。

## 🎮 项目目标

**第一阶段（当前）**：复刻第一关
- 完整的战棋核心玩法
- 回合制战斗系统
- 六边形地图系统
- 角色移动与攻击
- 基础 AI 系统

**后续阶段**：
- 完整关卡系统
- 剧情对话系统
- 装备与道具系统
- 技能与魔法系统
- 存档系统

## 🛠️ 技术栈

- **引擎**: Unity 2022.3 LTS (或更新)
- **语言**: C# 9.0+
- **架构**: 模块化 + ScriptableObject 数据驱动
- **版本控制**: Git + GitHub

## 📂 项目结构

```
Assets/
├── Scripts/
│   ├── Core/          # 核心系统（GameManager 等）
│   ├── Systems/       # 游戏系统（地图、战斗、AI）
│   │   ├── HexGrid.cs         # 六边形网格系统
│   │   ├── GameUnit.cs        # 游戏单位基类
│   │   ├── TurnManager.cs     # 回合管理器
│   │   └── Pathfinding.cs     # A* 寻路算法
│   ├── Data/          # 数据配置（ScriptableObject）
│   │   └── UnitData.cs        # 单位数据配置
│   ├── UI/            # 用户界面
│   └── Utilities/     # 工具类
├── Prefabs/           # 预制件
├── Scenes/            # 场景文件
└── Resources/         # 资源文件
    ├── Units/         # 单位数据
    ├── Stages/        # 关卡数据
    └── Art/           # 美术资源
```

## 🎯 核心系统设计

### 1. 六边形网格系统 (HexGrid)
- **坐标系统**: Axial Coordinates (q, r)
- **地形类型**: 草地、森林、山地、城堡、水域
- **地形效果**: 
  - 移动消耗（草地=1, 森林=2, 山地=3）
  - 防御加成（森林+10%, 山地+20%, 城堡+30%）

### 2. 战斗系统
- **回合制**: 玩家回合 → 敌人回合 → 循环
- **移动**: A* 寻路算法，考虑地形和移动力
- **攻击**: 物理/魔法攻击，伤害公式考虑地形防御
- **兵种克制**: 骑兵 > 弓兵 > 步兵 > 骑兵

### 3. 单位系统
- **基础属性**: HP, MP, 攻击, 防御, 魔攻, 魔防, 速度, 幸运
- **职业系统**: 步兵、骑兵、弓兵、魔法师、牧师、骑士、龙骑士
- **成长系统**: 等级、经验值、属性成长率

### 4. 数据驱动
- 使用 ScriptableObject 配置单位、装备、关卡数据
- JSON 格式存储关卡布局和事件
- 易于扩展和修改

## 🚀 快速开始

### 环境要求
- Unity 2022.3 LTS 或更高版本
- Git
- 基础 C# 编程知识

### 克隆项目
```bash
git clone https://github.com/chiuhoukazusa/FlameDragonKnights-Remake.git
cd FlameDragonKnights-Remake
```

### 在 Unity 中打开
1. 打开 Unity Hub
2. 点击 "Add" → 选择项目文件夹
3. 打开项目
4. 等待导入完成

### 运行测试场景
1. 打开 `Assets/Scenes/BattleTest.scene`
2. 点击 Play 按钮
3. 使用鼠标点击单位和地图进行测试

## 📊 开发进度

### ✅ 已完成
- [x] 项目初始化
- [x] 六边形网格系统
- [x] 单位基础类
- [x] 回合管理器
- [x] A* 寻路算法
- [x] ScriptableObject 数据结构

### 🔄 进行中
- [ ] 单位移动控制
- [ ] 攻击系统实现
- [ ] 战斗动画
- [ ] 敌人 AI

### 📋 待开发
- [ ] 第一关地图数据
- [ ] UI 系统
- [ ] 音效与 BGM
- [ ] 技能系统
- [ ] 装备系统

## 🎨 美术资源

**当前状态**: 使用简单几何体和纯色占位符

**未来计划**:
- 2D 像素风格单位
- 手绘风格地图
- 动画效果
- UI 美术

**资源来源**:
- Unity Asset Store
- itch.io / OpenGameArt.org
- 自制/委托

## 📖 原版游戏资料

### 游戏信息
- **发行年代**: 1995年（DOS版）
- **制作公司**: 汉堂国际
- **系列作品**: 炎龙骑士团 I/II/外传
- **游戏类型**: 战棋 SRPG

### 第一关信息
正在收集中...（请参考 `Docs/OriginalGameData.md`）

## 🤝 贡献指南

欢迎任何形式的贡献！

### 如何贡献
1. Fork 本仓库
2. 创建你的特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交你的改动 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开一个 Pull Request

### 代码规范
- 使用 C# 命名约定
- 添加必要的注释
- 遵循现有代码风格

## 📄 许可证

本项目仅供学习和研究使用。

**重要声明**:
- 本项目不包含任何原版游戏的资源
- 所有美术、音乐、文本均为重新创作或使用开源资源
- 炎龙骑士团及相关商标属于原版权方所有

## 📞 联系方式

- **GitHub Issues**: [提交问题](https://github.com/chiuhoukazusa/FlameDragonKnights-Remake/issues)
- **讨论区**: [参与讨论](https://github.com/chiuhoukazusa/FlameDragonKnights-Remake/discussions)

## 🎉 致谢

- 感谢汉堂国际创造了如此经典的游戏
- 感谢所有贡献者和支持者
- 感谢开源社区提供的工具和资源

---

**⚔️ 让我们一起重现经典！**
