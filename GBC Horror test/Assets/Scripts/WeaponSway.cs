using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float swayYDistance;
    [SerializeField] private float swayXDistance;
    [SerializeField] private float swayTime;
    [SerializeField] private float returnHomeFactor;
    [SerializeField] private Vector3 aimPosition = new Vector3(0, -0.2f, 0.2f);
    [SerializeField] private float aimTime;
    [SerializeField] private float rotationSwayModifier;
    [SerializeField] private float rotationSwaySmooth;

    private Vector3 leftTarget;
    private Vector3 rightTarget;
    private Vector3 adsLeftTarget;
    private Vector3 adsRightTarget;
    private Vector3 currentLeftTarget;
    private Vector3 currentRightTarget;
    private Vector3 currentCenterTarget;
    private bool bobbing = false;
    private bool aiming = false;
    private Vector3 homePosition;
    [SerializeField] private FirstPersonMovementRB Movement;

    private void Start()
    {
        homePosition = transform.localPosition;
        //Movement = GetComponentInParent<FirstPersonMovementRB>();

        leftTarget = new Vector3(homePosition.x - swayXDistance, homePosition.y + swayYDistance, homePosition.z);
        rightTarget = new Vector3(homePosition.x + swayXDistance, homePosition.y + swayYDistance, homePosition.z);
        adsLeftTarget = new Vector3(aimPosition.x - swayXDistance, aimPosition.y + swayYDistance, aimPosition.z);
        adsRightTarget = new Vector3(aimPosition.x + swayXDistance, aimPosition.y + swayYDistance, aimPosition.z);

        currentLeftTarget = leftTarget;
        currentRightTarget = rightTarget;
        currentCenterTarget = homePosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            StartCoroutine(Aim());
        }

        if (Movement.GetIsMoving() && !bobbing)
        {
            StartCoroutine(Bobbing());
        }
        else if (!Movement.GetIsMoving())
        {
            StartCoroutine(HeadHome());
        }

        RotationSetter();
    }

    private IEnumerator Bobbing()
    {   
        float timeElapsed = 0;
        float normalizedTime;
        Vector3 camStartPos = transform.localPosition;
        bobbing = true;


        while (true)
        {
            while (transform.localPosition.x < currentRightTarget.x)
            {
                if (!Movement.GetIsMoving())
                {
                    bobbing = false;
                    yield break;
                }

                timeElapsed += Time.deltaTime;
                normalizedTime = timeElapsed / (swayTime / 2);
                transform.localPosition = Vector3.Lerp(camStartPos, currentRightTarget, normalizedTime);
                yield return null;
            }

            timeElapsed = 0;

            while (transform.localPosition.x > currentLeftTarget.x)
            {
                if (!Movement.GetIsMoving())
                {
                    bobbing = false;
                    yield break;
                }

                timeElapsed += Time.deltaTime;
                normalizedTime = timeElapsed / swayTime;

                transform.localPosition =
                Vector3.Lerp(
                    Vector3.Lerp(currentRightTarget, currentCenterTarget, normalizedTime),
                    Vector3.Lerp(currentCenterTarget, currentLeftTarget, normalizedTime),
                    normalizedTime
                );

                yield return null;
            }

            timeElapsed = 0;

            while (transform.localPosition.x < currentRightTarget.x)
            {
                if (!Movement.GetIsMoving())
                {
                    bobbing = false;
                    yield break;
                }

                timeElapsed += Time.deltaTime;
                normalizedTime = timeElapsed / swayTime;

                transform.localPosition =
                Vector3.Lerp(
                    Vector3.Lerp(currentLeftTarget, currentCenterTarget, normalizedTime),
                    Vector3.Lerp(currentCenterTarget, currentRightTarget, normalizedTime),
                    normalizedTime
                );

                yield return null;
            }

            yield return null;
        }
    }

    private IEnumerator HeadHome()
    {
        Vector3 startPos = transform.localPosition;
        bobbing = true;
        float timeElapsed = 0;
        float normalizedTime;

        while (!Movement.GetIsMoving())
        {
            timeElapsed += Time.deltaTime;
            normalizedTime = timeElapsed / (swayTime / returnHomeFactor);

            transform.localPosition = Vector3.Lerp(startPos, currentCenterTarget, normalizedTime);

            yield return null;
        }

        bobbing = false;
    }

    private IEnumerator Aim()
    {
        float timeElapsed = 0;
        float normalizedTime;

        aiming = !aiming;

        while (aiming && currentCenterTarget != aimPosition)
        {
            timeElapsed += Time.deltaTime;
            normalizedTime = timeElapsed / aimTime;

            currentCenterTarget = Vector3.Lerp(homePosition, aimPosition, normalizedTime);
            currentLeftTarget = Vector3.Lerp(leftTarget, adsLeftTarget, normalizedTime);
            currentRightTarget = Vector3.Lerp(rightTarget, adsRightTarget, normalizedTime);

            yield return null;
        }

        timeElapsed = 0;

        while (!aiming && currentCenterTarget != homePosition)
        {
            timeElapsed += Time.deltaTime;
            normalizedTime = timeElapsed / aimTime;

            currentCenterTarget = Vector3.Lerp(aimPosition, homePosition, normalizedTime);
            currentLeftTarget = Vector3.Lerp(adsLeftTarget, leftTarget, normalizedTime);
            currentRightTarget = Vector3.Lerp(adsRightTarget, rightTarget, normalizedTime);

            yield return null;
        }
    }

    private void RotationSetter()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * rotationSwayModifier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * rotationSwayModifier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotationX * rotationY, rotationSwaySmooth * Time.deltaTime);
    }
}
