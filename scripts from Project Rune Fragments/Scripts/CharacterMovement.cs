using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]


public class CharacterMovement : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    private Rigidbody rb;
    public Vector3 CurrentInput { get; private set; }
    public float MaxWalkSpeed = 5f;
    public float SpeedMultiplier = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + CurrentInput * MaxWalkSpeed * SpeedMultiplier * Time.fixedDeltaTime);

    }
    public void SetMovementInput(Vector3 input)
    {

        CurrentInput = Vector3.ClampMagnitude(input, 1f);
    }

    public Vector3 GetDirection()
    {
        return CurrentInput;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        SpeedMultiplier = multiplier;
    }
}
