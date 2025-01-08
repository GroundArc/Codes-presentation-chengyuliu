using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // 退出游戏的方法
    public void QuitGame()
    {
        // 在编辑器模式下，停止播放
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在构建的游戏中退出应用程序
            Application.Quit();
#endif
    }
}
