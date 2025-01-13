using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;  // Spawn edilecek nesneler
    public int howManyObjects;  // Kaç tane nesne spawn edilecek

    void Start()
    {
        SpawnObjects();  // Ýlk nesneleri spawn et
    }

    // Yeni nesneleri spawn etme fonksiyonu
    public void SpawnObjects()
    {

        if (objectsToSpawn == null || objectsToSpawn.Length == 0)
        {
            return; // Eðer spawn edilecek nesneler yoksa iþlemi durdur
        }

        // Önceki nesneleri yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Nesne türlerinden ikiþer tane oluþturulacak
        int numObjects = objectsToSpawn.Length;


        // Nesneleri spawn et
        for (int i = 0; i < numObjects; i++)
        {
            Vector3 position = new Vector3(Random.Range(-2, 3), 2f, Random.Range(-3, 3));
            Instantiate(objectsToSpawn[i], position, Quaternion.identity);

            // Ayný nesneyi bir kez daha spawn et
            Vector3 secondPosition = new Vector3(Random.Range(-2, 3), 2f, Random.Range(-3, 3));
            Instantiate(objectsToSpawn[i], secondPosition, Quaternion.identity);
        }

    }
    
    // Nesnelerin toplam sayýsýný ayarlayan fonksiyon
    public void SetTotalObjects(int total)
    {
        howManyObjects = total;  // Nesne sayýsýný ayarla
    }
}