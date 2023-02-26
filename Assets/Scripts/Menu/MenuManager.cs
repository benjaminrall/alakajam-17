using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public RectTransform quitButton;

        void Start()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                quitButton.gameObject.SetActive(false);
            }
        }

        public void PlayButton()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitButton()
        {
            Application.Quit();
        }
    }
}
