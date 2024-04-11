using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoretext;
    public Text cointext;

    public static int Score = 0;
    public static int Coin = 0;

    public void Start()
    {
        scoretext.text = $"Score : {Score}";
        cointext.text = $"Coin : {Coin}";
    }

    public void FixedUpdate()
    {
        scoretext.text = $"Score : {Score}";
        cointext.text = $"Coin : {Coin}";
    }

    public void ReSetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}