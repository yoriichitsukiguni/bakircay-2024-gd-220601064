using UnityEngine;
using System.Collections;
using TMPro;

public class AreaTrigger : MonoBehaviour
{
    private GameObject firstObject = null;
    private GameObject secondObject = null;
    public float throwForce;
    public Vector3 throwDirection;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public ParticleSystem matchEffect;

    private void Start()
    {
        score = 0;
        updateScoreUI();
    }

    private void Update()
    {
        updateScoreUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (firstObject == null)
        {
            firstObject = other.gameObject;
        }
        else if (secondObject == null && other.gameObject != firstObject)
        {
            secondObject = other.gameObject;
            HandleMatch();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == firstObject)
        {
            firstObject = null;
        }
    }

    private void HandleMatch()
    {
        if (firstObject != null && secondObject != null)
        {
            if (firstObject.name == secondObject.name)
            {
                StartCoroutine(MergeAndDestroy(firstObject, secondObject));
                score += 10;
            }
            else
            {
                ThrowSecondObject();
            }
        }
    }

    private IEnumerator MergeAndDestroy(GameObject obj1, GameObject obj2)
    {
        Vector3 midPoint = (obj1.transform.position + obj2.transform.position) / 2;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            obj1.transform.position = Vector3.Lerp(obj1.transform.position, midPoint, elapsed / duration);
            obj2.transform.position = Vector3.Lerp(obj2.transform.position, midPoint, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Instantiate(matchEffect, midPoint, Quaternion.identity);
        Destroy(obj1);
        Destroy(obj2);
    }

    private void ThrowSecondObject()
    {
        if (secondObject != null)
        {
            Rigidbody rb = secondObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            }
        }
        ResetSecondObject();
    }

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
