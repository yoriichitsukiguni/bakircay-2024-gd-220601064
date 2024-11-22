using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private GameObject firstObject = null;  // Ýlk giren nesne
    private GameObject secondObject = null;  // Ýkinci giren nesne
    public float throwForce;  
    public Vector3 throwDirection;  

    // Bu fonksiyon, bir nesne alana girdiðinde çalýþýr
    private void OnTriggerEnter(Collider other)
    {
        // Eðer birinci nesne alana girdiyse kaydet
        if (firstObject == null)
        {
            firstObject = other.gameObject;
        }
        // Eðer ikinci nesne alana girdiyse, birinci nesne alanda ise, onu fýrlat
        else if (secondObject == null && other.gameObject != firstObject && firstObject != null)
        {
            secondObject = other.gameObject;
            ThrowSecondObject();
        }
    }

    // Ýkinci nesneyi fýrlatma iþlemi
    private void ThrowSecondObject()
    {
        if (secondObject != null)
        {
            Rigidbody rb = secondObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);  // Kuvvet uygula
            }
        }

        // Nesneyi fýrlattýktan sonra sýfýrla
        secondObject = null;
    }
}
