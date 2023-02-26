using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    public GameObject _playerWon;
    public GameObject _playerLost;

    public GameObject _playerWonText;
    public GameObject _playerLostText;

    private void Start()
    {
        if (FinishFlag._playerWon)
        {
            _playerWon.SetActive(true);
            _playerWonText.SetActive(true);
        }
        else
        {
            _playerLost.SetActive(true);
            _playerLostText.SetActive(true);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
