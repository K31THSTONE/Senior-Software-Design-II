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
	public static void initializePistol(){
		this.setName("Pistol");
		this.setDmg(10);
		this.setRng(75f);
		this.setFireRate(0f);
		this.setBullets(80);
		this.setReloadTime(0f);
		//this.setGraphics();
		this.setClipSize(8);
		this.setMaxAmmo(80);
	}
	public static void initializeSubmachine(){
		this.setName("Sub Machine Gun");
		this.setDmg(5);
		this.setRng(55f);
		this.setFireRate(2f);
		this.setBullets(150);
		this.setReloadTime(2f);
		//this.setGraphics();
		this.setClipSize(30);
		this.setMaxAmmo(150);
	}
	public static void initializeSniper(){
		this.setName("Sniper");
		this.setDmg(100);
		this.setRng(750f);
		this.setFireRate(0f);
		this.setBullets(10);
		this.setReloadTime(5f);
		//this.setGraphics();
		this.setClipSize(1);
		this.setMaxAmmo(10);
	}
	public static void initializeShotgun(){
		this.setName("Sawed Off Shotgun");
		this.setDmg(80);
		this.setRng(35f);
		this.setFireRate(0f);
		this.setBullets(30);
		this.setReloadTime(5f);
		//this.setGraphics();
		this.setClipSize(2);
		this.setMaxAmmo(30);
	}
public class Pistol : PlayerWeapons
{
  
}

public class Submachine : PlayerWeapons
{
    
}

public class Sniper : PlayerWeapons
{
    
}

public class Shotgun : PlayerWeapons
{
    
}