using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    protected CircleCollider2D circleCollder;
    public virtual void AttackPlayer(Player player)
    {
        Player.instance.PlayerHit();
    }
}
