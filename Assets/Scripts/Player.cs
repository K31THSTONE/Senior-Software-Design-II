using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {


    private bool _isDead = false;
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
        wasEnabled = new bool[disableOnDead.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDead[i].enabled;
        }
        SetDefaults();
    }

    void Update()
    {
        if (isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
        
    }

    [ClientRpc]
    public void RpcTakeDamage (int _amount)
    {
        if (isDead)
        {
            return;
        }
        currentHp -= _amount;

        Debug.Log(transform.name + " has " + currentHp + " health");

        if (currentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;

        //disable components
        for (int i = 0; i < disableOnDead.Length; i++)
        {
            disableOnDead[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Debug.Log(transform.name + " is dead");

        //call respawning
    }

    public void SetDefaults ()
    {

        currentHp = maxHp;
        isDead = false;

        for (int i = 0; i < disableOnDead.Length; i++)
        {
            disableOnDead[i].enabled = wasEnabled[i];
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }

}

