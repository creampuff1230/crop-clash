using System.Collections;
using UnityEngine;

public class GateOpener : MonoBehaviour
{
    public float openDuration = 0.5f; // Time in seconds to open the gate
    public float closeDelay = 1f;
    public float openAngle1 = 0f;
    public float openAngle2 = 180f;
    public float closedAngle = 90f;
    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion leftOpenRotation;
    private Quaternion rightOpenRotation;

    void Start()
    {
        // Set the initial and final rotation angles
        closedRotation = Quaternion.Euler(0, closedAngle, 0);
        leftOpenRotation = Quaternion.Euler(0, openAngle1, 0);
        rightOpenRotation = Quaternion.Euler(0, openAngle2, 0);
        transform.localRotation = closedRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player triggered the gate
        if (other.CompareTag("player") && !isOpen)
        {
            StartCoroutine(OpenGate(other.transform));
        }
    }

    private IEnumerator OpenGate(Transform player)
    {
        isOpen = true;

        // Calculate the direction from the gate to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Determine open direction
        Quaternion targetRotation;

        // Use the cross product to determine if the player is to the left or right of the gate
        Vector3 crossProduct = Vector3.Cross(transform.forward, directionToPlayer);

        // If cross product is positive, player is on the left side, else player is on the right side
        if (crossProduct.y > 0)
        {
            targetRotation = leftOpenRotation;
        }
        else
        {
            targetRotation = rightOpenRotation;
        }


        float elapsedTime = 0f;
        while (elapsedTime < openDuration)
        {
            transform.localRotation = Quaternion.Slerp(closedRotation, targetRotation, elapsedTime / openDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // make sure gate is open
        transform.localRotation = targetRotation;
        yield return new WaitForSeconds(closeDelay);

        // start closing the gate
        StartCoroutine(CloseGate());
    }

    private IEnumerator CloseGate()
    {

        float elapsedTime = 0f;


        while (elapsedTime < openDuration)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, closedRotation, elapsedTime / openDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = closedRotation;
        isOpen = false;

    }
}
