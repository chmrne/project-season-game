using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorTransform; // Drag the Door GameObject here in the inspector
    public float doorSlideDistance = 3f; // How high the door goes
    public float doorSlideSpeed = 2f; // How fast the door opens

    private bool isActivated = false;

    public void ActivateLever()
    {
        // Prevent clicking the lever multiple times
        if (!isActivated)
        {
            isActivated = true;
            
            // Rotate the lever 90 degrees on the Z axis
            transform.Rotate(0f, 0f, -90f); 

            // Start smoothly sliding the door up
            if (doorTransform != null)
            {
                StartCoroutine(SlideDoorUp());
            }
            else
            {
                Debug.LogWarning("Lever activated, but no Door Transform is assigned!");
            }
        }
    }

    private IEnumerator SlideDoorUp()
    {
        Vector3 startPos = doorTransform.position;
        Vector3 targetPos = startPos + new Vector3(0, doorSlideDistance, 0);

        // Move the door towards the target position smoothly over time
        while (Vector3.Distance(doorTransform.position, targetPos) > 0.01f)
        {
            doorTransform.position = Vector3.MoveTowards(doorTransform.position, targetPos, doorSlideSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Snap to exact final position just in case
        doorTransform.position = targetPos;
    }
}