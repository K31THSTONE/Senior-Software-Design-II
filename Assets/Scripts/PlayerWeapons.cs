using UnityEngine;
[System.Serializable]


public class PlayerWeapons  {


    public string name;
    public int damage;
    public float range;
    public float fireRate;
    public int maxAmmo;
    public int clipSize;
    public int bullets;
    public float reloadTime;
    public GameObject graphics;

    public PlayerWeapons()
    {
        this.bullets = maxAmmo;
    }

    public string getName()
    {
        return this.name;
    }
    public int getDmg()
    {
        return this.damage;
    }
    public float getRng()
    {
        return this.range;
    }
    public float getfireRate()
    {
        return this.range;
    }
    public int getClipSize()
    {
        return this.clipSize;
    }
    public float getBullets()
    {
        return this.bullets;
    }
    public float getReloadTime()
    {
        return this.reloadTime;
    }
    public int getMaxAmmo()
    {
        return this.maxAmmo;
    }
    public GameObject getGraphics()
    {
        return this.graphics;
    }
    public void setName(string name)
    {
        this.name = name;
    }
    public void setDmg(int damage)
    {
        this.damage = damage;
    }
    public void setRng(float range)
    {
        this.range = range;
    }
    public void setFireRate(float fireRate)
    {
        this.fireRate= fireRate;
    }
    public void setClipSize(int clipSize)
    {
        this.clipSize = clipSize;
    }
    public void setBullets(int bullets)
    {
        this.bullets = bullets;
    }
    public void setReloadTime(float reloadTime)
    {
        this.reloadTime = reloadTime;
    }
    public void setMaxAmmo(int maxAmmo)
    {
        this.maxAmmo = maxAmmo;
    }
    public void setGraphics(GameObject graphics)
    {
        this.graphics = graphics;
    }
}

public class Pistol : PlayerWeapons
{
    public static void initializePistol(PlayerWeapons playerWeapon)
    {
        playerWeapon.setName("Pistol");
        playerWeapon.setDmg(10);
        playerWeapon.setRng(75f);
        playerWeapon.setFireRate(0f);
        playerWeapon.setBullets(80);
        playerWeapon.setReloadTime(0f);
        //playerWeapon.setGraphics()
        playerWeapon.setClipSize(8);
        playerWeapon.setMaxAmmo(80);
    }
}

public class Submachine : PlayerWeapons
{
    public void initializeSubmachine(PlayerWeapons playerWeapon)
    {
        playerWeapon.setName("Sub Machine Gun");
        playerWeapon.setDmg(5);
        playerWeapon.setRng(55f);
        playerWeapon.setFireRate(2f);
        playerWeapon.setBullets(150);
        playerWeapon.setReloadTime(2f);
        //playerWeapon.setGraphics()
        playerWeapon.setClipSize(30);
        playerWeapon.setMaxAmmo(150);
    }
}

public class Sniper : PlayerWeapons
{
    public void initializeSniper(PlayerWeapons playerWeapon)
    {
        playerWeapon.setName("Sniper");
        playerWeapon.setDmg(100);
        playerWeapon.setRng(750f);
        playerWeapon.setFireRate(0f);
        playerWeapon.setBullets(10);
        playerWeapon.setReloadTime(5f);
        //playerWeapon.setGraphics()
        playerWeapon.setClipSize(1);
        playerWeapon.setMaxAmmo(10);
    }
}

public class Shotgun : PlayerWeapons
{
    public void initializeShotgun(PlayerWeapons playerWeapon)
    {
        playerWeapon.setName("Sawed Off Shotgun");
        playerWeapon.setDmg(80);
        playerWeapon.setRng(35f);
        playerWeapon.setFireRate(0f);
        playerWeapon.setBullets(30);
        playerWeapon.setReloadTime(5f);
        //playerWeapon.setGraphics()
        playerWeapon.setClipSize(2);
        playerWeapon.setMaxAmmo(30);
    }
}