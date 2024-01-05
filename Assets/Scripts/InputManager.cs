using UnityEngine.InputSystem;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    private PlayerInputControls inputControls;
    public static event Action ReloadLevel;
    public static event Action ToggleShowGameMenu;

    private void Awake()
    {
        inputControls = new PlayerInputControls();
        inputControls.Enable();
    }
    private void OnDisable()
    {
        inputControls.Disable();
    }

    private bool menuOn = false;

    void Start()
    {

        inputControls.PlayerActionMap.ReloadLevel.performed += ctx =>
        {
            if (menuOn) {
                ToggleShowGameMenu?.Invoke();
                TogglePause();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        };

        inputControls.PlayerActionMap.OpenGameMenu.performed += ctx =>
        {
            menuOn = !menuOn;
            ToggleShowGameMenu?.Invoke();
            TogglePause();
        };

        inputControls.PlayerActionMap.Pause.performed += ctx =>
        {
            TogglePause();
        };
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ContinueGame();
        }
        else
        {
            Pause();
        }
    }

    private bool isPaused = false;

    private void ContinueGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

}
