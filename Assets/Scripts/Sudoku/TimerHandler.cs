using System.Collections;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;

    private float _currentTime;
    private IEnumerator _timerCoroutine;


    public void Activate()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        _timerCoroutine = null;

        _timerCoroutine = StartTimer();
        StartCoroutine(_timerCoroutine);
    }

    public void Stop()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        _timerCoroutine = null;
    }

    public void ResetAndStart()
    {
        _currentTime = 0;
        
        Activate();
    }

    public string GetTimerText()
    {
        return _timerText.text;
    }
    
    private IEnumerator StartTimer()
    {
        while (enabled)
        {
            _currentTime += Time.deltaTime;
            UpdateTimer(_currentTime);
            yield return null;
        }
    }

    private void UpdateTimer(float time)
    {
        time += 1;

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        
         
        _timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
