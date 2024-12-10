using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject boss;

    // Spawns the boss at own position
    public void Spawn()
    {
        Instantiate(boss, transform.position, transform.rotation);
    }
}
