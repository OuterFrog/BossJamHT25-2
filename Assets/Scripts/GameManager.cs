using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [SerializeField] bool fullGameLoop = false;

    GameObject playerObject;
    GameObject topDownCamera;

    [SerializeField] Animation uiAnim;

    [SerializeField] GameObject fpPlayerPrefab;

    [SerializeField] TextMeshProUGUI timerText;

    bool killingMode = false;

    bool timerOn;
    [SerializeField] float startTime;
    float timeLeft;

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

        if(playerObject == null)
        {
            playerObject = FindFirstObjectByType<TopDownPlayer>().gameObject;
        }

        if(topDownCamera == null)
        {
            topDownCamera = FindFirstObjectByType<TopDownCamera>().gameObject;
        }
    }

    void Start()
    {
        if(timerText)
            timerText.enabled = false;   
    }

    public void KillingMode()
    {   
        if(killingMode) return;

        killingMode = true;

        GameObject oldPlayer = playerObject;
        playerObject = Instantiate(fpPlayerPrefab);
        fpPlayerPrefab.transform.position = oldPlayer.transform.position;

        if(uiAnim)
            uiAnim.Play("lnaanim");

        StartTimer();
        
        Destroy(oldPlayer);
        Destroy(topDownCamera);
    }

    public void EnemyCanSeeYou()
    {
        Debug.Log("You are dead");

        if (fullGameLoop)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void EnemyIsKilled()
    {
        Debug.Log("One less enemy, epic!");
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
        if (timerOn)
        {
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
