using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    small,
    big,
    flower,
    dead
}

public class Player : MonoBehaviour
{
    public static Player instance;
    public float maxSpeed;
    public float runSpeed;
    public float jumpPower;
    public float heightCutter;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    public CapsuleCollider2D col;

    private float lastHitTime = -10;
    public float hitInterval = 2;

    public Button GameOverBtn;

    public bool IsBig { get; private set; }
    public bool IsFire { get; private set; }

    public PlayerState state;
    public PlayerState State
    {
        get => state;
        set
        {
            switch (value)
            {
                case PlayerState.small:
                    anim.SetLayerWeight(0, 1);
                    anim.SetLayerWeight(1, 0);
                    anim.SetLayerWeight(2, 0);
                    col.size = new Vector2(col.size.x, 1);
                    StartCoroutine(nameof(ChangeMario));
                    break;

                case PlayerState.big:
                    IsBig = true;
                    col.size = new Vector2(col.size.x, 1.67f);
                    anim.SetBool("B_Idle", true);
                    anim.SetLayerWeight(0, 0);
                    anim.SetLayerWeight(1, 1);
                    anim.SetLayerWeight(2, 0);
                    StartCoroutine(nameof(ChangeMario));
                    break;

                case PlayerState.flower:
                    IsFire = true;
                    col.size = new Vector2(col.size.x, 1.67f);
                    anim.SetBool("F_Idle", true);
                    anim.SetLayerWeight(0, 0);
                    anim.SetLayerWeight(1, 0);
                    anim.SetLayerWeight(2, 1);
                    StartCoroutine(nameof(ChangeMario));
                    break;

                case PlayerState.dead:
                    anim.SetBool("Dead", true);
                    rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                    col.enabled = false;
                    rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
                    GameOverBtn.gameObject.SetActive(true);
                    break;
            }
            if (state != value && value != PlayerState.dead)
                StartCoroutine(ChangeState((int)state, (int)value));


            state = value;
        }
    }

    IEnumerator ChangeState(int a, int b)
    {
        for (int i = 0; i < 3; i++)
        {
            anim.SetLayerWeight(a, 1);
            anim.SetLayerWeight(b, 0);
            yield return new WaitForSecondsRealtime(0.1f);
            anim.SetLayerWeight(a, 0);
            anim.SetLayerWeight(b, 1);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public LayerMask detectionLayerMask;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
        instance = this;
    }

    void Update()
    {
        //if (Input.GetButtonDown("Run"))
        //{

        //}
        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") && !anim.GetBool("Dead"))
        {
            if (anim.GetBool("isJumping"))
            {
                return;
            }
            else
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }
        }
        //점프 높이
        if (Input.GetButtonUp("Jump"))
        {
            if (rigid.velocity.y > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * heightCutter);
            }
        }

        //움직임 멈추기
        if (Input.GetButtonUp("Horizontal")) //rigidbody의 linear drag 참고
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //스프라이트 좌우변경
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }

    void FixedUpdate()
    {
        //움직이는 속도
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), rigid.velocity.y);

        //레이히트 (에너미, 바닥 감지)
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(transform.position, -transform.up, Color.green);

            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector3.down, 1, detectionLayerMask);

            if (rayHit.collider != null)
            {
                if (rayHit.collider.TryGetComponent(out Goomba enemy))
                {
                    OnAttack(enemy.transform);
                }

                else
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }
    }

    //무언가가 플레이어게 닿았을때
    void OnCollisionEnter2D(Collision2D collision)
    {
        //미스터리 박스에 닿았을때
        if (collision.gameObject.CompareTag("Mysterybox"))
        {
            if (rigid.velocity.y > 0 && transform.position.y < collision.transform.position.y)
            {
                collision.transform.GetComponent<MysteryBox>().ItemSpawn();
            }
        }

        if (collision.gameObject.TryGetComponent(out IItem item))
        {
            item.UseItem(this);
        }

        //에너미에게 플레이어가 닿았을 때
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            PlayerHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //아이템에 닿았을때
        if (collision.gameObject.TryGetComponent(out IItem item))
        {
            item.UseItem(this);
        }
    }

    //에너미 공격
    void OnAttack(Transform enemy_tr)
    {
        rigid.AddForce(Vector2.up * 6, ForceMode2D.Impulse);

        Goomba goomba = enemy_tr.GetComponent<Goomba>();
        goomba.GoombaDamaged();
    }

    //플레이어가 맞았을 때
    public void PlayerHit()
    {

        if (Time.time - lastHitTime < hitInterval)
        {
            return;
        }

        if (!IsBig && !IsFire)
        {
            PlayerDead();
        }

        if (IsBig)
        {
            IsBig = false;
            State = PlayerState.small;
        }
        else if (IsFire)
        {
            IsBig = true;
            IsFire = false;
            State = PlayerState.big;
        }

        lastHitTime = Time.time;
    }

    //플레이어 사망
    public void PlayerDead()
    {
        State = PlayerState.dead;
    }

    //마리오 변신할 때 시간타임
    IEnumerator ChangeMario()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.4f);
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}