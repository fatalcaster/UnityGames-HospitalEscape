using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public int maxHealth = 3;


    public int numOfHearts;

    public Image[] lifes;
    public Sprite life;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
            lifes[i].enabled = true;
        numOfHearts = 3;
    }

    
    // Update is called once per frame
    void Update()
    {
        int health = Player.health;
        if (health == 0)
        {
            for (int i = 0; i < 3; i++)
                lifes[i].enabled = false;
            numOfHearts = 0;
        }
        if (health < numOfHearts)
        {
            lifes[health].enabled = false;
            numOfHearts--;
        }

        if (health > numOfHearts)
        {
            lifes[health-1].enabled = true;
            numOfHearts++;
        }
    }


}
