using UnityEngine;

public class RainFilterController : MonoBehaviour
{
    public Camera targetCamera; // 指定目标相机
    private CameraFilterPack_Atmosphere_Rain rainFilter; // 相机上的雨滤镜组件
    public AnimationPlayer animationPlayer; // 引用动画播放器组件
    private AudioSource rainAudioSource; // 控制雨声音效的AudioSource

    private float activeRainSpeed = 0.275f; // 正常降雨速度
    public AudioClip rainSound; // 雨声音效文件
    public CameraSwitcher switcher;

    void Start()
    {
        // 获取相机上的雨滤镜
        if (targetCamera != null)
        {
            rainFilter = targetCamera.GetComponent<CameraFilterPack_Atmosphere_Rain>();
            if (rainFilter != null)
            {
                rainFilter.Speed = 0f; // 初始状态为暂停降雨
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

        // 检查是否设置了动画播放器
        if (animationPlayer == null)
        {
            Debug.LogError("AnimationPlayer component is not assigned.");
        }

        // 设置雨声音效
        rainAudioSource = gameObject.AddComponent<AudioSource>();
        rainAudioSource.clip = rainSound;
        rainAudioSource.loop = true; // 设置循环播放
        rainAudioSource.volume = 0.5f; // 根据需要调整音量
    }

    void Update()
    {
        if (!switcher.GetSwitched()) 
        {
            if (rainAudioSource.isPlaying)
            {
                rainAudioSource.Pause(); // 暂停雨声
            }
        }
        if (rainFilter != null && animationPlayer != null)
        {
            // 根据动画是否暂停来设置降雨速度和雨声播放
            if (animationPlayer.IsPaused)
            {
                rainFilter.Speed = 0f;
                if (rainAudioSource.isPlaying)
                {
                    rainAudioSource.Pause(); // 暂停雨声
                }
            }
            else
            {
                rainFilter.Speed = activeRainSpeed;
                if (!rainAudioSource.isPlaying && switcher.GetSwitched())
                {
                    rainAudioSource.Play(); // 播放雨声
                }
            }
        }
    }
}
