using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour instance = null;

    public GameObject Shooter;
    public GameObject Chaser;

    GameObject set;
    GameObject input1;
    GameObject input2;
    GameObject player;
    GameObject spawner;
    GameObject hud;
    GameObject overScreen;

    private string lastScene;
    private List<Transform> spawnPoint = new List<Transform>();

    private int matchLength = 60;
    private int spawnDelay = 10;

    private int score = 0;

    private Coroutine routine1;
    private Coroutine routine2;

    void Awake()
    {
        //Singleton Pattern (kind of)
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {//Adds to sceneLoaded event a delegate pointing to LevelJustLoaded
        SceneManager.sceneLoaded += LevelJustLoaded;
    }

    void OnDisable()
    {//Subtracts from sceneLoaded event a delegate pointing to LevelJustLoaded
        SceneManager.sceneLoaded -= LevelJustLoaded;
    }

    void LevelJustLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            matchLength = 60;
            spawnDelay = 10;
            input1 = GameObject.FindGameObjectWithTag("Input1");
            input2 = GameObject.FindGameObjectWithTag("Input2");
        }
        if(SceneManager.GetActiveScene().name == "MainGame")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            spawner = GameObject.FindGameObjectWithTag("Spawner");
            hud = GameObject.FindGameObjectWithTag("HUD");
            overScreen = GameObject.FindGameObjectWithTag("Over");
            overScreen.SetActive(false);
            spawnPoint = new List<Transform>();
            for(int i = 0; i < spawner.transform.childCount; i++) { spawnPoint.Add(spawner.transform.GetChild(i)); }
            score = 0;
            routine1 = StartCoroutine(MatchCountDown(matchLength));
            routine2 = StartCoroutine(Spawner(spawnDelay));
        }
    }

    IEnumerator MatchCountDown(int length)
    {
        yield return new WaitForSeconds(length);
        GameOver();
        yield break;
        StopCoroutine(routine1);
    }

    IEnumerator Spawner(int delay)
    {
        while(SceneManager.GetActiveScene().name == "MainGame")
        {
            GameObject enemy;
            if(Random.Range(0, 4) < 3) { enemy = Shooter; }
            else { enemy = Chaser; }
            int r = Random.Range(0, spawnPoint.Count);
            Instantiate(enemy, spawnPoint[r].position, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        yield break;
        StopCoroutine(routine2);
    }

    public void GameOver()
    {
        hud.SetActive(false);
        Time.timeScale = 0.3f;
        overScreen.GetComponentInChildren<Text>().text = "Você marcou "+score+" pontos !";
        overScreen.SetActive(true);
    }

    public void ChangeScene()
    {
        string name = "";
        if(SceneManager.GetActiveScene().name == "MainMenu") { name = "MainGame"; }
        if(SceneManager.GetActiveScene().name == "MainGame") { name = "MainMenu"; }
        SceneManager.LoadScene(name);
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void IncrementScore(int score)
    {
        this.score += score;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    //UI
    
    public void UpdateSettings()
    {
        matchLength = Mathf.Abs(int.Parse(input1.GetComponent<InputField>().text));
        spawnDelay = Mathf.Abs(int.Parse(input2.GetComponent<InputField>().text));
    }

    //GetSet

    public string GetLastSceneName()
    {
        return lastScene;
    }
}
