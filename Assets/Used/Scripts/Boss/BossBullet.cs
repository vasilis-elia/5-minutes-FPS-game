using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject weapon;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == StringRepo.Player)
        {
            // At the location of the hit create a particle object. Play() and destruction are managed by the object itself
            GameObject particle = Instantiate(particlePrefab, col.transform.position, Quaternion.identity);          

            // Damage the player will take depends on the damage of the boss weapon
            int damage = weapon.GetComponent<BossWeapon>().damage;
            col.gameObject.GetComponent<Player>().TakeDamage(damage);

            // Bullet is destroyed if it hits the player
            Destroy(gameObject);           
        }
    }
}
