using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Statistics : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI victoriesText;
    [SerializeField] private TextMeshProUGUI maxScoreText;
    [SerializeField] private TextMeshProUGUI maxValueText;

    public void AddVictory()
    {
        int increasedVictoriesCount = PlayerPrefs.GetInt("Victories") + 1;
        PlayerPrefs.SetInt("Victories", increasedVictoriesCount);
        victoriesText.text = increasedVictoriesCount.ToString();
    }

    public void ChangeMaxScore(int newScore)
    {
        PlayerPrefs.SetInt("Score", newScore);
        maxScoreText.text = newScore.ToString();
    }

    public void ChangeMaxNumber(int newNumber)
    {
        PlayerPrefs.SetInt("Value", newNumber);
        maxValueText.text = newNumber.ToString();
    }

    private void Awake()
    {
        victoriesText.text = PlayerPrefs.GetInt("Victories").ToString();
        maxScoreText.text = PlayerPrefs.GetInt("Score").ToString();
        maxValueText.text = PlayerPrefs.GetInt("Value").ToString();
    }
}
