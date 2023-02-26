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
        
        private bool _paused;

        private PlayerController _player;
        private BotController _bot;

        public Slider playerSlider;
        public Slider enemySlider;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();
            _bot = FindObjectOfType<BotController>();
        }

        private void Update()
        {
            pauseMenu.SetActive(_paused);
            Time.timeScale = _paused ? 0f : 1f;

            playerSlider.value = CheckpointManager.Instance.GetCompletionPercentage(
                _player.transform.position,
                _player.CurrentCheckpointIndex
                );
            enemySlider.value = CheckpointManager.Instance.GetCompletionPercentage(
                _bot.transform.position,
                _bot.CurrentCheckpointIndex
            );;
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
            _paused = false;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }
}