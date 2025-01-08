using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public Button[] playPauseButtons;
    public Button[] restartButtons;
    public string playTrigger = "PlayTrigger"; // Default trigger name
    public string animationName; // The name of the animation to play

    private bool isPlaying = false;

    void Start()
    {
        foreach (Button playPauseButton in playPauseButtons)
        {
            playPauseButton.onClick.AddListener(TogglePlayPause);
        }

        foreach (Button restartButton in restartButtons)
        {
            restartButton.onClick.AddListener(RestartAnimation);
        }
    }

    void TogglePlayPause()
    {
        if (!isPlaying)
        {
            animator.SetTrigger(playTrigger);
            animator.speed = 1.0f;
            isPlaying = true;
        }
        else
        {
            animator.speed = 0.0f;
            isPlaying = false;
        }
    }

    void RestartAnimation()
    {
        if (!string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName, 0, 0.0f);
            animator.speed = 0.0f;
            isPlaying = false;
        }
        else
        {
            Debug.LogError("Animation name is not set.");
        }
    }
}
