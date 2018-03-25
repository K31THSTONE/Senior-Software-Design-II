using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] disabledComponents;
    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string dontDrawLayer = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;
    [SerializeField]
    GameObject playerUI;
    private GameObject playerUIInstance;

    Camera sceneCamera;

    //disabling components to allow proper networking between players and objects
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            //get rid of local player gui
            SetRecursiveLayer(playerGraphics, LayerMask.NameToLayer(dontDrawLayer));
            //create playerUI
            playerUIInstance = Instantiate(playerUI);
            playerUIInstance.name = playerUI.name;
        }

        GetComponent<Player>().Setup();

    }

    void SetRecursiveLayer(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            //goes through each child layer down heirarchy
            SetRecursiveLayer(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
    }


    void AssignRemoteLayer ()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents ()
    {
        for (int i = 0; i < disabledComponents.Length; i++)
        {
            disabledComponents[i].enabled = false;
        }
    }


    void OnDisable()
    {

        Destroy(playerUIInstance);
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }


}
