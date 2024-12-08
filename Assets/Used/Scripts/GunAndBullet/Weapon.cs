using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 0f;
    public GameObject bulletPrefab;
    public GameObject bulletLight;
    public ParticleSystem muzzleFlash;
    public GameObject bulletPoint;
    public float bulletSpeed = 1000f;

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {     
        if (Input.GetButtonDown(StringRepo.mouse1))
            shoot();   
    }

    void shoot()
    {
        muzzleFlash.Play();
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bulletPoint.transform.forward * bulletSpeed);

        GameObject light = Instantiate(bulletLight, bullet.transform);

        Destroy(bullet, 5);
        Destroy(light, 5);
    }   
}


