using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] disabledComponents;

    Camera sceneCamera;

    //disabling components to allow proper networking between players and objects
    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < disabledComponents.Length; i++)
            {
                disabledComponents[i].enabled = false;
            }
        } else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }

    }

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }


}
