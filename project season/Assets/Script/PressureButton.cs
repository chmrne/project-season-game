using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public Transform doorTransform; 
    public float doorSlideDistance = 3f; 
    public float doorSlideSpeed = 5f; 

    public Transform buttonVisual; 
    public float buttonPressDistance = 0.2f; 

    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    Vector3 buttonUpPos;
    Vector3 buttonDownPos;

    int objectsOnButton = 0;

    void Start()
    {
        if (doorTransform != null)
        {
            doorClosedPos = doorTransform.position;
            doorOpenPos = doorClosedPos + new Vector3(0, doorSlideDistance, 0);
        }

        if (buttonVisual != null)
        {
            buttonUpPos = buttonVisual.position;
            buttonDownPos = buttonUpPos - new Vector3(0, buttonPressDistance, 0);
        }
    }

    void Update()
    {
        if (doorTransform == null) return;

        Vector3 targetDoorPos = objectsOnButton > 0 ? doorOpenPos : doorClosedPos;
        doorTransform.position = Vector3.MoveTowards(doorTransform.position, targetDoorPos, doorSlideSpeed * Time.deltaTime);

        if (buttonVisual != null)
        {
            Vector3 targetButtonPos = objectsOnButton > 0 ? buttonDownPos : buttonUpPos;
            buttonVisual.position = Vector3.MoveTowards(buttonVisual.position, targetButtonPos, doorSlideSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            objectsOnButton++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            objectsOnButton--;
            if (objectsOnButton < 0) objectsOnButton = 0; 
        }
    }
}