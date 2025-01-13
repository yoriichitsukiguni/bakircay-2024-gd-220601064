using UnityEngine;
using System.Collections;
using TMPro;

public class AreaTrigger : MonoBehaviour
{
    private GameObject firstObject = null;  // First object that enters
    private GameObject secondObject = null;  // Second object that enters
    public float throwForce;
    public Vector3 throwDirection;
    public int score = 0;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        score = 0;
        updateScoreUI();
    }
    private void Update()
    {
        updateScoreUI();
    }
    // This function is called when an object enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // If the first object hasn't been assigned yet, assign it
        if (firstObject == null)
        {
            firstObject = other.gameObject;
        }
        // If the second object enters, and the first object is already in the area, check if they match
        else if (secondObject == null && other.gameObject != firstObject)
        {
            secondObject = other.gameObject;
            HandleMatch();  // Check if objects match based on their names
        }
    }

    // This function is called when an object exits the trigger area
    private void OnTriggerExit(Collider other)
    {
        // If the first object exits, reset and reassign the first object
        if (other.gameObject == firstObject)
        {
            firstObject = null; // The first object has exited
        }
    }

    // Handle the matching logic
    private void HandleMatch()
    {
        if (firstObject != null && secondObject != null)
        {
            // Compare the names of both objects
            if (firstObject.name == secondObject.name) // Check if the names are the same
            {
                // If they match, destroy both objects     
                Destroy(firstObject);
                Destroy(secondObject);
                score += 10;
                int count = GameObject.FindGameObjectsWithTag("Matchable").Length;          
   
            }
            else
            {
                // If they don't match, throw the second object and keep the first one
                ThrowSecondObject();
            }
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

        // After throwing the second object, reset the second object to allow for new objects to be placed
        ResetSecondObject();
    }

    // Reset second object to allow the process to start again
    private void ResetSecondObject()
    {
        secondObject = null;
    }
    public void updateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();

        }
    }
 
   
}