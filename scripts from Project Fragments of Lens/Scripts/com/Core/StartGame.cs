using UnityEngine;
using UnityEngine.Events;

public class StartGame : MonoBehaviour
{
    public bool afterStart = false; // ��־�Ƿ������ʼ�׶�
    public UnityEvent onGameStart; // ����Ϸ��ʼ�׶δ������¼�
    public UnityEvent onGamePhaseEnd; // �ڿ�ʼ�׶ν���ʱ�������¼�

    private bool isGameStarted = false; // ��ֹ��δ��� onGameStart

    void Start()
    {
        StartGamePhase(); // �Զ���ʼ��Ϸ�׶�
    }

    // ��ʼ��Ϸ�׶�
    public void StartGamePhase()
    {
        if (!isGameStarted)
        {
            Debug.Log("Game is starting...");
            isGameStarted = true;
            afterStart = false; // ��Ϸ��ʼ�׶ν�����

            // ������Ϸ��ʼ���¼�
            onGameStart?.Invoke();
        }
    }

    // �ֶ�������ʼ�׶Σ�������ʽ��Ϸ
    public void EndStartPhase()
    {
        if (!afterStart)
        {
            Debug.Log("Start phase ended. Entering main game...");
            afterStart = true; // ������ʽ��Ϸ�׶�
            CameraFocusSystem.instance.SetGameStartPhaseComplete();


            // ������ʼ�׶ν������¼�
            onGamePhaseEnd?.Invoke();
        }
    }

    void Update()
    {
        // ���Դ��룺���¼��̿ո��������ʼ�׶Σ����������ã�
        //if (Input.GetKeyDown(KeyCode.Space) && !afterStart)
        //{
        //    EndStartPhase();
        //}
    }
}
