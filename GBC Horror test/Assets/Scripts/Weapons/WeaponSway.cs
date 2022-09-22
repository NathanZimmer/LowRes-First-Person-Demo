using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Position")]
    [SerializeField] private float swayYDistance;
    [SerializeField] private float swayXDistance;
    [SerializeField] private float swayTime;
    [SerializeField] private float returnHomeFactor;
    [Header("Sway Rotation")]
    [SerializeField] private float rotationSwayModifier;
    [SerializeField] private float rotationSwaySmooth;
    [Header("Aiming")]
    [SerializeField] private Vector3 aimPosition = new Vector3(0, -0.2f, 0.2f);
    [SerializeField] private float aimTime;
    [Header("Other")]
    [SerializeField] private PlayerMovement Movement;

    // see the aiming coroutine for understanding why there are som many target vectors
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
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    private void OnEnable()
    {
        homePosition = transform.localPosition;
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;

        leftTarget = new Vector3(homePosition.x - swayXDistance, homePosition.y + swayYDistance, homePosition.z);
        rightTarget = new Vector3(homePosition.x + swayXDistance, homePosition.y + swayYDistance, homePosition.z);
        adsLeftTarget = new Vector3(aimPosition.x - swayXDistance, aimPosition.y + swayYDistance, aimPosition.z);
        adsRightTarget = new Vector3(aimPosition.x + swayXDistance, aimPosition.y + swayYDistance, aimPosition.z);

        currentLeftTarget = leftTarget;
        currentRightTarget = rightTarget;
        currentCenterTarget = homePosition;
    }

    private void OnDisable()
    {
        transform.localPosition = defaultPosition;
        transform.localRotation = defaultRotation;

        currentLeftTarget = leftTarget;
        currentRightTarget = rightTarget;
        currentCenterTarget = homePosition;

        aiming = false;
        bobbing = false;
        StopAllCoroutines();
    }

    private void Update()
    {
        // lerps current targets from default positions to ADS positions when mouse1 is pressed and back when it is released
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            StartCoroutine(Aim());
        }

        if (Movement.IsMoving() && !bobbing)
        {
            StartCoroutine(Bobbing());
        }
        else if (!Movement.IsMoving() && !bobbing)
        {
            StopCoroutine(Bobbing());
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

            // moves from start position to right target
            while (transform.localPosition.x < currentRightTarget.x)
            {
                if (!Movement.IsMoving())
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

            // moves from right target to middle target and then to left target in nested lerp
            while (transform.localPosition.x > currentLeftTarget.x)
            {
                if (!Movement.IsMoving())
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

            // moves from left target to middle target and then to right target in nested lerp
            while (transform.localPosition.x < currentRightTarget.x)
            {
                if (!Movement.IsMoving())
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

        while (!Movement.IsMoving())
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

        // if we are aiming, lerps current targets from default targets to ADS targets
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

        // if we are not aiming, lerps current targets from ADS targets to default targets
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
