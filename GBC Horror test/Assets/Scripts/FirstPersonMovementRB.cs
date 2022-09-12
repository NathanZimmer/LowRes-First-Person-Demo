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
    [SerializeField] private float headBobYDistance;
    [SerializeField] private float headBobXDistance;
    [SerializeField] private float headBobTime;

    private bool bobbing = false;
    private Rigidbody rb;
    private CapsuleCollider co;
    private bool isGrounded;
    private Transform cam;
    private Vector3 camCentralPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        co = GetComponentInChildren<CapsuleCollider>();
        cam = GameObject.Find("Main Camera").transform;
        camCentralPos = cam.transform.localPosition;
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
            if (!bobbing) StartCoroutine(HeadBob());
        }
        else
        {
            StartCoroutine(HeadHome());
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

    private IEnumerator HeadBob()
    {
        Vector3 leftDestination = new Vector3(-headBobXDistance, camCentralPos.y + headBobYDistance, 0);
        Vector3 rightDestination = new Vector3(headBobXDistance, camCentralPos.y + headBobYDistance, 0);
        float timeElapsed = 0;
        float normalizedTime;
        Vector3 camStartPos = cam.localPosition;
        bobbing = true;
        

       
        while(true)
        {

            while (cam.localPosition.x < rightDestination.x)
            {
                if (rb.velocity == Vector3.zero)
                {
                    bobbing = false;
                    yield break;
                }

                timeElapsed += Time.deltaTime;
                normalizedTime = timeElapsed / (headBobTime / 2);
                cam.localPosition = Vector3.Lerp(camStartPos, rightDestination, normalizedTime);
                yield return null;
            }

            timeElapsed = 0;

            while (cam.transform.localPosition.x > leftDestination.x)
            {
                if (rb.velocity == Vector3.zero)
                {
                    bobbing = false;
                    yield break;
                }

                timeElapsed += Time.deltaTime;
                normalizedTime = timeElapsed / headBobTime;

                cam.localPosition =
                Vector3.Lerp(
                    Vector3.Lerp(rightDestination, camCentralPos, normalizedTime),
                    Vector3.Lerp(camCentralPos, leftDestination, normalizedTime),
                    normalizedTime
                );

                yield return null;
            }

            timeElapsed = 0;

            while (cam.transform.localPosition.x < rightDestination.x)
            {
                if (rb.velocity == Vector3.zero)
                {
                    bobbing = false;
                    yield break;
                }

                timeElapsed += Time.deltaTime;
                normalizedTime = timeElapsed / headBobTime;

                cam.localPosition =
                Vector3.Lerp(
                    Vector3.Lerp(leftDestination, camCentralPos, normalizedTime),
                    Vector3.Lerp(camCentralPos, rightDestination, normalizedTime),
                    normalizedTime
                );

                yield return null;
            }

            yield return null;
        }
    }

    private IEnumerator HeadHome()
    {
        Vector3 startPos = cam.localPosition;
        bobbing = true;
        float timeElapsed = 0;
        float normalizedTime;

        while (rb.velocity == Vector3.zero)
        {
            timeElapsed += Time.deltaTime;
            normalizedTime = timeElapsed / (headBobTime / 2);

            cam.transform.localPosition = Vector3.Lerp(startPos, camCentralPos, normalizedTime);

            yield return null;
        }

        bobbing = false;
    }
}
