using UnityEngine;

public class RainFilterController : MonoBehaviour
{
    public Camera targetCamera; // ָ��Ŀ�����
    private CameraFilterPack_Atmosphere_Rain rainFilter; // ����ϵ����˾����
    public AnimationPlayer animationPlayer; // ���ö������������
    private AudioSource rainAudioSource; // ����������Ч��AudioSource

    private float activeRainSpeed = 0.275f; // ���������ٶ�
    public AudioClip rainSound; // ������Ч�ļ�
    public CameraSwitcher switcher;

    void Start()
    {
        // ��ȡ����ϵ����˾�
        if (targetCamera != null)
        {
            rainFilter = targetCamera.GetComponent<CameraFilterPack_Atmosphere_Rain>();
            if (rainFilter != null)
            {
                rainFilter.Speed = 0f; // ��ʼ״̬Ϊ��ͣ����
            }
            else
            {
                Debug.LogError("Rain filter component is not found on the target camera.");
            }
        }
        else
        {
            Debug.LogError("Target camera is not assigned.");
        }

        // ����Ƿ������˶���������
        if (animationPlayer == null)
        {
            Debug.LogError("AnimationPlayer component is not assigned.");
        }

        // ����������Ч
        rainAudioSource = gameObject.AddComponent<AudioSource>();
        rainAudioSource.clip = rainSound;
        rainAudioSource.loop = true; // ����ѭ������
        rainAudioSource.volume = 0.5f; // ������Ҫ��������
    }

    void Update()
    {
        if (!switcher.GetSwitched()) 
        {
            if (rainAudioSource.isPlaying)
            {
                rainAudioSource.Pause(); // ��ͣ����
            }
        }
        if (rainFilter != null && animationPlayer != null)
        {
            // ���ݶ����Ƿ���ͣ�����ý����ٶȺ���������
            if (animationPlayer.IsPaused)
            {
                rainFilter.Speed = 0f;
                if (rainAudioSource.isPlaying)
                {
                    rainAudioSource.Pause(); // ��ͣ����
                }
            }
            else
            {
                rainFilter.Speed = activeRainSpeed;
                if (!rainAudioSource.isPlaying && switcher.GetSwitched())
                {
                    rainAudioSource.Play(); // ��������
                }
            }
        }
    }
}
