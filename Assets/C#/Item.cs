using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    public Rigidbody2D I_rigid;
    public CircleCollider2D col;

    public bool isApear = false;

    public bool startUp = true;

    void Awake()
    {
        I_rigid = GetComponent<Rigidbody2D>();
    }

    IEnumerator Start()
    {
        //��ŸƮ���� �ƴ� �� �������   
        if (!startUp)
            yield break;

        //�̽��͸� �ڽ����� ������ ��ȯ
        col.enabled = false;
        I_rigid.gravityScale = 0;
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.Translate(0, Time.deltaTime, 0);
            yield return null;
        }
        I_rigid.gravityScale = 1;
        col.enabled = true;
        isApear = true;
    }

    public virtual void UseItem(Player player)
    {
        Destroy(this.gameObject);
    }
}
