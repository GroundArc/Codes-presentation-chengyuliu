using UnityEngine;

public class Draggable3D : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 offset;
    public float dragHeight = 0.5f; // �ɵ�����ק�߶�

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isDragging)
        {
            // �����λ��ת��Ϊ�������꣬��������X��Zƽ����
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, dragHeight, 0)); // Y��Ϊ��ֱ�����ƽ��
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 point = ray.GetPoint(distance);
                transform.position = new Vector3(point.x - offset.x, dragHeight, point.z - offset.z);
                //Debug.Log("Dragging to position: " + transform.position);
            }

            // �ɿ���������ק
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                //Debug.Log("Mouse button up, ending drag.");
            }
        }
    }

    void OnMouseDown()
    {
        // ȷ�������������Ŀ����󣬶�������������
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("Ray origin: " + ray.origin + ", direction: " + ray.direction);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2.0f); // �������ߣ����ӻ�

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("Raycast hit: " + hit.transform.name + " at point: " + hit.point);
            if (hit.transform == transform)
            {
                isDragging = true;
                // ������������λ�õ�ƫ��
                Plane plane = new Plane(Vector3.up, new Vector3(0, dragHeight, 0));
                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    offset = point - transform.position;
                    //Debug.Log("Mouse down on object, starting drag. Offset: " + offset);
                }
            }
            else
            {
                //Debug.Log("Raycast hit a different object: " + hit.transform.name);
            }
        }
        else
        {
            //Debug.Log("Raycast did not hit any object.");
        }
    }
}
