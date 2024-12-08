using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitDestroy : MonoBehaviour
{
    private ParticleSystem self;
    // Start is called before the first frame update
    void Start()
    {
        // The moment this object is insantiated play the particle animation
        self = GetComponent<ParticleSystem>();
        self.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the bullet particle animation after it's done playing
        if (self != null && !self.IsAlive())
        {
            Destroy(gameObject);
        }

    }
}
