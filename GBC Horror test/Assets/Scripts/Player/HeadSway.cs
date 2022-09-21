using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSway : MonoBehaviour
{
    [Header("Sway Position")]
    [SerializeField] protected float swayYDistance;
    [SerializeField] protected float swayXDistance;
    [SerializeField] protected float swayTime;
    [SerializeField] protected float returnHomeFactor;
    [Header("Other")]
    [SerializeField] protected PlayerMovement Movement;

    protected Vector3 leftTarget;
    protected Vector3 rightTarget;
    protected bool bobbing = false;
    protected Vector3 homePosition;

    private void Start()
    {
        homePosition = transform.localPosition;

        leftTarget = new Vector3(homePosition.x - swayXDistance, homePosition.y + swayYDistance, homePosition.z);
        rightTarget = new Vector3(homePosition.x + swayXDistance, homePosition.y + swayYDistance, homePosition.z);
    }

    private void Update()
    {
        if (Movement.IsMoving() && !bobbing)
        {
            StartCoroutine(Bobbing());
        }
        else if (!Movement.IsMoving() && !bobbing)
        {
            StopCoroutine(Bobbing());
            StartCoroutine(HeadHome());
        }
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
            while (transform.localPosition.x < rightTarget.x)
            {
                if (!Movement.IsMoving())
                {
                    bobbing = false;
                    yield break;
                }

                timeElapsed += Time.deltaTime;
                normalizedTime = timeElapsed / (swayTime / 2);
                transform.localPosition = Vector3.Lerp(camStartPos, rightTarget, normalizedTime);
                yield return null;
            }

            timeElapsed = 0;

            // moves from right target to middle target and then to left target in nested lerp
            while (transform.localPosition.x > leftTarget.x)
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
                    Vector3.Lerp(rightTarget, homePosition, normalizedTime),
                    Vector3.Lerp(homePosition, leftTarget, normalizedTime),
                    normalizedTime
                );

                yield return null;
            }

            timeElapsed = 0;

            // moves from left target to middle target and then to right target in nested lerp
            while (transform.localPosition.x < rightTarget.x)
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
                    Vector3.Lerp(leftTarget, homePosition, normalizedTime),
                    Vector3.Lerp(homePosition, rightTarget, normalizedTime),
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

            transform.localPosition = Vector3.Lerp(startPos, homePosition, normalizedTime);

            yield return null;
        }

        bobbing = false;
    }
}
