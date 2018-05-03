using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    RectTransform thrusterFuelFill;

    private PlayerController controller;

    public void SetController (PlayerController _controller)
    {
        controller = _controller;
    }
    //will set the fuel dynamically
    void SetFuelAmount (float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void Start()
    {
        PauseMenu.IsOn = false;
    }
    void Update()
    {
        SetFuelAmount (controller.GetThrusterFuelAmount());

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
