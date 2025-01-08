using UnityEngine;

public class LineUpdater : MonoBehaviour
{
    private Transform startPoint;
    private Transform endPoint;
    private LineRenderer lineRenderer;

    public void SetTargets(Transform start, Transform end)
    {
        startPoint = start;
        endPoint = end;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (lineRenderer != null && startPoint != null && endPoint != null)
        {
            // Update the positions of the line
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }
    }
}
