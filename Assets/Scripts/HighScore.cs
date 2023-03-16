using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private Text highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        highScoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        highScoreText.text = "BEST SCORE\n" + PlayerPrefs.GetInt("HighScore", 0);
    }
}