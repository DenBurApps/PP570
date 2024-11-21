using System;
using System.Collections;
using UnityEngine;

public class StartGameScreen : MonoBehaviour
{
    private const int Interval = 1;
    
    private int _lifecycle;

    public event Action GameStarted; 

    private void Awake()
    {
        _lifecycle = 2;
    }

    public IEnumerator ShowCoroutine()
    {
        WaitForSeconds lifecycle = new WaitForSeconds(Interval);

        while (_lifecycle > 0)
        {
            _lifecycle--;
            yield return lifecycle;
        }
        
        GameStarted?.Invoke();
        Disable();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowCoroutine());
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
