using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [HideInInspector] public int Value;
    [HideInInspector] public int Row;
    [HideInInspector] public int Col;
    [HideInInspector] public bool IsLocked;
    [HideInInspector] public bool IsIncorrect;

    [SerializeField] private SpriteRenderer _bgSrpite;
    [SerializeField] private TMP_Text _valueText;

    public void Init(int value)
    {
        IsIncorrect = false;
        Value = value;

        if (value == 0)
        {
            IsLocked = false;
            _valueText.text = "";
        }
        else
        {
            IsLocked = true;
            _valueText.text = Value.ToString();
        }
    }

    public void Select()
    {
        /*if (IsIncorrect)
        {
            _valueText.color = Color.red;
        }
        else
        {
            _valueText.color = Color.green;
        }*/
    }


    public void Reset()
    {
       
    }

    public void UpdateValue(int value)
    {
        Value = value;
        _valueText.text = Value == 0 ? "" : Value.ToString();
    }

    public void UpdateWin()
    {
       
    }
}
