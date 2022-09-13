using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField]List<Enemy> enemies = new List<Enemy>();
    [SerializeField]List<Spawner> spawners = new List<Spawner>();
    [SerializeField]List<Enemy> prefabEnemy = new List<Enemy>();
    [SerializeField]List<string> ranks = new List<string>();
    [SerializeField]List<string> earnedRanks = new List<string>();
    [SerializeField]float yourTime = 0f;
    [SerializeField]int wave = 0;
    [SerializeField]int spawnEnemy = 0;
    [SerializeField]float timer = 2f;
    [SerializeField]float spawnTimer = 10f;
    [SerializeField]bool isYourTime = false;
    [SerializeField]Player player;
    [SerializeField]int currentRank = 0;
    [SerializeField]int score = 0;
    [SerializeField]GameObject gameOver;
    [SerializeField]GameObject uiNoAmmo;
    [Header("UI")]
    [SerializeField]TextMeshProUGUI txtYourTime;
    // Start is called before the first frame update

    public static GameManager instance;


    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }
    void Awake() 
    { 
    
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
        if(!isYourTime)
        {
            CheckWave();
            Timer();
        }
    }

    void CheckWave()
    {
        if(enemies.Count == 0 && spawnEnemy == 0)
        {
            wave++;
            spawnEnemy = wave * 5;
        }
    }

    void Timer()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            if(spawnEnemy > 0)
            {
                SpawnEnemy();
                Debug.Log("test");
                spawnEnemy -= 1;
            }
            timer = spawnTimer;
        }
    }

    void UpdateGUI()
    {
        txtYourTime.text = "" + Mathf.Round(yourTime);
    }

    public void ActionTime()
    {
        if(!isYourTime && yourTime <= 10f)
        {
            yourTime+=0.001f;
        }
        if(yourTime > 10f)
        {
            yourTime = 10;
        }
    }

    public float GetActionTime()
    {
        return yourTime;
    }

    public void StartYourTime()
    {
        StartCoroutine(YourTime());
    }

    IEnumerator YourTime()
    {
        // start your time in coroutine with timer of 10s;
        isYourTime = true;
        FreezeEnemy(true);
        while(yourTime > 0)
        {
            yourTime -= Time.deltaTime;
            yield return null;
        }
        FreezeEnemy(false);
        yourTime = 0;
        isYourTime = false;
        currentRank = 0;
    }

    public void FreezeEnemy(bool condition)
    {
        // freze or unfreeze any enemy
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetFreeze(true);
        }
    }

    public void RemoveEnemy(Enemy temp)
    {
        enemies.Remove(temp);

    }

    public void AddEnemy(Enemy temp)
    {
        enemies.Add(temp);
    }

    public void AddScore(Enemy temp)
    {
        if(!isYourTime && temp.GetFreeze() == false)
        {
            yourTime += 1;
        }
        else if(isYourTime)
        {
            currentRank += 1;
            if(currentRank > ranks.Count-1)
            {
                currentRank = ranks.Count-1;
            }
        }
        score += 1;
    }

    void SpawnEnemy()
    {
        int spawnerIndex = 0;
        int enemyIndex = 0;
        spawnerIndex = Random.Range(0,spawners.Count);
        enemyIndex = Random.Range(0,prefabEnemy.Count);
        spawners[spawnerIndex].Spawn(prefabEnemy[enemyIndex]);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        SceneManager.LoadScene("Game");
    }

    public void ShowNoAmmo()
    {
        StartCoroutine(NoAmmo());
    }

    IEnumerator NoAmmo()
    {
        float temp = 1f;
        uiNoAmmo.SetActive(true);
        while(temp > 0)
        {
            temp -= Time.deltaTime;
            yield return null;
        }
        uiNoAmmo.SetActive(false);
    }
}
