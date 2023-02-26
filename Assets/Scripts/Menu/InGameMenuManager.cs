using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Player;

namespace Menu
{
    public class InGameMenuManager : MonoBehaviour
    {
        public GameObject pauseMenu;
        
        private bool _paused = false;

        private Transform _player;

        public Slider playerSlider;
        public Slider enemySlider;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>().transform;
        }

        private void Update()
        {
            pauseMenu.SetActive(_paused);
            Time.timeScale = _paused ? 0f : 1f;

            playerSlider.value = CheckpointManager.Instance.GetCompletionPercentage(_player.position,
                CheckpointManager.Instance.CurrentCheckpointIndex);
            enemySlider.value = 0f;
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
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}