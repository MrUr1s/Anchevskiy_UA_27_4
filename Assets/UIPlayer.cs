using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIPlayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _hPText,_wonOrLostText;

    public void SetHPText(int HPValue)
    {
        _hPText.text="HP: "+ HPValue.ToString();
    }
    public void SetWonOrLost(bool IsLost)
    {
        _wonOrLostText.text = IsLost ? "You Lost" : "You Won" ;
    }
}
