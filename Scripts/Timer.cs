using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool timerGoing;

    public static event OnEndTime onEndTime;
    public delegate void OnEndTime();

    [SerializeField] private float startTime = 120f;

    private Text text;
    private float elapsedTime;
    private TimeSpan timePlaying;

    private void Start()
    {
        text = GetComponent<Text>();
        text.text = "00:00.00";
        elapsedTime = startTime;
        timerGoing = true;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime -= Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            text.text = timePlayingStr;
            if (elapsedTime <= 0)
            {
                timerGoing = false;
                onEndTime?.Invoke();
            }
            yield return null;
        }
    }
}
