using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CircleCollider2D))]
public class InteractableObject : MonoBehaviour,IInteractable
{
    private readonly float _speed = 1200f;
    private RectTransform _rectTransform;
    private IEnumerator _movingCoroutine;
    
    public RectTransform RectTransform => _rectTransform;

    private void OnEnable()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
    
    private void OnDisable()
    {
        DisableMovement();
    }
    
    public void EnableMovement()
    {
        if (_movingCoroutine == null)
            _movingCoroutine = StartMoving();

        StartCoroutine(_movingCoroutine);
    }
    
    public void DisableMovement()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
    }

    private IEnumerator StartMoving()
    {
        while (true)
        {
            _rectTransform.position += Vector3.down * _speed * Time.deltaTime;
            
            yield return null;
        }
    }
}