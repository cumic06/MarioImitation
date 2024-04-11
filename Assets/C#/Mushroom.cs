using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Item
{
    void Update()
    {
        if (isApear)
        {
            transform.Translate(Time.deltaTime, 0, 0);
        }
    }

    public override void UseItem(Player player)
    {
        if(!player.IsBig)
        {
            player.State = PlayerState.big;
            GameManager.Score += 50;
        }

        base.UseItem(player);
    }
}
