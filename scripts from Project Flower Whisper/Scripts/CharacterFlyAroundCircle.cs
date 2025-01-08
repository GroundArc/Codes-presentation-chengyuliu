using UnityEngine;
using DG.Tweening;

public class CharacterFlyAroundCircle : MonoBehaviour
{
    public Transform centerPoint; // 圆心点
    public float duration = 5f; // 飞行一圈的时间
    public float heightVariation = 2f; // Y轴的起伏幅度

    private Animator animator;
    private bool isFlying = false; // 控制是否正在飞行
    private float radius; // 自动计算的半径

    void Start()
    {
        animator = GetComponent<Animator>();

        // 计算半径，等于角色初始位置到圆心的距离
        radius = Vector3.Distance(transform.position, centerPoint.position);
    }

    // 调用此方法开始飞行
    public void StartFly()
    {
        if (!isFlying)
        {
            isFlying = true;

            // 生成路径点
            Vector3[] path = GenerateCircularPath();

            // 让角色沿着路径飞行
            transform.DOPath(path, duration, PathType.CatmullRom)
                .SetOptions(true) // 不关闭路径，角色飞行一圈
                .SetEase(Ease.Linear) // 匀速运动
                .OnWaypointChange(OnWaypointChanged) // 在到达路径点时切换动画状态
                .OnComplete(OnFlyComplete) // 飞行完成后的回调
                .OnUpdate(LockZAxisRotation); // 在飞行过程中锁定Z轴旋转
        }
    }

    // 生成圆形路径
    Vector3[] GenerateCircularPath()
    {
        int pointsCount = 20; // 用20个点来描绘圆形
        Vector3[] path = new Vector3[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            float angle = i * Mathf.PI * 2 / pointsCount;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            float y = Mathf.Sin(angle * 2) * heightVariation; // Y轴的起伏
            path[i] = new Vector3(x, y, z) + centerPoint.position;
        }

        return path;
    }

    // 在到达路径点时，根据运动方向切换动画状态
    void OnWaypointChanged(int waypointIndex)
    {
        Vector3 direction = transform.forward;

        // 重置所有动画状态
        ResetAnimationStates();

        // 根据方向设置相应的动画状态
        if (Vector3.Dot(direction, Vector3.forward) > 0.5f)
        {
            animator.SetBool("FlyForward", true);
        }
        else if (Vector3.Dot(direction, Vector3.back) > 0.5f)
        {
            animator.SetBool("FlyBackward", true);
        }
        else if (Vector3.Dot(direction, Vector3.left) > 0.5f)
        {
            animator.SetBool("FlyLeft", true);
        }
        else if (Vector3.Dot(direction, Vector3.right) > 0.5f)
        {
            animator.SetBool("FlyRight", true);
        }
    }

    // 飞行过程中锁定Z轴旋转
    void LockZAxisRotation()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = 0f; // 保持Z轴为0
        transform.rotation = Quaternion.Euler(rotation);
    }

    // 飞行完成后的回调
    void OnFlyComplete()
    {
        // 重置所有动画状态
        ResetAnimationStates();

        // 重置飞行状态
        isFlying = false;

        Debug.Log("Flight complete!");
    }

    // 重置所有动画状态
    void ResetAnimationStates()
    {
        animator.SetBool("FlyForward", false);
        animator.SetBool("FlyBackward", false);
        animator.SetBool("FlyLeft", false);
        animator.SetBool("FlyRight", false);


    }
}