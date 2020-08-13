using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //declare class variables
    public PlayerController player;
    public ShootEnemyController enemy;
    public static GameController instance = null;
    public GameObject pausePanel;
    public bool isPanelActive = false;
    public AudioClip soundEffect1;
    public AudioClip soundEffect2;
    public AudioSource audioSource;
    void Start()
    {
        //initialize class variables and start game music
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect1;
        audioSource.loop = true;
        audioSource.Play();
        pausePanel = GameObject.Find("PausePanel");
        pausePanel.SetActive(isPanelActive);
        SetAudioLevel(0.1f);
    }
    //Singleton implementation so the object maintains state across scenes
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    void Update()
    {
        //checking for input for pause menu
        if (Input.GetKeyDown(KeyCode.P))
        {
            PausePressed();
        }
        //checking to ensure pausePanel and player objects have a reference
        if (pausePanel == null)
        {
            pausePanel = GameObject.Find("PausePanel");
            pausePanel.SetActive(isPanelActive);
        }
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
    }
    //active pause menu panel
    public void PausePressed()
    {
        isPanelActive = !isPanelActive;
        pausePanel.SetActive(isPanelActive);
    }
    public void ToMain()
    {
        PausePressed();
        player.transform.position = new Vector3(-9f, -2.8f, 1f);
        SceneManager.LoadScene("MainMenu");
    }
    //load levels and reset player position
    public void ProgressLevel(int level)
    {
        if (level == 0)
        {
            SceneManager.LoadScene("TutorialScene");
            player.transform.position = new Vector3(-9f, -2.8f, 1f);
        }
        if(level == 1)
        {
            SceneManager.LoadScene("FirstLevel");
            player.transform.position = new Vector3(-9f, -2.8f, 1f);
        }
        if (level == 2)
        {
            SceneManager.LoadScene("SecondLevel");
            player.transform.position = new Vector3(-9f, -2.8f, 1f);
        }
        if (level == 3)
        {
            SceneManager.LoadScene("ThirdLevel");
            player.transform.position = new Vector3(-9f, -2.8f, 1f);
        }
        if (level == 4)
        {
            SceneManager.LoadScene("FourthLevel");
            player.transform.position = new Vector3(-9f, -2.8f, 1f);
        }
        if (level == 5)
        {
            SceneManager.LoadScene("Credits");
            Destroy(player.gameObject);
            audioSource.Stop();
            //stop game audio for credis
        }
    }

    public void SetAudioLevel(float sliderValue)
    {
        audioSource.volume = sliderValue;
    }
}
