using UnityEngine;
using System.Collections.Generic;

public class RopeSystem : MonoBehaviour
{
    public Transform startPoint;         // ���
    public Transform endPoint;           // �յ�
    public List<Transform> controlPoints; // �м���ƽڵ㣨����midpoint��

    public LineRenderer lineRenderer;    // ������ʾ������LineRenderer���
    public int resolution = 50;          // �ֱ��ʣ�Խ������Խƽ��
    public float ropeSpeed = 1f;         // �����ƶ����ٶ�

    private List<Vector3> ropeNodes = new List<Vector3>(); // ���ڴ洢��������������ڵ�

    private void Start()
    {
        // ��ʼ������
        InitializeRope();
    }

    private void InitializeRope()
    {
        // ��յ�ǰ�����ڵ�
        ropeNodes.Clear();

        // ��������ڵ�
        ropeNodes.Add(startPoint.position);

        // ������еĿ��Ƶ�
        foreach (var point in controlPoints)
        {
            ropeNodes.Add(point.position);
        }

        // ���յ����ڵ�
        ropeNodes.Add(endPoint.position);

        // ����LineRenderer�Ľڵ�����
        lineRenderer.positionCount = resolution * (ropeNodes.Count - 1);

        // ��������
        DrawRope();
    }

    private void DrawRope()
    {
        for (int i = 0; i < ropeNodes.Count - 1; i++)
        {
            // ���Ƶ�ǰ�ڵ�����һ���ڵ�֮������߶�
            Vector3 p0 = ropeNodes[Mathf.Max(i - 1, 0)];
            Vector3 p1 = ropeNodes[i];
            Vector3 p2 = ropeNodes[Mathf.Min(i + 1, ropeNodes.Count - 1)];
            Vector3 p3 = ropeNodes[Mathf.Min(i + 2, ropeNodes.Count - 1)];

            // ��ֵ����������
            for (int j = 0; j < resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 position = GetCatmullRomPosition(t, p0, p1, p2, p3);
                lineRenderer.SetPosition(i * resolution + j, position);
            }
        }
    }

    // Catmull-Rom ��������
    private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 position = 0.5f * (
            (2.0f * p1) +
            (-p0 + p2) * t +
            (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t2 +
            (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3
        );

        return position;
    }

    // ��̬�ƶ�����
    public void MoveRope()
    {
        // ʹ�� DOTween �������߼���̬�ƶ������ڵ�
        for (int i = 0; i < controlPoints.Count; i++)
        {
            controlPoints[i].position = Vector3.Lerp(controlPoints[i].position, endPoint.position, Time.deltaTime * ropeSpeed);
        }

        // ��������
        DrawRope();
    }
}
