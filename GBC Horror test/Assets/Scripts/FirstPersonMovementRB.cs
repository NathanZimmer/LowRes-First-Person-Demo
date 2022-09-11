using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovementRB : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float acceleration = 10;
    [SerializeField] private float airAcceleration = 1;
    [SerializeField] private float jumpAcceleration = 100;
    [SerializeField] private float friction = 10;
    [SerializeField] private float airFriction = 0.1f;
    [Header("Other")]
    [SerializeField] private float stepHeight = 0.1f;
    [SerializeField] private float stepForce = 1f;
    [SerializeField] private LayerMask groundMask;
    private Rigidbody rb;
    private CapsuleCollider co;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        co = GetComponentInChildren<CapsuleCollider>();
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        //isGrounded = Physics.Raycast(co.bounds.center, Vector3.down, co.bounds.extents.y + 0.1f, groundMask);
        //isGrounded = Physics.SphereCast(co.bounds.center, co.radius, Vector3.down, out RaycastHit hit, 0.5f, groundMask);
        isGrounded = Physics.BoxCast(co.bounds.center, new Vector3(co.radius, 0.1f, co.radius), Vector3.down, Quaternion.Euler(Vector3.zero), co.bounds.extents.y + 0.1f);
        Movement();
    }

    private void Movement()
    {
        Vector3 movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        rb.drag = (isGrounded) ? friction : airFriction;

        if (isGrounded)
            rb.AddForce(movement.normalized * acceleration, ForceMode.Acceleration);
        else
            rb.AddForce(movement.normalized * airAcceleration, ForceMode.Acceleration);

        if (movement != Vector3.zero)
        {
            Step(movement.normalized);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpAcceleration, ForceMode.Acceleration);
        }
    }

    // if raycast from bottom hits an object and raycast from top does not, player "steps" up onto the object
    private void Step(Vector3 direction)
    {
        bool bottomCast = Physics.Raycast(transform.position + new Vector3(0, 0.01f, 0), direction, out RaycastHit bottomHit, co.bounds.extents.z + 0.1f);
        bool topCast = Physics.Raycast(transform.position + new Vector3(0, stepHeight, 0), direction, out RaycastHit topHit, co.bounds.extents.z + 0.2f);

        // debug
        Debug.DrawRay(transform.position, direction * (co.bounds.extents.z + 0.1f));
        Debug.DrawRay(transform.position + new Vector3(0, stepHeight, 0), direction * (co.bounds.extents.z + 0.2f));

        if (bottomCast && !topCast)
        {
            rb.AddForce(Vector3.up * stepForce, ForceMode.Acceleration);
        }

    }
}
