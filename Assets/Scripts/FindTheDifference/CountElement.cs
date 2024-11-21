using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CountElement : MonoBehaviour
{
    [SerializeField] private Sprite _numberSprite;
    [SerializeField] private Sprite _completeSprite;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _numberSprite = _image.sprite;
    }

    public void SetCompleteSprite()
    {
        _image.sprite = _completeSprite;
    }

    public void SetNumberSprite()
    {
        _image.sprite = _numberSprite;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
