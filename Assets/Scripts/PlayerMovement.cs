using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    #region GameObjects
    
    public ParticleSystem ps;
    private Transform player;

    #endregion

    #region VARIABLES

    public static int score;
    public bool isDead;
    public float speed;
    private Vector3 direc;
    private string btnName;
    public Transform contactPoint;

    #endregion
    
    #region InstancePM

    private static PlayerMovement instance;
    public static PlayerMovement InstancePM
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerMovement>();
            }

            return PlayerMovement.instance;
        }
    }

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        score = -1;
        isDead = false;
        player = GetComponent<Transform>();
        DiamondTextController.Initialize();
        direc = Vector3.zero;
    }

    // to get the name of the play button to be used in if condition in Update method
    public void GetBtnName(Button btn)
    {
        btnName = btn.name;
    }
    
    // Update is called once per frame
    private void Update()
    {
        // game over condition
        if (!isGrounded())
        {
            isDead = true;
            if (transform.childCount > 0)
            {
                transform.GetChild(0).transform.parent = null; // removing all the children from the ball or player
            }
            GameOver();
        }
        
        // to move the ball on mouse button or click
        if (Input.GetMouseButtonDown(0) && !isDead && btnName ==  "PlayBtn")
        {
            if (direc == Vector3.forward)
            {
                direc = Vector3.left;
            }
            else
            {
                direc = Vector3.forward;
            }

            score++;
        }
        
        player.Translate(speed * Time.deltaTime * direc);
    }

    // for moving the ball as soon as the play button is pressed
    public void StartMove()
    {
        if (direc == Vector3.forward)
        {
            direc = Vector3.left;
        }
        else
        {
            direc = Vector3.forward;
        }

        player.Translate(speed * Time.deltaTime * direc);
        score++;
    }

    //for collecting diamonds
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Diamond")
        {
            DiamondTextController.createFloatingText(other.transform);
            other.gameObject.SetActive(false); //deactivating the diamond
            Instantiate(ps, transform.position, Quaternion.identity); //particle effect on hitting the diamond
            score += 2;
        }
    }

    // called when the ball falls off the edge
    private void GameOver()
    {
        if (isDead)
        {
            TileManager.InstanceTM.SetPageState(TileManager.PageState.GameOver);
            TileManager.InstanceTM.gameOverAnimator.SetBool("GameOver",true);
            TileManager.InstanceTM.gameOverPage.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Text>().text = "SCORE\n" + score.ToString();
            int savedScore = PlayerPrefs.GetInt("HighScore", 0);
            if (score > savedScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
                TileManager.InstanceTM.gameOverPage.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
    
    // to check if the ball is grounded or not
    private bool isGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(contactPoint.position, 0.1f);
        foreach (Collider variable in colliders)
        {
            if (variable.gameObject != gameObject)
            {
                return true;
            }
        }
        
        return false;
    }
    
    // for making a sphere at the contact point as Physics.OverlapSphere is invisible
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere (contactPoint.position, 0.1f);
    }
    
}
