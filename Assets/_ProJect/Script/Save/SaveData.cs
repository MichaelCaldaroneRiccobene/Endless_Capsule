using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public bool hasDoubleLife;
    public bool hasShield;
    public int coin;
    public float[] scoresArray;

    public bool hasPowerUp(PowerUp powerUp)
    {
        return powerUp switch
        {
            PowerUp.None => false,
            PowerUp.DoubleLife => hasDoubleLife,
            PowerUp.Shield => hasShield,
            _ => false,
        };
    }

    public void UnlockPowerUp(PowerUp powerUp)
    {
        switch (powerUp)
        {
            case PowerUp.None:
                break;
            case PowerUp.DoubleLife:
                hasDoubleLife = true;
                break;
            case PowerUp.Shield:
                hasShield = true;   
                break;
        }
    }
}
