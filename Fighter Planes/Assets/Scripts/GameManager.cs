using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject cloudPrefab;
    public GameObject coinPrefab;
    public GameObject powerUpPrefab;

    public PlayerController playerController;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI powerupText;
    public GameObject gameOverText;
    public GameObject restartText;

    public float horizontalScreenSize;
    public float verticalScreenSize;

    public int score;
    public int cloudMove;

    private bool gameOver;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        horizontalScreenSize = 10f;
        verticalScreenSize = 6.5f;
        score = 0;
        cloudMove = 1;
        gameOver = false;
        CreateSky();
        InvokeRepeating("CreateEnemy", 1, 3);
        InvokeRepeating("CreateCoin", 5, 5);
        StartCoroutine(SpawnPowerUp ());
        powerupText.text = "No powerups yet!";

}

    // Update is called once per frame
    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } 
    }

    void CreateEnemy()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), verticalScreenSize, 0) * 0.9f, Quaternion.Euler(180, 0, 0));
    }

    void CreatePowerUp()
    {
        Instantiate(powerUpPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.8f, horizontalScreenSize * 0.8f), Random.Range(-verticalScreenSize * 0.8f, verticalScreenSize * 0.8f), 0), Quaternion.identity);
    }

    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(cloudPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0), Quaternion.identity);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        float spawnTime = Random.Range(3, 5);
        yield return new WaitForSeconds(spawnTime);
        CreatePowerUp();
        StartCoroutine(SpawnPowerUp());
    }


    public void ManagePowerupText(int powerupType)
    {
        switch (powerupType)
        {
            case 1:
                powerupText.text = "Speed Powerup!";
                break;
            case 2:
                powerupText.text = "Double Weapon Powerup!";
                break;
            case 3:
                powerupText.text = "Triple Weapon Powerup!";
                break;
            case 4:
                powerupText.text = "Shield Powerup";
                break;
            default:
                powerupText.text = "No powerups yet!";
                break;
        }
    }

    public void AddScore(int earnedScore)
    {
        score = score + earnedScore;
        ChangeScoreText(score);

    }

    public void ChangeLivesText (int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }

    public void ChangeScoreText (int score)
    {
        scoreText.text = "Score: " + score;
    }

    void CreateCoin()
    {
        Instantiate(coinPrefab, new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0), Quaternion.identity);
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        restartText.SetActive(true);
        gameOver = true;
        CancelInvoke();
        StopAllCoroutines();
        cloudMove = 0;
    }
}
