using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallDetector : MonoBehaviour
{
    public GameObject player;

    // If the player falls off the map and touches this object they die instantly
    private void Update()
    {
        if (player.transform.position.y < transform.position.y)
            player.GetComponent<Player>().TakeDamage(100);
    }
}
