using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishFlag : Singleton<FinishFlag> 
{
    public static bool _playerWon = true;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Call()
    {
        StartCoroutine(LoadFinish());
    }

    public static IEnumerator LoadFinish()
    {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(2);
    }
}
