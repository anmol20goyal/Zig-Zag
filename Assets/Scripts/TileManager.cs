using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    
    #region GameObjects

    private GameObject currentTile;
    public GameObject mainPlatform;
    public GameObject[] tilePrefabs;
    public Animator startPageAnimator, gameOverAnimator;

    #endregion

    #region UI GameObjects

    public GameObject Score;
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject playBtn;

    #endregion
    
    #region Instance

    private static TileManager instance;
    public static TileManager InstanceTM
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TileManager>();
            }

            return TileManager.instance;
        }
    }

    #endregion

    #region Stacks

    public Stack<GameObject> leftTiles = new Stack<GameObject>();
    public Stack<GameObject> topTiles = new Stack<GameObject>();

    #endregion

    public enum PageState
    {
        None,
        Start,
        GameOver
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetPageState(PageState.Start);

        int randomIndex = Random.Range(0, 2);
        currentTile = Instantiate(tilePrefabs[randomIndex] , mainPlatform.transform.GetChild(0).transform.GetChild(randomIndex).position , Quaternion.identity);

        createTiles(50);

        for (int i = 0; i < 50; i++)
        {
            spawnTile();
        }
    }

    private void Update()
    {
        Score.GetComponent<Text>().text = PlayerMovement.score.ToString();
    }
    
    void createTiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            leftTiles.Push(Instantiate(tilePrefabs[0]));
            leftTiles.Peek().name = "LeftTile";
            leftTiles.Peek().SetActive(false);
            topTiles.Push(Instantiate(tilePrefabs[1]));
            topTiles.Peek().name = "TopTiles";
            topTiles.Peek().SetActive(false);
        }
    }

    public void spawnTile()
    {

        if (leftTiles.Count == 0 || topTiles.Count == 0)
        {
            createTiles(10);
        }
        
        // to randomize btw left and top tile
        int RandomIndex = Random.Range(0, 2); // [0] for left tile [1] for top tile

        // to show the tiles in the respective stack
        if (RandomIndex == 0)
        {
            GameObject tmp = leftTiles.Pop();
            tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(RandomIndex).position;
            tmp.SetActive(true);
            currentTile = tmp;
        }
        else if (RandomIndex == 1)
        {
            GameObject tmp = topTiles.Pop();
            tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(RandomIndex).position;
            tmp.SetActive(true);
            currentTile = tmp;
        }

        // to spawn diamonds randomly
        int RanSpawnDiamond = Random.Range(0, 10);
        if (RanSpawnDiamond == 0)
        {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                Score.SetActive(true);
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;

            case PageState.Start:
                Score.SetActive(false);
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                break;
            
            case PageState.GameOver:
                Score.SetActive(false);
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                break;
        }
    }

    public void HelpBtn()
    {
        StartCoroutine(SceneChanger.InstanceSC.Transition(2));
    }

    public void InfoBtn()
    {
        StartCoroutine(SceneChanger.InstanceSC.Transition(1));
    }

    public void RetryBtn()
    {
        gameOverAnimator.enabled = false;
        SetPageState(PageState.Start);
        StartCoroutine(SceneChanger.InstanceSC.Transition(SceneManager.GetActiveScene().buildIndex));
    }

    public void PlayBtn()
    {
        playBtn.SetActive(false);
        Score.SetActive(true);
        PlayerMovement.InstancePM.StartMove();
        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransition()
    {
        startPageAnimator.SetTrigger("StartGame");
        yield return new WaitForSeconds(1.2f);
        SetPageState(PageState.None);
    }
    
    public void ResetBtn()
    {
        PlayerMovement.score = 0;
        gameOverPage.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
        PlayerPrefs.DeleteKey("HighScore");
    }
    
    public void ExitBtn()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void BackBtn()
    {
        // in SceneChanger script
    }
    
    public void ForwardBtn()
    {
        // in SceneChanger script
    }
}
