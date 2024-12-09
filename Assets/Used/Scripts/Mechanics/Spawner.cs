using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject rake;
    public GameObject player;
    public float spawnDistance;

    bool hasSpawned = false;
    bool hasSpawnedTimeUp = false;    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Waiter());

        player = GameObject.Find(StringRepo.Player);
        if (player == null)
            Debug.Log("Player object not found");
        
        // Spawning distance should be equal to the aggro distance of the rake
        spawnDistance = rake.GetComponent<RakeMovement>().aggroDistance;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player has finished don't spawn anymore rakes
        if (GameManager.hasFinished)
            return;

        checkWhetherToSpawn();
        if (GameManager.timesUp && !hasSpawnedTimeUp)
        {
            Spawn();
            hasSpawnedTimeUp = true;
        }            
    }

    // Each spawner will spawn one rake
    void checkWhetherToSpawn()
    {
        if (hasSpawned)
            return;

        Vector3 targetPosition = player.transform.position;
        Vector3 currentPosition = transform.position;
        float distance = (targetPosition - currentPosition).magnitude;

        // Spawn a rake if player comes within radius
        if (distance <= spawnDistance)
        {
            hasSpawned = true;
            Spawn();            
        }
    }

    // Spawns a rake at own position
    public void Spawn()
    {
        Instantiate(rake, transform.position, Quaternion.identity);
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
