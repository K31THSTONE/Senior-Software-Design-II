using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class Player : NetworkBehaviour { 

    private bool _isDead = false;
    public bool lost = false;

    [SyncVar]
	public int deaths = 0;
    public int kills = 0;

    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHp = 100;

    [SerializeField]
    private Behaviour[] disableOnDead;
    private bool[] wasEnabled;

    //allows variable update to be pushed to all clients in use
    [SyncVar]
    private int currentHp;

    public void Setup()
    {

        this.CmdJoinNewPlayerSetup();
    }

    private void CmdJoinNewPlayerSetup()
    {
        this.RpcSetupPlayerOnAllInstances();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllInstances()
    {
        wasEnabled = new bool[disableOnDead.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDead[i].enabled;
        }
        SetDefaults();
    }
    /*
    void Update()
    {
        if (isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
        
    }
    */
    [ClientRpc]
    public void RpcTakeDamage (int _amount)
    {
        if (isDead)
        {
            return;
        }
        this.currentHp -= _amount;

        Debug.Log(transform.name + " has " + this.currentHp + " health");

        if (currentHp <= 0)
        {
            this.deaths++;
            if (deaths == 1)
            {
                this.lost = true;
                this.isDead = true;
                this.Die();
            }
            else
            {
                this.Die();
            }
        }
    }
    private void Die()
    {
        this.isDead = true;

        //disable components
        for (int i = 0; i < disableOnDead.Length; i++)
        {
            this.disableOnDead[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Debug.Log(transform.name + " is dead");

        //call respawning
        StartCoroutine(Respawn(this));
    }

    //coroutine basically for respawning the player
    private IEnumerator Respawn (Player player)
    {
        //directs to the variable within the round setting script
        yield return new WaitForSeconds(GameManager.gameInstance.roundSettings.respawnTimer);

        player.SetDefaults();
        //will return one of the fixed spawn/start points in our game
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " respawned.");
    }

    public void SetDefaults ()
    {

        this.currentHp = maxHp;
        this.isDead = false;
        //enable components
        for (int i = 0; i < disableOnDead.Length; i++)
        {
            this.disableOnDead[i].enabled = this.wasEnabled[i];
        }

        //enable gameobjects
        //enables collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }
    }

    public void EndGame()
    {
        NetworkManager networkManager = NetworkManager.singleton;
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }

}

