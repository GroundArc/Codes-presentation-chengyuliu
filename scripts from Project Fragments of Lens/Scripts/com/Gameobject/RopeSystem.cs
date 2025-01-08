using UnityEngine;
using System.Collections.Generic;

public class RopeSystem : MonoBehaviour
{
    public Transform startPoint;         // 起点
    public Transform endPoint;           // 终点
    public List<Transform> controlPoints; // 中间控制节点（包括midpoint）

    public LineRenderer lineRenderer;    // 用于显示绳索的LineRenderer组件
    public int resolution = 50;          // 分辨率，越高曲线越平滑
    public float ropeSpeed = 1f;         // 绳索移动的速度

    private List<Vector3> ropeNodes = new List<Vector3>(); // 用于存储计算出来的绳索节点

    private void Start()
    {
        // 初始化绳索
        InitializeRope();
    }

    private void InitializeRope()
    {
        // 清空当前绳索节点
        ropeNodes.Clear();

        // 将起点加入节点
        ropeNodes.Add(startPoint.position);

        // 添加所有的控制点
        foreach (var point in controlPoints)
        {
            ropeNodes.Add(point.position);
        }

        // 将终点加入节点
        ropeNodes.Add(endPoint.position);

        // 设置LineRenderer的节点数量
        lineRenderer.positionCount = resolution * (ropeNodes.Count - 1);

        // 绘制绳索
        DrawRope();
    }

    private void DrawRope()
    {
        for (int i = 0; i < ropeNodes.Count - 1; i++)
        {
            // 绘制当前节点与下一个节点之间的曲线段
            Vector3 p0 = ropeNodes[Mathf.Max(i - 1, 0)];
            Vector3 p1 = ropeNodes[i];
            Vector3 p2 = ropeNodes[Mathf.Min(i + 1, ropeNodes.Count - 1)];
            Vector3 p3 = ropeNodes[Mathf.Min(i + 2, ropeNodes.Count - 1)];

            // 插值，生成曲线
            for (int j = 0; j < resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 position = GetCatmullRomPosition(t, p0, p1, p2, p3);
                lineRenderer.SetPosition(i * resolution + j, position);
            }
        }
    }

    // Catmull-Rom 样条曲线
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

    // 动态移动绳子
    public void MoveRope()
    {
        // 使用 DOTween 或其他逻辑动态移动绳索节点
        for (int i = 0; i < controlPoints.Count; i++)
        {
            controlPoints[i].position = Vector3.Lerp(controlPoints[i].position, endPoint.position, Time.deltaTime * ropeSpeed);
        }

        // 绘制绳索
        DrawRope();
    }
}
