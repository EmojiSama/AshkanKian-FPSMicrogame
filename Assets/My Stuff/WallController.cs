using System.Collections;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public int requiredHits = 3; // Number of hits required to move the wall
    public float moveDistance = 10f; // Distance to move the wall down
    public float waitTime = 5f; // Time to wait before raising the wall back up
    public float moveSpeed = 1f; // Speed at which the wall moves down and up

    private int currentHits = 0; // Tracks how many hits have occurred
    private Vector3 originalPosition; // Store the original position of the wall
    private Rigidbody rb; // Rigidbody component

    private void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile_Turret"))
        {
            currentHits++;

            if (currentHits >= requiredHits)
            {
                StartCoroutine(MoveWall(originalPosition + Vector3.down * moveDistance));
            }
        }
    }

    private IEnumerator MoveWall(Vector3 targetPosition)
    {
        float elapsedTime = 0;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure the position is set precisely at the end of the movement

        if (targetPosition != originalPosition) // Check if it's not the original position
        {
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(MoveWall(originalPosition)); // Move the wall back to the original position
        }
        else
        {
            currentHits = 0; // Reset hits only when the wall returns to the original position
        }
    }
}
