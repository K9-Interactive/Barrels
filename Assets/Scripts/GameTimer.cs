using UnityEngine;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float startTimeInSeconds = 30f;
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool isRunning = false;

    public Action OnTimerFinished;

    public void StartTimer()
    {
        timeRemaining = startTimeInSeconds;
        isRunning = true;
    }

    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;

            OnTimerFinished?.Invoke();
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        }
    }
}