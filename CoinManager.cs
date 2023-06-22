using UnityEngine.UI;
using System;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager instance;
    public Text text;
    public int CoinValue = 1; // wartość jednej monety
    private int CoinCounter = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        text.text = "COINS:" + CoinCounter.ToString();
    }

    public void ChangeScore()
    {
        CoinCounter += CoinValue;
        text.text = "COINS:" + CoinCounter.ToString();
    }

}
