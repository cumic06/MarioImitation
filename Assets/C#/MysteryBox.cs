using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mysterybox
{
    mushroom = 0,
    flower,
    star
}

public class MysteryBox : MonoBehaviour
{
    public GameObject[] mysteryObject;
    private Animator myAnim;
    public bool use;
    public Mysterybox mb;

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }
    public void ItemSpawn()
    {
        if (!use)
        {
            Instantiate(mysteryObject[(int)mb], transform.position, Quaternion.identity);
            use = true;
        }
    }

    public void FixedUpdate()
    { 
        UseMysterybox();
    }

    public void UseMysterybox()
    {
        if (use)
        {
            myAnim.SetLayerWeight(1, 1);
            myAnim.SetLayerWeight(0, 0);
        }
    }
}

