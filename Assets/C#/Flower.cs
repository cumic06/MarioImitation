using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Item
{
    public override void UseItem(Player player)
    {
        if (!player.IsFire)
        {
            player.State = PlayerState.flower;
            GameManager.Score += 50;
        }

        base.UseItem(player);
    }
}
