using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RakeGetHit : MonoBehaviour
{
    public GameObject particlePrefab;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == StringRepo.Bullet + "(Clone)")
        {
            // At the location of the hit create a particle object. Play() and destruction are managed by the object itself
            GameObject particle = Instantiate(particlePrefab, col.transform.position, Quaternion.identity);
            particle.transform.SetParent(transform);

            // If a bullet hits a Rake the Rake takes damage and the bullet is destroyed instantly
            Rake rake = GetComponent<Rake>();
            rake.TakeDamage(1);
            Destroy(col.gameObject);
        }
    }
}
