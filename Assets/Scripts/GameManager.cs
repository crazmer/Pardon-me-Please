using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject powerupPrefab;

    private float xBound = 13;
    private float zBound = 13;
    private float zSpawnPos = 19;

    private int enemyCount;
    public int waveCount = 1;
    private int startSpawningPowerup = 5;


    public static int score;

    public GameObject PauseMenu;
    
    private TextMeshProUGUI scoreText;
   
    


    public bool isGameOver = false;
    public bool isGameOn = false;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        
        scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();    
        


        InitiateScreen();
        isGameOn = true;
        StartGame();
    }


    public void StartGame()
    {
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGameOn)
        {
            SpawnManager();

            if (isGameOver)
            {

                ClearScreenAtEnd();

            }

        }
        Pause();

    }



    public void SpawnManager()
    {

        enemyCount = FindObjectsOfType<EnemyBehavior>().Length;

        if (enemyCount == 0 && !isGameOver)
        {
            if (waveCount >= startSpawningPowerup)
            {
                SpwanPowerupPrefabWave();
            }

            for (int i = 0; i < waveCount; i++)
            {
                SpawnEnemyPrefabsWave();
            }
            waveCount++;
        }


    }


    void SpawnEnemyPrefabsWave()
    {
        int minIndex = 0;
        int maxIndex = 6;
        int generateIndex = GenerateIndex(minIndex, maxIndex);

        if (generateIndex >= (maxIndex / 2))
        {
            Instantiate(enemyPrefabs[generateIndex], XGenerateRandomPos(), enemyPrefabs[generateIndex].transform.rotation);
        }

        if (generateIndex < (maxIndex / 2))
        {
            Instantiate(enemyPrefabs[generateIndex], negXGenerateRandomPos(), enemyPrefabs[generateIndex].transform.rotation);
        }

    }

    void SpwanPowerupPrefabWave()
    {
        Instantiate(powerupPrefab, GenerateRandomPos(), powerupPrefab.transform.rotation);
    }

    int GenerateIndex(int minIndex, int maxIndex)
    {
        return Random.Range(minIndex, maxIndex);
    }

    float XGeneratePos()
    {
        return Random.Range(-xBound, xBound);
    }

    float ZGeneratePos()
    {
        return Random.Range(-zBound, zBound);
    }

    Vector3 XGenerateRandomPos()
    {
        return new Vector3(XGeneratePos(), 0.5000001f, zSpawnPos);
    }

    Vector3 negXGenerateRandomPos()
    {
        return new Vector3(XGeneratePos(), 0.5000001f, -zSpawnPos);
    }

    Vector3 GenerateRandomPos()
    {
        return new Vector3(XGeneratePos(), 0.5000001f, ZGeneratePos());
    }


    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;

        scoreText.text = "Score: " + score;
    }


    private void InitiateScreen()
    {
        for (int i = 0; i < 6; i++)
        {

            enemyPrefabs[i].SetActive(true);

        }


        
    }

    private void ClearScreenAtEnd()
    {

        StartCoroutine(ClearAfterSeconds());

    }

    IEnumerator ClearAfterSeconds()
    {
        yield return new WaitForSeconds(2.4f);
        Destroy(GameObject.FindWithTag("Enemy"));
        Destroy(GameObject.FindWithTag("Powerup"));
        scoreText.text = "";
       

        yield return new WaitForSeconds(2.5f);
       

        yield return new WaitForSeconds(1);
        

    }




    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if(Time.timeScale == 1.0f)
            {
                Debug.Log("Pause");
                PauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
            }
             if(Time.timeScale == 0.0f)
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
            }
            
        }
    }

}