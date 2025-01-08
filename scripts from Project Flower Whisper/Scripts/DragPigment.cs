using UnityEngine;

public class DragPigment : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 offset;
    public Vector3 initialPosition; // 初始位置
    public Transform initialParent; // 初始父对象
    private FlowerBehaviour targetFlower; // 目标花朵对象

    public Item pigmentItem; // 颜料块对应的Item

    void Start()
    {
        mainCamera = Camera.main;
        initialPosition = transform.position; // 记录初始位置
        initialParent = transform.parent; // 记录初始父对象
    }

    void Update()
    {
        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 point = ray.GetPoint(distance);
                transform.position = new Vector3(point.x - offset.x, transform.position.y, point.z - offset.z);
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                if (targetFlower != null)
                {
                    ApplyPaint();
                }
                else
                {
                    ResetPosition();
                }
            }
        }
    }

    void OnMouseDown()
    {
        // 记录拖拽开始时的初始位置
        initialPosition = transform.position;
        initialParent = transform.parent;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            isDragging = true;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 point = ray.GetPoint(distance);
                offset = point - transform.position;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.name);
        if (other.CompareTag("FlowerPart"))
        {
            targetFlower = other.GetComponentInParent<FlowerBehaviour>();
            if (targetFlower != null)
            {
                foreach (var renderer in targetFlower.colorableParts)
                {
                    ReplaceAllMaterials(renderer, pigmentItem.colorMaterial);
                }
                Debug.Log("Previewing paint on flower: " + other.transform.parent.gameObject.name);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: " + other.gameObject.name);
        if (other.CompareTag("FlowerPart") && targetFlower != null)
        {
            foreach (var renderer in targetFlower.colorableParts)
            {
                Material revertMaterial = targetFlower.currentPigmentItem != null ? targetFlower.currentPigmentItem.colorMaterial : renderer.material;
                ReplaceAllMaterials(renderer, revertMaterial);
            }
            targetFlower = null;
            Debug.Log("Reverting paint on flower: " + other.transform.parent.gameObject.name);
        }
    }

    private void ApplyPaint()
    {
        if (targetFlower != null)
        {
            foreach (var renderer in targetFlower.colorableParts)
            {
                ReplaceAllMaterials(renderer, pigmentItem.colorMaterial);
            }
            targetFlower.currentPigmentItem = pigmentItem;
            ColorOwned.instance.Remove(pigmentItem);
            Debug.Log("Applied paint to flower: " + targetFlower.gameObject.name);
            // 更新容器内花的花语显示
            FlowerLanguageDisplay flowerLanguageDisplay = targetFlower.transform.parent.GetComponent<FlowerLanguageDisplay>();
            if (flowerLanguageDisplay != null)
            {
                Debug.Log("Updating flower languages on container.");
                flowerLanguageDisplay.UpdateFlowerLanguages();
            }
            targetFlower = null;
        }
        else
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        transform.position = initialPosition;
        transform.SetParent(initialParent, true);
        Debug.Log("Reset pigment position.");
    }

    // 辅助方法：替换Renderer中所有材质的材质
    private void ReplaceAllMaterials(Renderer renderer, Material newMaterial)
    {
        Material[] newMaterials = new Material[renderer.materials.Length];
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = newMaterial;
        }
        renderer.materials = newMaterials;
    }
}
