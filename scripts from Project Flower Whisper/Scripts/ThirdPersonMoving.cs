using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMoving : MonoBehaviour
{
    public CharacterController controller;
    public Camera mainCamera;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform groundCheck;
    public Transform workstation; // 工作台的Transform
    public Transform flowereditor; // 编辑器的Transform
    public Transform holdPosition; // 玩家拾取花束后的持有位置
    public Transform spawnPoint; // 出生点的Transform
    public float interactionDistance = 7f; // 与工作台交互的距离

    public Animator animator; // 在编辑器中手动指定Animator组件

    private float turnSmoothVelocity;
    private Vector3 velocity;
    private bool isGrounded;

    private ModeSwitcher modeSwitcher;
    private Backpack backpack;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        modeSwitcher = FindObjectOfType<ModeSwitcher>();

        if (modeSwitcher == null)
        {
            Debug.LogError("ModeSwitcher component not found in the scene. Please ensure there is a GameObject with ModeSwitcher attached.");
        }
        else
        {
            Debug.Log("ModeSwitcher component found.");
        }

        // 初始化摄像机状态
        modeSwitcher.SetMainMode();

        // 确保Animator组件已指定
        if (animator == null)
        {
            Debug.LogError("Animator component not assigned!");
        }
    }

    void Update()
    {
        if (modeSwitcher != null && modeSwitcher.isInFlowerEditorMode)
        {
            // 在花编辑器模式下检测 ESC 键
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                modeSwitcher.ExitFlowerEditorMode();
            }
            return;
        }

        // Ground check to ensure the character is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // A small negative value to ensure the character stays grounded
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(h, 0f, v).normalized;

        // 更新Speed参数以控制动画
        float currentSpeed = direction.magnitude * speed;
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }

        if (direction.magnitude >= 0.1f)
        {
            // 设置玩家角色朝向为移动方向
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Interaction check
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        // Check if player is near the workstation and presses F to interact (store colors)
        if (Vector3.Distance(transform.position, workstation.position) <= interactionDistance && Input.GetKeyDown(KeyCode.F))
        {
            Backpack.instance.TransferToColorOwned();
        }

        // Check if player is near the flowereditor and presses F to interact (switch to flower editor mode)
        if (Vector3.Distance(transform.position, flowereditor.position) <= interactionDistance && Input.GetKeyDown(KeyCode.F))
        {
            modeSwitcher.EnterFlowerEditorMode();
        }

        // 检测R键触发瞬间移动到出生点
        if (Input.GetKeyDown(KeyCode.R))
        {
            TeleportToSpawnPoint();
        }
    }

    public void TeleportToSpawnPoint()
    {
        if (spawnPoint != null)
        {
            // 暂时禁用 CharacterController
            controller.enabled = false;

            // 瞬间移动到出生点位置
            transform.position = spawnPoint.position;

            // 设置旋转为 (0, 0, 0)
            transform.rotation = Quaternion.identity;

            // 重置velocity以避免物理残留效果
            velocity = Vector3.zero;

            // 确保位置已更新后重新启用 CharacterController
            controller.enabled = true;

            Debug.Log("Player teleported to spawn point with rotation (0, 0, 0).");
        }
        else
        {
            Debug.LogError("Spawn point is not assigned!");
        }
    }



    private void DropContainer()
    {
        if (holdPosition.childCount > 0)
        {
            Transform container = holdPosition.GetChild(0);
            container.SetParent(null); // Remove from hold position
            container.position = transform.position; // Set position to player's current position
            container.rotation = Quaternion.Euler(-90, transform.eulerAngles.y, 0); // Set rotation with X axis -90
            Debug.Log("Dropped container at player's position.");
        }
        else
        {
            Debug.Log("No container to drop.");
        }
    }
}
