using System;
using TMPro;
using UnityEngine;

public class SpinCounter : MonoBehaviour
{
    public TextMeshProUGUI spinCountText;
    private int _spinCount = 0;
    public WheelController wheelController;

    private void Start()
    {
        wheelController = FindObjectOfType<WheelController>();
    }

    public void IncreaseSpinCount()
    {
        _spinCount++;
        UpdateSpinCountText();

        if (_spinCount % 5 == 0)
        {
            ChangeWheelToSilver();
        }
        else if (_spinCount % 30 == 0)
        {
            ChangeWheelToGold();
        }
        else
        {
            ChangeWheelToBronze();
        }
    }

    private void UpdateSpinCountText()
    {
        spinCountText.text = _spinCount.ToString();
    }
    
    private void ChangeWheelToBronze()
    {
        wheelController.ChangeWheelSprite(1);
    }
    
    private void ChangeWheelToSilver()
    {
        wheelController.ChangeWheelSprite(1);
    }

    private void ChangeWheelToGold()
    {
        wheelController.ChangeWheelSprite(1);
    }

}
