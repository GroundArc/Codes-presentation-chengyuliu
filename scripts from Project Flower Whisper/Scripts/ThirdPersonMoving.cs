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
    public Transform workstation; // ����̨��Transform
    public Transform flowereditor; // �༭����Transform
    public Transform holdPosition; // ���ʰȡ������ĳ���λ��
    public Transform spawnPoint; // �������Transform
    public float interactionDistance = 7f; // �빤��̨�����ľ���

    public Animator animator; // �ڱ༭�����ֶ�ָ��Animator���

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

        // ��ʼ�������״̬
        modeSwitcher.SetMainMode();

        // ȷ��Animator�����ָ��
        if (animator == null)
        {
            Debug.LogError("Animator component not assigned!");
        }
    }

    void Update()
    {
        if (modeSwitcher != null && modeSwitcher.isInFlowerEditorMode)
        {
            // �ڻ��༭��ģʽ�¼�� ESC ��
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

        // ����Speed�����Կ��ƶ���
        float currentSpeed = direction.magnitude * speed;
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }

        if (direction.magnitude >= 0.1f)
        {
            // ������ҽ�ɫ����Ϊ�ƶ�����
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

        // ���R������˲���ƶ���������
        if (Input.GetKeyDown(KeyCode.R))
        {
            TeleportToSpawnPoint();
        }
    }

    public void TeleportToSpawnPoint()
    {
        if (spawnPoint != null)
        {
            // ��ʱ���� CharacterController
            controller.enabled = false;

            // ˲���ƶ���������λ��
            transform.position = spawnPoint.position;

            // ������תΪ (0, 0, 0)
            transform.rotation = Quaternion.identity;

            // ����velocity�Ա����������Ч��
            velocity = Vector3.zero;

            // ȷ��λ���Ѹ��º��������� CharacterController
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
