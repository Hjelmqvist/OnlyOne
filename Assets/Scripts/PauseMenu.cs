using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Singleton<PauseMenu>
{
    [SerializeField] GameObject pausePanel = null;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (pausePanel.activeInHierarchy)
                Resume();
            else
                Pause();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
