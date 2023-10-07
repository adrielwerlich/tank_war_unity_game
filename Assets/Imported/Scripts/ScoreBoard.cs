using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    int score;

    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        scoreText.text = "Start";
    }


    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        scoreText.text = score.ToString();
    }

}
