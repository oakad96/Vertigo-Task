using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelController : MonoBehaviour
{
    private bool _isSpinning = false;
    private bool _inSpriteChangeAnimation = false;
    private float _finalRotation = 0f;
    private int _segmentCount;
    private float _segmentAngle;
    private float _radius = 110;
    public GameObject wheelSegmentPrefab;
    public GameObject tryAgainPanel;
    [FormerlySerializedAs("_wheelCenter")] public Transform wheelCenter;

    [SerializeField] private List<WheelSegment> wheelSegments = new(8);
    [NonSerialized] public SpinCounter _spinCounter;

    [Header("Spin Speed")] public float spinSpeed = 500f; // Speed of rotation

    [Header("Wheel Sprites")] public Image wheelImage;
    public Sprite bronzeWheelSprite;
    public Sprite silverWheelSprite;
    public Sprite goldWheelSprite;

    [Header("UI Elements")] public Button spinButton;

    private void Start()
    {
        _spinCounter = FindObjectOfType<SpinCounter>();
        spinButton.onClick.AddListener(SpinWheel);
        _segmentCount = wheelSegments.Count;
        _segmentAngle = 360f / _segmentCount;
        SetupWheel();
    }

    public void ResetWheel()
    {
        _spinCounter.ResetSpinCount();
        ChangeWheelSprite(1);
    }

    public void SpinWheel()
    {
        if (_isSpinning || _inSpriteChangeAnimation) return;
        _isSpinning = true;
        EventManager.OnWheelSpun.Invoke();
        StartCoroutine(Spin());
        _spinCounter.IncreaseSpinCount();
    }

    private System.Collections.IEnumerator Spin()
    {
        float spinTime = Random.Range(0.25f, 0.5f); // Random spin duration
        float elapsedTime = 0f;

        while (elapsedTime < spinTime)
        {
            wheelImage.transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float slowDownTime = 0.25f;
        while (slowDownTime > 0)
        {
            wheelImage.transform.Rotate(0f, 0f, Mathf.Lerp(spinSpeed, 0, 1 - slowDownTime / 2f) * Time.deltaTime);
            slowDownTime -= Time.deltaTime;
            yield return null;
        }

        _isSpinning = false;
        _finalRotation = wheelImage.transform.eulerAngles.z;
        DetermineReward(_finalRotation);
    }

    private void SetupWheel()
    {
        float defaultAspectRatio = 16f / 9f;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float aspectRatio = screenWidth / screenHeight;
        
        float dynamicRadius = _radius * Mathf.Min(1f, aspectRatio / defaultAspectRatio);
        float angleStep = 360f / wheelSegments.Count;

        for (int i = 0; i < wheelSegments.Count; i++)
        {
            GameObject segmentObj = Instantiate(wheelSegmentPrefab, wheelCenter);

            float angle = i * angleStep;

            Vector3 position = GetPositionOnCircle(wheelCenter.position, dynamicRadius, angle);
            segmentObj.transform.position = position;

            segmentObj.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            WheelSegmentDisplay segmentDisplay = segmentObj.GetComponent<WheelSegmentDisplay>();
            segmentDisplay.SetSegmentData(wheelSegments[i]);
        }
    }

    // Method to calculate the position on a circle based on an angle
    private Vector3 GetPositionOnCircle(Vector3 center, float radius, float angleDegrees)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float x = center.x + radius * Mathf.Cos(angleRadians);
        float y = center.y + radius * Mathf.Sin(angleRadians);
        return new Vector3(x, y, 0);
    }

    private void DetermineReward(float finalAngle)
    {
        int segmentIndex = Mathf.FloorToInt((finalAngle % 360) / _segmentAngle);

        WheelSegment stoppedSegment = wheelSegments[segmentIndex];

        if (stoppedSegment.isBomb)
        {
            EventManager.OnBombHit.Invoke();
            Debug.Log("You lose;");
            tryAgainPanel.SetActive(true);
        }
        else
        {
            EventManager.OnRewardHit.Invoke();
            Debug.Log("You win;" + stoppedSegment.segmentName);
        }
    }


    public void ChangeWheelSprite(int option)
    {
        _inSpriteChangeAnimation = true;
        if (option == 1)
        {
            wheelImage.transform.DOScale(0f, 0.5f)
                .SetEase(Ease.OutBounce)
                .SetDelay(1f)
                .OnComplete(() =>
                {
                    wheelImage.sprite = bronzeWheelSprite;
                    wheelImage.transform.DOScale(1f, 0.5f);
                    _inSpriteChangeAnimation = false;
                });
        }
        else if (option == 2)
        {
            wheelImage.transform.DOScale(0f, 0.5f)
                .SetEase(Ease.OutBounce).SetDelay(1f)
                .OnComplete(() =>
                {
                    wheelImage.sprite = silverWheelSprite;
                    wheelImage.transform.DOScale(1f, 0.5f);
                    _inSpriteChangeAnimation = false;
                });
        }
        else if (option == 3)
        {
            wheelImage.transform.DOScale(0f, 0.5f)
                .SetEase(Ease.OutBounce).SetDelay(1f)
                .OnComplete(() =>
                {
                    wheelImage.sprite = goldWheelSprite;
                    wheelImage.transform.DOScale(1f, 0.5f);
                    _inSpriteChangeAnimation = false;
                });
        }
    }
}