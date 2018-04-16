using UnityEngine;
[System.Serializable]


public class PlayerWeapons  {


    public string name;
    public int damage;
    public float range;


}

public class Pistol : PlayerWeapons
{
    public static void initializePistol(PlayerWeapons playerWeapon)
    {
        playerWeapon.damage = 10;
        playerWeapon.range = 75f;
        playerWeapon.name = "Pistol";
    }
}

public class Submachine : PlayerWeapons
{
    public void initializePistol(PlayerWeapons playerWeapon)
    {
        playerWeapon.damage = 8;
        playerWeapon.range = 55f;
        playerWeapon.name = "Sub Machine Gun";
    }
}

public class Sniper : PlayerWeapons
{
    public void initializePistol(PlayerWeapons playerWeapon)
    {
        playerWeapon.damage = 100;
        playerWeapon.range = 750f;
        playerWeapon.name = "Sniper";
    }
}

public class Shotgun : PlayerWeapons
{
    public void initializePistol(PlayerWeapons playerWeapon)
    {
        playerWeapon.damage = 60;
        playerWeapon.range = 35f;
        playerWeapon.name = "Sawed Off Shotgun";
    }
}