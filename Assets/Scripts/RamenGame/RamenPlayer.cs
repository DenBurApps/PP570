using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Collider2D))]
public class RamenPlayer : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _transparentColor;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _minBorder;
    [SerializeField] private float _maxBorder;

    private Vector2 _previousTouchPosition;
    private RectTransform _rectTransform;
    private IEnumerator _movingCoroutine;
    private Image _image;

    public event Action<InteractableObject> CorrectObjectCatched;
    public event Action IncorrectObjectCatched;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    private void OnValidate()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void EnableMovement()
    {
        if (_movingCoroutine == null)
        {
            _movingCoroutine = StartMovement();
        }

        StartCoroutine(_movingCoroutine);
        _image.color = _defaultColor;
    }

    public void DisableMovement()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
        
        _image.color = _transparentColor;
    }

    private IEnumerator StartMovement()
    {
        while (enabled)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _previousTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouchPosition = touch.position;
                    Vector2 moveDelta = currentTouchPosition - _previousTouchPosition;

                    Vector3 newPosition = _rectTransform.anchoredPosition;
                    newPosition.x += moveDelta.x * _speed;

                    newPosition.x = Mathf.Clamp(newPosition.x, _minBorder, _maxBorder);

                    _rectTransform.anchoredPosition = newPosition;

                    _previousTouchPosition = currentTouchPosition;
                }
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable))
        {
            if (interactable is IncorrectObject)
            {
                IncorrectObjectCatched?.Invoke();
            }
            else if(interactable is CorrectObject)
            {
                CorrectObjectCatched?.Invoke((InteractableObject)interactable);
            }
        }
    }
}
