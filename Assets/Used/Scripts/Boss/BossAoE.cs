using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAoE : MonoBehaviour
{
    public GameObject player;
    public int damagePerSecond = 1;
    public float range = 37f;

    float timer; // For keeping track the duration when then player is close to the boss
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(StringRepo.Player);
        if (player == null)
            Debug.Log("Player object not found");
    }

    // Update is called once per frame
    void Update()
    {
        // Update the positions and direction towards player on each frame
        Vector3 playerPosition = player.transform.position;
        Vector3 currentPosition = transform.position;
        float distance = (playerPosition - currentPosition).magnitude;        

        if (distance < range)
        {            
            DoDamage(); // While the player is within range the timer will stack up and at each 1 second they will take damagePerSecond damage
        }
        else
            timer = 0f; // Reset the timer if the player goes of the AoE range
    }

    void DoDamage()
    {
        timer += Time.deltaTime;      
        // Every one second do damagePerSecond damage to the player
        if (timer > 1f)
        {
            player.GetComponent<Player>().TakeDamage(damagePerSecond);
            timer = 0f;
        }
    }    
}
