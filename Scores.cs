using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scores : MonoBehaviour
{
    [SerializeField] private Transform scorePanel;
    [SerializeField] private Transform riflePanel;
    [SerializeField] private float distance;
    [SerializeField] private float sizeMult;

    [Space(15)]
    [SerializeField] private TextMeshPro scoreIndicator;
    [SerializeField] private TextMeshPro scoreForShotIndicator;
    [SerializeField] private Color possibleShotColor;
    [SerializeField] private Color impossibleShotColor;

    private int currentScore;
    private int currentShotScore = 4;

    public int score => currentScore;
    public int shotScore => currentShotScore;

    public void Place(Vector2 center, int height, int sizeScale)
    {
        scorePanel.position = center + Vector2.up * (distance + height / 2);
        scorePanel.localScale *= sizeMult * height;

        riflePanel.position = center + Vector2.down * (distance + height / 2);
        riflePanel.localScale *= sizeMult * height;
    } 

    public void ChangeScore(int score)
    {
        currentScore += score;
        scoreIndicator.text = currentScore.ToString();
        if (currentScore >= currentShotScore) scoreForShotIndicator.color = possibleShotColor;
        else scoreForShotIndicator.color = impossibleShotColor;
    }

    public void IncreaseScoresForShot(int shotScore)
    {
        currentShotScore = shotScore;
        scoreForShotIndicator.text = currentShotScore.ToString();
        if (currentScore >= currentShotScore) scoreForShotIndicator.color = possibleShotColor;
        else scoreForShotIndicator.color = impossibleShotColor;
    }
}
