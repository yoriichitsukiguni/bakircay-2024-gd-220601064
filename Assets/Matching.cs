using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class MatchManager : MonoBehaviour
{
    public RandomSpawner spawner;  // RandomSpawner'� referans olarak ekleyin
    private int totalObjects;  // E�le�en nesnelerin toplam say�s�
    private int matchedObjects = 0;  // E�le�en nesnelerin say�s� 
    public float startTime = 30f;
    public float currentTime;
    public bool isTimerRunning = false;
    private bool isGameOver = false;
    private bool canMatch = false;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;  // Oyun Bitti yaz�s�
    public TextMeshProUGUI restartText;

    void Start()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<RandomSpawner>();  // E�er spawner referans� atanmad�ysa, sahnede RandomSpawner'� bul
        }
        isTimerRunning = true;
        currentTime = startTime;
        gameOverText.gameObject.SetActive(false);  
        restartText.gameObject.SetActive(false);
        // Nesnelerin say�s�n� set et
        spawner.SetTotalObjects(spawner.objectsToSpawn.Length * 2); // ��nk� her nesneden 2 tane olacak
    }

    public void SpawnNewObjects()
    {
        scoreText.text = "Score: 0";
        if(!isGameOver)
        {
            spawner.SpawnObjects();  // RandomSpawner �zerinden yeni nesneleri spawn et
            ResetTimer();
        }
  
    }

    // Ba�lang��ta e�le�mesi gereken nesneleri say
    public void SetTotalObjects(int total)
    {
        totalObjects = total;  // Total nesne say�s�n� ayarla
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
            timerText.text ="Time: " + Mathf.Floor(timeToDisplay).ToString();  // S�reyi ekranda g�ster
        }
    }

    // Zaman bitti�inde yap�lacak i�lemler

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
        gameOverText.gameObject.SetActive(true);  // Oyun bitti yaz�s�n� g�ster
        restartText.gameObject.SetActive(true);  // Yeniden ba�lat yaz�s�n� g�ster
        gameOverText.text = "Game Over! "+"Score :"+ scoreText.text;
        DeleteAllObjects();
    }
    void DeleteAllObjects()
    {
        // Sahnedeki t�m nesneleri sil
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Matchable");
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }
    void OnGUI()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))  // 'R' tu�una bas�ld���nda yeniden ba�lat
        {
            scoreText.text = "Score: 0";
            RestartGame();
        }
    }
    void RestartGame()
    {
        isGameOver = false;
        canMatch = true;  // E�le�me i�lemleri tekrar aktif
        gameOverText.gameObject.SetActive(false);  // Oyun bitti yaz�s�n� gizle
        restartText.gameObject.SetActive(false);  // Yeniden ba�lat yaz�s�n� gizle
        currentTime = startTime;  // S�reyi yeniden ba�lat
        isTimerRunning = true;

      
        // Yeni nesneleri spawn et
        spawner.SpawnObjects();
    }

}
