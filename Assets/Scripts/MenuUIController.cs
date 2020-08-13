using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    public PlayerController player;
    public GameController gameController;
    public GameObject mainCanvas;
    public GameObject creditsPanel;
    // Start is called before the first frame update
    void Start()
    {
        mainCanvas = GameObject.Find("MainCanvas");
        creditsPanel = mainCanvas.transform.Find("CreditsPanel").gameObject;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    //disable credits panel
    private void Awake()
    {
        creditsPanel.SetActive(false);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = (false);
#else
        Application.Quit();
#endif
    }

    public void NewGame()
    {
        if(player != null)
        {
            Destroy(player.gameObject);//destroy player to start fresh game
        }
        SceneManager.LoadScene("TutorialScene");
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        if (player != null)//check for player object, can't resume if a previous run hadn't created a player object
        {
            gameController.ProgressLevel(player.levelProgress);
        }
    }

    public void ReturnToMain()
    {
        creditsPanel.SetActive(false);
    }
    public void ToMainFromEnd()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
