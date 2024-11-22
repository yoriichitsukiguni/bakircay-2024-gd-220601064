using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;  // Spawn edilecek nesneler
    public int howManyObjects;  // Kaç tane nesne spawn edilecek

    void Start()
    {
        SpawnObjects();  // Baþlatýnca nesneleri yerleþtir
    }

    void SpawnObjects()
    {
        for (int i = 0; i < howManyObjects; i++)
        {
            // Rastgele bir yer seç
            Vector3 position = new Vector3(Random.Range(-2, 3), 2f, Random.Range(-3, 3));
            // Rastgele bir nesne seç
            int randomIndex = Random.Range(0, objectsToSpawn.Length);
            // O nesneyi yarat
            Instantiate(objectsToSpawn[randomIndex], position, Quaternion.identity);
        }
    }
}
