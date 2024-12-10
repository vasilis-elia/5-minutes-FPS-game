using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int damage = 1;
    public float lowReload = 1f;  // Time threshholds for firing the gun
    public float highReload = 2f;    
    public GameObject bulletPrefab;
    public GameObject bulletLight;
    public ParticleSystem muzzleFlash;
    public GameObject bulletPoint;
    public GameObject player;
    public GameObject boss;
    public float bulletSpeed = 700f;
    public float recoil = 10f;

    private AudioSource audioSource;
    private float shootTime; // The actual reload time calculated randomly between low and high reload
    private float timer = 0f; // To keep track the time since last shot a bullet

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shootTime = UnityEngine.Random.value * (highReload - lowReload) + lowReload;

        // So the player is certain to be instantiated first
        StartCoroutine(Waiter());

        // Each boss weapon will find the player on its own
        player = GameObject.Find(StringRepo.Player);
        if (player == null)
            Debug.Log("Player object not found");
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused || GameManager.gameOver)
            return;

        // Wait for the boss to land before aiming or shooting at the player. Also if the boss dies stop shooting
        if (!boss.GetComponent<BossMovement>().HasLanded() || boss.GetComponent<Boss>().IsDead())
            return;

        timer += Time.deltaTime;
        
        transform.LookAt(player.transform.position);
        transform.rotation *= Quaternion.Euler(-90f, 0f, 0f); // Because the model is not oriented correctly
       if (timer >= shootTime)
       {
            Shoot();
            timer = 0f;
            shootTime = UnityEngine.Random.value * (highReload - lowReload) + lowReload;
        }                
    }

    void Shoot()
    {     
        // Play audio, instantiate bullet, bullet muzzle, and point light to follow the bullet
        audioSource.Play();
        muzzleFlash.Play();
             
        // Extra degrees to rotate the weapon before firing (for recoil)
        float randomVal1 = UnityEngine.Random.value * recoil - recoil / 2; // minus recoil so there are negative value as well
        float randomVal2 = UnityEngine.Random.value * recoil - recoil / 2;
        float randomVal3 = UnityEngine.Random.value * recoil - recoil / 2;

        Vector3 eulerRotation = new Vector3 (randomVal1, randomVal2, randomVal3);

        // Tne new rotation the weapon will have before firing (on the same position)
        transform.Rotate(eulerRotation, Space.Self);    
        
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bulletPoint.transform.forward * bulletSpeed);

        GameObject light = Instantiate(bulletLight, bullet.transform);

        Destroy(bullet, 5);
        Destroy(light, 5);
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.5f);
    }
}


