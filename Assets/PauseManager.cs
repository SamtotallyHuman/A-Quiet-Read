using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool paused = false;

	void Start()
	{
		Resume();
	}

	void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			if (paused)
				Resume();
			else
				Pause();
		}
    }

    void Pause()
    {
        paused = true;
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Resume()
    {
        paused = false;
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}