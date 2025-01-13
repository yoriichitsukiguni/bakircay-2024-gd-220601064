using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class MatchManager : MonoBehaviour
{
    public RandomSpawner spawner;  // RandomSpawner'ý referans olarak ekleyin
    private int totalObjects;  // Eþleþen nesnelerin toplam sayýsý
    private int matchedObjects = 0;  // Eþleþen nesnelerin sayýsý 
    public float startTime = 30f;
    public float currentTime;
    public bool isTimerRunning = false;
    private bool isGameOver = false;
    private bool canMatch = false;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;  // Oyun Bitti yazýsý
    public TextMeshProUGUI restartText;

    void Start()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<RandomSpawner>();  // Eðer spawner referansý atanmadýysa, sahnede RandomSpawner'ý bul
        }
        isTimerRunning = true;
        currentTime = startTime;
        gameOverText.gameObject.SetActive(false);  
        restartText.gameObject.SetActive(false);
        // Nesnelerin sayýsýný set et
        spawner.SetTotalObjects(spawner.objectsToSpawn.Length * 2); // Çünkü her nesneden 2 tane olacak
    }

    public void SpawnNewObjects()
    {
        scoreText.text = "Score: 0";
        if(!isGameOver)
        {
            spawner.SpawnObjects();  // RandomSpawner üzerinden yeni nesneleri spawn et
            ResetTimer();
        }
  
    }

    // Baþlangýçta eþleþmesi gereken nesneleri say
    public void SetTotalObjects(int total)
    {
        totalObjects = total;  // Total nesne sayýsýný ayarla
    }

    // Update fonksiyonu
    void Update()
    {
        if (isTimerRunning && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimeDisplay(currentTime);
        }
        else if (currentTime <= 0 && isTimerRunning && !isGameOver)
        {
            GameOver();
            scoreText.text = "Score: 0";
        }
        if(!isGameOver)
        {
            GameObject[] matchableObjects = GameObject.FindGameObjectsWithTag("Matchable");
            if (matchableObjects.Length == 0)
            {
                if (spawner != null)
                {
                    ResetTimer();
                    UpdateTimeDisplay(startTime);
                    spawner.SpawnObjects();
                }
                else
                {
                }
            }
        }
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            scoreText.text = "Score: 0";
            RestartGame();

        }

    }

    public void UpdateTimeDisplay(float timeToDisplay)
    {
        if (timerText != null)
        {
            timerText.text ="Time: " + Mathf.Floor(timeToDisplay).ToString();  // Süreyi ekranda göster
        }
    }

    // Zaman bittiðinde yapýlacak iþlemler

    public void ResetTimer()
    {
        currentTime = startTime;
        isTimerRunning = true;
        UpdateTimeDisplay(currentTime);
        Debug.Log("Timer Reset to: " + startTime);
    }
    void GameOver()
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);  // Oyun bitti yazýsýný göster
        restartText.gameObject.SetActive(true);  // Yeniden baþlat yazýsýný göster
        gameOverText.text = "Game Over! "+"Score :"+ scoreText.text;
        DeleteAllObjects();
    }
    void DeleteAllObjects()
    {
        // Sahnedeki tüm nesneleri sil
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Matchable");
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }
    void OnGUI()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))  // 'R' tuþuna basýldýðýnda yeniden baþlat
        {
            scoreText.text = "Score: 0";
            RestartGame();
        }
    }
    void RestartGame()
    {
        isGameOver = false;
        canMatch = true;  // Eþleþme iþlemleri tekrar aktif
        gameOverText.gameObject.SetActive(false);  // Oyun bitti yazýsýný gizle
        restartText.gameObject.SetActive(false);  // Yeniden baþlat yazýsýný gizle
        currentTime = startTime;  // Süreyi yeniden baþlat
        isTimerRunning = true;

      
        // Yeni nesneleri spawn et
        spawner.SpawnObjects();
    }

}
