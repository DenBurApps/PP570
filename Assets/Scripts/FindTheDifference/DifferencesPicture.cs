using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferencesPicture : MonoBehaviour
{
    private const int DisableInterval = 3;

    [SerializeField] private BoxCollider2D[] _differences;
    [SerializeField] private SpriteRenderer _correctChoiceImage;
    [SerializeField] private SpriteRenderer _incorrectChoiceImage;

    [SerializeField] private float _minXBounds;
    [SerializeField] private float _maxXBounds;
    [SerializeField] private float _minYBounds;
    [SerializeField] private float _maxYBounds;

    private List<SpriteRenderer> _images = new List<SpriteRenderer>();
    private IEnumerator _inputCoroutine;
    private bool _canTouch = true;

    public event Action<int> DifferenceIndexFound;
    public event Action LevelComplete;

    public bool IsComplete { get; private set; }
    public int DifferencesCount { get; private set; }

    private void OnEnable()
    {
        DifferencesCount = _differences.Length;
    }

    private void OnDisable()
    {
        DisableInput();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void EnableInput()
    {
        if (_inputCoroutine != null)
        {
            StopCoroutine(_inputCoroutine);
            _inputCoroutine = null;
        }

        _inputCoroutine = ValidateTouchInput();
        StartCoroutine(_inputCoroutine);
    }

    public void DisableInput()
    {
        if (_inputCoroutine != null)
        {
            StopCoroutine(_inputCoroutine);
            _inputCoroutine = null;
        }
    }

    public void ReturnToDefault()
    {
        foreach (var difference in _differences)
        {
            difference.enabled = true;
        }

        foreach (var image in _images)
        {
            Destroy(image);
        }

        _images.Clear();
        IsComplete = false;
        DifferencesCount = _differences.Length;
    }

    public void DisableAllImages()
    {
        foreach (var image in _images)
        {
            Destroy(image.gameObject);
        }

        _images.Clear();
    }

    private IEnumerator ValidateTouchInput()
    {
        while (enabled)
        {
            if (Input.touchCount > 0 && _canTouch)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    
                    if (touchPosition.x >= _minXBounds && touchPosition.x <= _maxXBounds &&
                        touchPosition.y >= _minYBounds && touchPosition.y <= _maxYBounds)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                        if (hit.collider != null && hit.collider.TryGetComponent(out BoxCollider2D boxCollider))
                        {
                            SearchTheDifference(boxCollider);
                        }
                        else
                        {
                            var incorrectObject = Instantiate(_incorrectChoiceImage, touchPosition, Quaternion.identity);
                            Destroy(incorrectObject.gameObject, 3);
                            StartCoroutine(DisableInputForPeriod());
                        }
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator DisableInputForPeriod()
    {
        _canTouch = false;
        yield return new WaitForSeconds(DisableInterval);
        _canTouch = true;
    }

    private void SearchTheDifference(BoxCollider2D collider)
    {
        for (int i = 0; i < _differences.Length; i++)
        {
            if (_differences[i] == collider)
            {
                _differences[i].enabled = false;
                var correctSprite = Instantiate(_correctChoiceImage, _differences[i].transform.position,
                    Quaternion.identity);

                _images.Add(correctSprite);
                DifferenceIndexFound?.Invoke(i);
                DifferencesCount--;

                if (DifferencesCount <= 0)
                {
                    IsComplete = true;
                    DisableAllImages();
                    LevelComplete?.Invoke();
                }

                break;
            }
        }
    }
}