using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    //can change dependent on what size room a person wants to find
    [SerializeField]
    private uint roomSize = 2;

    private string roomName;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    //allows room name creation
    public void SetName (string _name)
    {
        roomName = _name;
    }

    public void CreateRoom ()
    {
       
        
            Debug.Log("Making room:" + roomName + "with" + roomSize + "players allowed");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        
    }
}
