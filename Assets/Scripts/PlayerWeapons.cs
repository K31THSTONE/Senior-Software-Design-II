using UnityEngine;
[System.Serializable]


public class PlayerWeapons  {


    public string name;
    public int damage;
    public float range;
    //public int damage = 10;
    //public float range = 75f;


}

public class Pistol : PlayerWeapons
{
    public PlayerWeapons create()
    {
        PlayerWeapons pistol = new PlayerWeapons();
        pistol.damage = 10;
        pistol.range = 75f;
        pistol.name = "Pistol";
        return pistol;
    }
}

public class Submachine : PlayerWeapons
{
    public PlayerWeapons create()
    {
        PlayerWeapons submachine = new PlayerWeapons();
        submachine.damage = 5;
        submachine.range = 55f;
        submachine.name = "Sub Machine Gun";
        return submachine;
    }
}

public class Sniper : PlayerWeapons
{
    public PlayerWeapons create()
    {
        PlayerWeapons sniper = new PlayerWeapons();
        sniper.damage = 100;
        sniper.range = 750f;
        sniper.name = "Sniper";
        return sniper;
    }
}

public class Shotgun : PlayerWeapons
{
    public PlayerWeapons create()
    {
        PlayerWeapons shotgun = new PlayerWeapons();
        shotgun.damage = 60;
        shotgun.range = 35f;
        shotgun.name = "Sawed Off Shotgun";
        return shotgun;
    }
}