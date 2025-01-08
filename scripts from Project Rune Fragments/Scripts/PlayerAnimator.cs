using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        Vector3 moveDirection = movement.GetDirection().normalized;
        Vector3 lookDirection = transform.forward;

        float angle = Vector3.Angle(lookDirection, moveDirection);
        Vector3 cross = Vector3.Cross(lookDirection, moveDirection);

        if (cross.y < 0) angle = -angle;

        float moveSpeed = moveDirection.magnitude;
        float horizontal = Mathf.Sin(angle * Mathf.Deg2Rad) * moveSpeed;
        float vertical = Mathf.Cos(angle * Mathf.Deg2Rad) * moveSpeed;

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        // Adjust animation speed based on movement speed
        float speedMultiplier = Mathf.Clamp(moveSpeed * 1.5f, 0.5f, 2f);
        animator.SetFloat("AnimationSpeed", speedMultiplier);
        animator.speed = animator.GetFloat("AnimationSpeed");
    }
}

