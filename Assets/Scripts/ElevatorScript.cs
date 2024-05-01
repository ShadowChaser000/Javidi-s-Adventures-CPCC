using UnityEngine;
using System.Collections;

public class ElevatorPlatform : MonoBehaviour
{
    public GameObject startingPositionObject; // Reference to the GameObject representing the starting position
    public GameObject targetPositionObject; // Reference to the GameObject representing the target position
    public float travelSpeed = 2f; // Speed at which the elevator will travel
    public float delayBeforeMoving = 1f; // Delay before the elevator starts moving after player enters collider
    public float delayBeforeReturning = 2f; // Delay before the elevator returns to its starting position after reaching target

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private bool playerInsideCollider = false;
    private bool moving = false;
    private float startTime;

    void Start()
    {
        startingPosition = startingPositionObject.transform.position;
        targetPosition = targetPositionObject.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
            StartCoroutine(StartMoving());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }

    IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(delayBeforeMoving);
        startTime = Time.time;
        moving = true;
    }

    void Update()
    {
        if (moving)
        {
            float distanceCovered = (Time.time - startTime) * travelSpeed;
            float fractionOfDistance = distanceCovered / Vector3.Distance(startingPosition, targetPosition);
            transform.position = Vector3.Lerp(startingPosition, targetPosition, fractionOfDistance);

            if (fractionOfDistance >= 1f)
            {
                StartCoroutine(ReturnToStartingPosition());
            }
        }
    }

    IEnumerator ReturnToStartingPosition()
    {
        yield return new WaitForSeconds(delayBeforeReturning);
        startTime = Time.time;
        float distanceToStartingPosition = Vector3.Distance(transform.position, startingPosition);
        float timeToReturn = distanceToStartingPosition / travelSpeed;
        while (transform.position != startingPosition)
        {
            float elapsedTime = (Time.time - startTime) / timeToReturn;
            transform.position = Vector3.Lerp(transform.position, startingPosition, elapsedTime);
            yield return null;
        }
        moving = false;
    }
}
