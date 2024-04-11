using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    public int nextMove;

    private Rigidbody2D M_rigid;

    public int score;

    void Awake()
    {
        M_rigid = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        circleCollder = GetComponent<CircleCollider2D>();

        StartCoroutine(ThinkCor(5));
    }



    void FixedUpdate()
    {
        //Move
        M_rigid.velocity = new Vector2(nextMove, M_rigid.velocity.y);

        //PlatFormCheck
        Vector2 forntVec = new Vector2(M_rigid.position.x + nextMove * 0.3f, M_rigid.position.y);
        Debug.DrawRay(forntVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(forntVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Return();
        }
    }

    //재귀 함수 : 스스로 호출하는 함수
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);
        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);
        //Flip Sprite
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    IEnumerator ThinkCor(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        while (true)
        {
            //Set Next Active
            nextMove = Random.Range(-1, 2);
            //Sprite Animation
            anim.SetInteger("WalkSpeed", nextMove);
            //Flip Sprite
            if (nextMove != 0)
            {
                spriteRenderer.flipX = nextMove == 1;
            }

            //Recursive
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    void Return()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;

        StopAllCoroutines();
        StartCoroutine(ThinkCor(2));
    }

    void DeActive()
    {
        GameManager.Score += 100;
        gameObject.SetActive(false);
    }

    public void GoombaDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        circleCollder.enabled = false;

        anim.SetBool("M_Dead", true);

        M_rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        Invoke("DeActive", 2);
    }
    public override void AttackPlayer(Player player)
    {

    }

}
