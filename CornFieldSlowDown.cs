using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CornfieldSlowdown : MonoBehaviour
{
    public float slowedSpeed = 3f; // Speed when in the cornfield
    private float originalSpeed; // Player's original speed
    private ThirdPersonMovement playerMovement; // Reference to the player's movement script

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        if (player != null)
        {
            playerMovement = player.GetComponent<ThirdPersonMovement>();
            if (playerMovement != null)
            {
                originalSpeed = playerMovement.speed;
                Debug.Log("Original speed set to: " + originalSpeed);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // check if player entered cornfield
        if (other.CompareTag("player"))
        {
            Debug.Log("Player entered the cornfield.");
            
            if (playerMovement != null)
            {
                playerMovement.speed = slowedSpeed;  // Apply slowed speed
                Debug.Log("Player speed reduced to: " + slowedSpeed);
            } else
            {
                Debug.Log("can't find script");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // check if the player exited the cornfield
        if (other.CompareTag("player") && playerMovement != null)
        {
            playerMovement.speed = originalSpeed; // restore original speed

            Debug.Log("orig speed" + originalSpeed);
            Debug.Log("Player speed back to " + playerMovement.speed);
        }
    }
}
