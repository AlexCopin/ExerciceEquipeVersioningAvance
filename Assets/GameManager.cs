using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (pauseMenuPanel.activeInHierarchy)
                pauseMenuPanel.SetActive(false);
            else
            {
                pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
            }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuPanel.SetActive(false);
    }

    public void OpenOptionsMenu()
    {
        optionsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    public void CloseOptionsMenu()
    {
        optionsMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void QuitGame() => Application.Quit();
}
