using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // �˳���Ϸ�ķ���
    public void QuitGame()
    {
        // �ڱ༭��ģʽ�£�ֹͣ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // �ڹ�������Ϸ���˳�Ӧ�ó���
            Application.Quit();
#endif
    }
}
