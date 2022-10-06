using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolSM : WeaponSM
{
    [SerializeField] private float bulletDamage;
    [SerializeField] private float maxDistance;
    [SerializeField] private float maxSpread;
    [SerializeField] private float spreadIncreaseTime;
    [SerializeField] private float spreadDecreaseTime;
    [SerializeField] private float spreadDecreaseDelay;

    private float xSpread = 0;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("Equip");
        StartCoroutine(Spread());
    }

    protected override void Projectile()
    {
        Vector3 bulletDirection = mainCam.forward + mainCam.TransformDirection(new Vector3(Random.Range(-xSpread, xSpread), Random.Range(-xSpread, xSpread), 0));
        Physics.Raycast(mainCam.position, bulletDirection, out RaycastHit hit, maxDistance, bulletsHit);

        if (hit.point != null)
        {
            DrawDecals(hit);

            if (hit.transform.CompareTag(enemyTag))
            {
                hit.transform.gameObject.GetComponent<EnemyTest>().CalculateHits(bulletDamage);
            }
        }
    }

    private IEnumerator Spread()
    {
        float timeElapsed;
        float decreaseTimer = 0;

        while (true)
        {
            timeElapsed = 0;

            while ((animator.GetCurrentAnimatorStateInfo(0).IsName("Shot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Last Shot")) && xSpread < maxSpread)
            {
                decreaseTimer = 0;
                timeElapsed += Time.deltaTime;

                xSpread += (maxSpread / spreadIncreaseTime) * Time.deltaTime;

                yield return null;
            }

            timeElapsed = 0;

            while (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Shot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Last Shot")) && decreaseTimer > spreadDecreaseDelay && xSpread > 0)
            {
                timeElapsed += Time.deltaTime;

                xSpread -= (maxSpread / spreadDecreaseTime) * Time.deltaTime;

                yield return null;
            }

            if (decreaseTimer < spreadDecreaseDelay)
                decreaseTimer += Time.deltaTime;

            yield return null;
        }
    }
}
