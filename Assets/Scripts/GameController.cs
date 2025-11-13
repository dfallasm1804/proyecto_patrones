using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject gameOverScreen;

    public static event Action OnReset;
    
    private static GameController _instance;
    public static GameController Instance {get {return _instance;}}
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);
        HoldToLoad.OnHoldComplete += Restart;
    }

    // Update is called once per frame
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        gameOverScreen.SetActive(false);
        player.transform.position = new Vector2(6, 31);
        OnReset.Invoke();
        Time.timeScale = 1;
    }
}
