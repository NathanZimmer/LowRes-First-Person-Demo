using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Shake(0.2f, 0.1f));
        }
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orinalPos = transform.localPosition;

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;
            Vector3 target = new Vector3(x, y, transform.localPosition.z);
            timeElapsed += Time.deltaTime;
            float normalizedTime = timeElapsed / duration;

            //transform.localPosition = Vector3.Lerp(transform.position, target, normalizedTime);
            transform.localPosition = new Vector3(orinalPos.x + x, orinalPos.y + y, orinalPos.z);

            yield return null;
        }

        transform.localPosition = orinalPos;
    }
}
