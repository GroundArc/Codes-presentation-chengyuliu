using UnityEngine;
using DG.Tweening;

public class CharacterFlyAroundCircle : MonoBehaviour
{
    public Transform centerPoint; // Բ�ĵ�
    public float duration = 5f; // ����һȦ��ʱ��
    public float heightVariation = 2f; // Y����������

    private Animator animator;
    private bool isFlying = false; // �����Ƿ����ڷ���
    private float radius; // �Զ�����İ뾶

    void Start()
    {
        animator = GetComponent<Animator>();

        // ����뾶�����ڽ�ɫ��ʼλ�õ�Բ�ĵľ���
        radius = Vector3.Distance(transform.position, centerPoint.position);
    }

    // ���ô˷�����ʼ����
    public void StartFly()
    {
        if (!isFlying)
        {
            isFlying = true;

            // ����·����
            Vector3[] path = GenerateCircularPath();

            // �ý�ɫ����·������
            transform.DOPath(path, duration, PathType.CatmullRom)
                .SetOptions(true) // ���ر�·������ɫ����һȦ
                .SetEase(Ease.Linear) // �����˶�
                .OnWaypointChange(OnWaypointChanged) // �ڵ���·����ʱ�л�����״̬
                .OnComplete(OnFlyComplete) // ������ɺ�Ļص�
                .OnUpdate(LockZAxisRotation); // �ڷ��й���������Z����ת
        }
    }

    // ����Բ��·��
    Vector3[] GenerateCircularPath()
    {
        int pointsCount = 20; // ��20���������Բ��
        Vector3[] path = new Vector3[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            float angle = i * Mathf.PI * 2 / pointsCount;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            float y = Mathf.Sin(angle * 2) * heightVariation; // Y������
            path[i] = new Vector3(x, y, z) + centerPoint.position;
        }

        return path;
    }

    // �ڵ���·����ʱ�������˶������л�����״̬
    void OnWaypointChanged(int waypointIndex)
    {
        Vector3 direction = transform.forward;

        // �������ж���״̬
        ResetAnimationStates();

        // ���ݷ���������Ӧ�Ķ���״̬
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

    // ���й���������Z����ת
    void LockZAxisRotation()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = 0f; // ����Z��Ϊ0
        transform.rotation = Quaternion.Euler(rotation);
    }

    // ������ɺ�Ļص�
    void OnFlyComplete()
    {
        // �������ж���״̬
        ResetAnimationStates();

        // ���÷���״̬
        isFlying = false;

        Debug.Log("Flight complete!");
    }

    // �������ж���״̬
    void ResetAnimationStates()
    {
        animator.SetBool("FlyForward", false);
        animator.SetBool("FlyBackward", false);
        animator.SetBool("FlyLeft", false);
        animator.SetBool("FlyRight", false);


    }
}