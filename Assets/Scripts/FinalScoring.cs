using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalScoring : MonoBehaviour {

	private int finalScore;
	private int currentScore;
	private Text finalText; 

	void Start () {
		finalScore = ScoreManager.score;
		finalText = GetComponent <Text> ();
		currentScore = 0;
		finalText.text = "" + currentScore;
	}

	void Update () {
        AnimateScoreCount();
        AnimateScoreCount();
        AnimateScoreCount();
		AnimateScoreCount();
	}

    void AnimateScoreCount()
    {
        if (currentScore + 16 < finalScore)
        {
            currentScore += 26;
            finalText.text = "" + currentScore;
        }
        else
        {
            finalText.text = "" + finalScore;
        }
    }
}
