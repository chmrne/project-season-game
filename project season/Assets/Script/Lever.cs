using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorTransform; 
    public float doorSlideDistance = 3f; 
    public float doorSlideSpeed = 2f; 

    private bool isActivated = false;

    public void ActivateLever()
    {
        
        if (!isActivated)
        {
            isActivated = true;
            
            
            transform.Rotate(0f, 0f, -90f); 

            
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

        
        while (Vector3.Distance(doorTransform.position, targetPos) > 0.01f)
        {
            doorTransform.position = Vector3.MoveTowards(doorTransform.position, targetPos, doorSlideSpeed * Time.deltaTime);
            yield return null; 
        }

        
        doorTransform.position = targetPos;
    }
}