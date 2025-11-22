using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{  
    // General
    public static GameManager singleton;
    
    [SerializeField] bool fullGameLoop = false;

    GameObject playerObject;

    [SerializeField] Animation uiAnim;

    float gameTimer = 0;
    float killModeTimer = 0;

    public bool dead = false;

    [SerializeField] Button restartButton;

    // Top down stuff
    GameObject topDownCamera;

    // FP stuff

    [SerializeField] GameObject fpPlayerPrefab;

    [SerializeField] TextMeshProUGUI timerText;

    bool killingMode = false;

    bool timerOn;
    [SerializeField] float startTime;
    float timeLeft;

    [SerializeField] int enemyCount;
    int enemiesLeft;
    
    bool hasWon = false;

    [SerializeField] GameObject musicInThisScene;
    static GameObject musicPlayer;

    public GameObject GetPlayerObj()
    {
        return playerObject;
    }

    public bool GetMode()
    {
        return killingMode;
    }

    void Awake()
    {
        singleton = this;

        if (musicPlayer)
        {
            Destroy(musicInThisScene);
        }
        else
        {
            musicPlayer = musicInThisScene;
            DontDestroyOnLoad(musicPlayer);
        }

        if(!playerObject)
        {
            playerObject = FindFirstObjectByType<TopDownPlayer>().gameObject;
        }

        if(!topDownCamera)
        {
            topDownCamera = FindFirstObjectByType<TopDownCamera>().gameObject;
        }

        enemyCount = FindObjectsByType<movmentScript>(FindObjectsSortMode.None).Count();
    }

    void Start()
    {
        enemiesLeft = enemyCount;

        if(timerText)
            timerText.enabled = false;

        if(restartButton)
            restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));

    }

    public void KillingMode()
    {   
        if(killingMode) return;

        killingMode = true;

        GameObject oldPlayer = playerObject;
        Debug.Log(oldPlayer);
        playerObject = Instantiate(fpPlayerPrefab);
        playerObject.transform.position = oldPlayer.transform.position;

        if(uiAnim)
            uiAnim.Play("lnaanim");

        StartTimer();
        
        Destroy(oldPlayer);
        Destroy(topDownCamera);
    }

    public void EnemyCanSeeYou()
    {
        if(dead) return;

        dead = true;

        Debug.Log("You are dead");

        if (fullGameLoop)
        {
            restartButton.gameObject.SetActive(true);
            if (playerObject.GetComponent<TopDownPlayer>())
            {
                playerObject.GetComponent<TopDownPlayer>().Die();
            }
            else if (playerObject.GetComponent<FPPlayer>())
            {
                playerObject.GetComponent<FPPlayer>().Die();
            }


            movmentScript[] enemies = FindObjectsByType<movmentScript>(FindObjectsSortMode.None);
            foreach(movmentScript enemy in enemies)
            {
                enemy.StopMoving();
            }
            //Time.timeScale = 0;

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void EnemyIsKilled()
    {
        Debug.Log("One less enemy, epic!");

        enemyCount--;

        if(enemyCount <= 0)
        {
            Debug.Log("You win!");
            hasWon = true;
        }
    }

    void StartTimer()
    {
        if(!timerText)
            return;

        timerText.enabled = true;
        timerOn = true;
        timeLeft = startTime;
    }

    void Update()
    {   
        if(!hasWon)
            gameTimer += Time.deltaTime;

        if (timerOn)
        {   
            if(!hasWon)
                killModeTimer += Time.deltaTime;

            timeLeft -= Time.deltaTime;
            timeLeft = Mathf.Clamp(timeLeft, 0, startTime);

            timerText.text = timeLeft.ToString("0.0");

            if(timeLeft <= 0)
            {
                timerOn = false;
                EnemyCanSeeYou();
            }
        }
    }
}
