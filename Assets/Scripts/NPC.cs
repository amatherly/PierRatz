using UnityEngine;

public class NPC

{


    private readonly string myName;
    private int myHealth;
    private int myDamage;
    private bool myIsAlive;


    public NPC(string name, int health)
    {
        myName = name;
        myHealth = health;
        myIsAlive = true;
    }

    public int Hit()
    {
        myHealth -= myDamage;
        if (myHealth <= 0)
        {
            myIsAlive = false;
            return 0;
        }
        else
        {
            return myHealth;
        }
    }

    public bool IsAlive()
    {
        return myIsAlive;
    }
}
