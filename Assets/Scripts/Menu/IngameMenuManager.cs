using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class IngameMenuManager : MonoBehaviour
    {
        private bool _paused = false;
        public GameObject _pauseMenu;
        private Transform _player;

        public Slider _playerSlider;
        public Slider _enemySlider;

        private void Start()
        {
            _player = FindObjectOfType<Player.PlayerController>().transform;
        }

        // Update is called once per frame
        void Update()
        {
            _pauseMenu.SetActive(_paused);
            if (_paused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1.0f;

            _playerSlider.value = CheckpointManager.Instance.GetCompletionPercentage(_player.position, CheckpointManager.Instance._currentCheckpointIndex);
            _enemySlider.value = 0.0f;

        }

        public void TogglePauseMenu(InputAction.CallbackContext context)
        {
            if (context.action.triggered)
            {
                _paused = !_paused;
            }
        }

        public void ExitButton()
        {
            SceneManager.LoadScene(0);
        }
    }

}