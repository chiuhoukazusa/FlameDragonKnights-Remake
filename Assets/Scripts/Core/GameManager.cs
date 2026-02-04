using UnityEngine;

namespace FlameDragon.Core
{
    /// <summary>
    /// 游戏主管理器 - 控制游戏流程和全局状态
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        public GameState CurrentState = GameState.Menu;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Debug.Log("Game Manager Initialized");
        }

        public void StartBattle(int stageId)
        {
            CurrentState = GameState.Battle;
            Debug.Log($"Starting Stage {stageId}");
            // TODO: Load battle scene
        }

        public void EndBattle(bool victory)
        {
            Debug.Log(victory ? "Victory!" : "Defeat!");
            CurrentState = GameState.Menu;
        }
    }

    public enum GameState
    {
        Menu,
        Battle,
        Pause,
        GameOver
    }
}
