using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashController : MonoBehaviour
{
    public static int sceneNumber;
    void Start()
    {
        //use coroutines to load spash scenes with a wait time
        if(sceneNumber == 0)
        {
            StartCoroutine(ToSplash2());
        }
        if (sceneNumber == 1)
        {
            StartCoroutine(ToMain());

        }
    }
    IEnumerator ToSplash2()
    {
        yield return new WaitForSeconds(5);
        sceneNumber = 1;
        SceneManager.LoadScene("Splash2");
    }
    IEnumerator ToMain()
    {
        yield return new WaitForSeconds(5);
        sceneNumber = 2;
        SceneManager.LoadScene("MainMenu");
    }
}
