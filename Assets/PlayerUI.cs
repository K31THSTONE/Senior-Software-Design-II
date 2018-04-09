using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;
    private PlayerController controller;

    public void SetController (PlayerController _controller)
    {
        controller = _controller;
    }

    void Start()
    {
        PauseMenu.IsOn = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu ()
    {
        //gets current state, enable/disable pause
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
    }
}
