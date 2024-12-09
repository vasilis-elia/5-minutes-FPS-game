using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 0f;
    public float reloadTime = 1f;
    public GameObject bulletPrefab;
    public GameObject bulletLight;
    public ParticleSystem muzzleFlash;
    public GameObject bulletPoint;
    public float bulletSpeed = 1000f;

    private AudioSource audioSource;
    private float timer = 0f; // To keep track the time since last shot a bullet

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!PauseMenu.isPaused && !GameManager.gameOver)
            if (timer >= reloadTime && Input.GetButtonDown(StringRepo.mouse1))
            {
                Shoot();
                timer = 0f;
            }                
    }

    void Shoot()
    {
        // Play audio, instantiate bullet, bullet muzzle, and point light to follow the bullet
        audioSource.Play();
        muzzleFlash.Play();
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bulletPoint.transform.forward * bulletSpeed);

        GameObject light = Instantiate(bulletLight, bullet.transform);

        Destroy(bullet, 5);
        Destroy(light, 5);
    }   
}


