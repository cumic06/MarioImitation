using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public int coinValue;

    public override void UseItem(Player player)
    {
        GameManager.Coin += coinValue;
        base.UseItem(player);
    }
}

