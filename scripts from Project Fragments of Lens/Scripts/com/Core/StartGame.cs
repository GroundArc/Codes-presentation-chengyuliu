using UnityEngine;
using UnityEngine.Events;

public class StartGame : MonoBehaviour
{
    public bool afterStart = false; // 标志是否结束开始阶段
    public UnityEvent onGameStart; // 在游戏开始阶段触发的事件
    public UnityEvent onGamePhaseEnd; // 在开始阶段结束时触发的事件

    private bool isGameStarted = false; // 防止多次触发 onGameStart

    void Start()
    {
        StartGamePhase(); // 自动开始游戏阶段
    }

    // 开始游戏阶段
    public void StartGamePhase()
    {
        if (!isGameStarted)
        {
            Debug.Log("Game is starting...");
            isGameStarted = true;
            afterStart = false; // 游戏开始阶段进行中

            // 触发游戏开始的事件
            onGameStart?.Invoke();
        }
    }

    // 手动结束开始阶段，进入正式游戏
    public void EndStartPhase()
    {
        if (!afterStart)
        {
            Debug.Log("Start phase ended. Entering main game...");
            afterStart = true; // 进入正式游戏阶段
            CameraFocusSystem.instance.SetGameStartPhaseComplete();


            // 触发开始阶段结束的事件
            onGamePhaseEnd?.Invoke();
        }
    }

    void Update()
    {
        // 测试代码：按下键盘空格键结束开始阶段（仅供测试用）
        //if (Input.GetKeyDown(KeyCode.Space) && !afterStart)
        //{
        //    EndStartPhase();
        //}
    }
}
