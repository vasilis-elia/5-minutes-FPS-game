using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGetHit : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject weapon;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == StringRepo.Bullet + "(Clone)")
        {
            // At the location of the hit create a particle object. Play() and destruction are managed by the object itself
            GameObject particle = Instantiate(particlePrefab, col.transform.position, Quaternion.identity);
            particle.transform.SetParent(transform);

            // If a bullet hits the Boss the Boss takes damage and the bullet is destroyed instantly            
            GetComponent<Boss>().TakeDamage(weapon.GetComponent<Weapon>().damage);

            Destroy(col.gameObject);
        }
    }
}
