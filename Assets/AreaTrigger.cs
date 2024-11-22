using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private GameObject firstObject = null;  // First object that enters
    private GameObject secondObject = null;  // Second object that enters
    public float throwForce;
    public Vector3 throwDirection;

    // This function is called when an object enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // If the first object hasn't been assigned yet, assign it
        if (firstObject == null)
        {
            firstObject = other.gameObject;
        }
        // If the second object enters, and the first object is already in the area, throw the second object
        else if (secondObject == null && other.gameObject != firstObject)
        {
            secondObject = other.gameObject;
            ThrowSecondObject();
        }
    }

    // This function is called when an object exits the trigger area
    private void OnTriggerExit(Collider other)
    {
        // If the first object exits, reset and reassign the first object
        if (other.gameObject == firstObject)
        {
            firstObject = null; // The first object has exited

            // Now we need to set the next object that enters as the new firstObject
            // If the second object is already inside, it should be set as the new firstObject
            // Otherwise, the first object will be null until a new object enters
        }
    }

    // This function is called when the second object is thrown
    private void ThrowSecondObject()
    {
        if (secondObject != null)
        {
            Rigidbody rb = secondObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);  // Apply force to throw
            }
        }

        // After throwing the second object, reset the second object and keep the first object for future use
        ResetSecondObject();
    }

    // Reset second object to allow the process to start again
    private void ResetSecondObject()
    {
        secondObject = null;
    }
}
