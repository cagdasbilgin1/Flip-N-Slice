using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance;

    //private void Awake()
    //{
    //    if(Instance == null)
    //    {
    //        Instance = this;
    //    }
    //}

    [SerializeField] TMP_Text tapToStartText;
    [SerializeField] ParticleSystem winFx;
    public TMP_Text money;
    public GameObject gameOverPanel;
    [SerializeField] private AudioSource audioSource;

    public bool isGameStarted;

    void Start()
    {
        isGameStarted = false;
        money.text = "$" + Game.money.ToString();
    }

    void Update()
    {
        if (isGameStarted)
        {
            tapToStartText.enabled = false;
        }
    }

    public void GameOver()
    {

        gameOverPanel.SetActive(true);
        audioSource.Stop();
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        if(SceneManager.GetActiveScene().buildIndex > 0)
        {
            Game.money = PlayerPrefs.GetInt(GetPreviousLevel());
        }
        else
        {
            Game.money = 0;
        }

        money.text = "$" + Game.money.ToString();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        audioSource.Play();
    }

    public void Win()
    {
        winFx.Play();        
    }

    public void NextLevel()
    {
        if(SceneManager.sceneCountInBuildSettings - 1 > SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public string GetCurrentLevel()
    {
        return "level" + SceneManager.GetActiveScene().buildIndex;
    }

    public string GetPreviousLevel()
    {
        return "level" + (SceneManager.GetActiveScene().buildIndex - 1);
    }
}
