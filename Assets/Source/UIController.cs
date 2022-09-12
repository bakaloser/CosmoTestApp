using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    protected Text scoreTxt;
    [SerializeField]
    protected Text distanceTxt;

    public void UpdateUI(int score, float distance)
    {
        scoreTxt.text = "Score: " + score;
        distanceTxt.text = "Distance: " + Math.Round((decimal)distance, 3);
    }
}
