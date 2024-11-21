using UnityEngine;

public class CountHolder : MonoBehaviour
{
    [SerializeField] private CountElement[] _countElements;

    public CountElement[] CountElements => _countElements;


    public void ReturnAllElementsToDefault()
    {
        foreach (var element in _countElements)
        {
            element.SetNumberSprite();
            element.Disable();
        }
    }

    public void ActivateElements(int count)
    {
        if (count > _countElements.Length)
            Debug.LogError("Incorrect count");

        for (int i = 0; i < _countElements.Length; i++)
        {
            if (i < count)
            {
                _countElements[i].Enable();
            }
            else
            {
                _countElements[i].Disable();
            }
        }
    }

    public void SetElementCompleted(int elementNumber)
    {
        if (elementNumber > _countElements.Length)
            Debug.LogError("Incorrect number");

        _countElements[elementNumber].SetCompleteSprite();
    }
}