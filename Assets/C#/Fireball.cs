using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireboll : MonoBehaviour
{
    public Rigidbody2D firb;
    void Start()
    {
        firb = GetComponent<Rigidbody2D>();
    }
}
