using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelController : MonoBehaviour
{
    private bool _isSpinning = false;
    private float _finalRotation = 0f;
    private const float DeltaRotation = 360f / 8f;
    private int _finalZone = 0;
    [NonSerialized] public SpinCounter _spinCounter;
    [Header("Spin Speed")]
    public float spinSpeed = 500f; // Speed of rotation

    [Header("Wheel Sprites")]
    public Image wheelImage;
    public Sprite bronzeWheelSprite;
    public Sprite silverWheelSprite;
    public Sprite goldWheelSprite;
    
    [Header("UI Elements")]
    public Button spinButton;

    private void Start()
    {
        _spinCounter = FindObjectOfType<SpinCounter>();
        spinButton.onClick.AddListener(SpinWheel);
    }
    public void SpinWheel()
    {
        if (_isSpinning) return;
        _isSpinning = true;
        StartCoroutine(Spin());
        _spinCounter.IncreaseSpinCount();
    }

    private System.Collections.IEnumerator Spin()
    {
        float spinTime = Random.Range(0.25f, 0.5f); // Random spin duration
        float elapsedTime = 0f;

        while (elapsedTime < spinTime)
        {
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float slowDownTime = 0.25f;
        while (slowDownTime > 0)
        {
            transform.Rotate(0f, 0f, Mathf.Lerp(spinSpeed, 0, 1 - slowDownTime / 2f) * Time.deltaTime);
            slowDownTime -= Time.deltaTime;
            yield return null;
        }

        _isSpinning = false;
        _finalRotation = transform.eulerAngles.z;
        // TO DO fix this
        _finalZone = (int)(_finalRotation % DeltaRotation);
    }

    public void ChangeWheelSprite(int option)
    {
        if (option == 1)
        {
            wheelImage.sprite = bronzeWheelSprite;
        }
        else if (option == 3)
        {
            wheelImage.sprite = goldWheelSprite;
        }
        else
        {
            wheelImage.sprite = silverWheelSprite;
        }
    }
}
