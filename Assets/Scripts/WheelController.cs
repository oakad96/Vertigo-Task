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
    public GameObject takeRewardsPanel;
    public Button takeRewardsButton;
    [FormerlySerializedAs("_wheelCenter")] public Transform wheelCenter;

    public GameObject takeRewardsNotifications;

    [SerializeField] private List<WheelSegment> wheelSegments = new(8);
    [NonSerialized] public SpinCounter _spinCounter;

    [Header("Spin Speed")] public float spinSpeed = 500f; // Speed of rotation

    [Header("Wheel Sprites")] 
    public Image wheelImage;
    public Sprite bronzeWheelSprite;
    public Sprite silverWheelSprite;
    public Sprite goldWheelSprite;
    
    [Header("Tick Images")]
    public Image tickImage;
    public Sprite bronzeTickImage;
    public Sprite silverTickImage;
    public Sprite goldTickImage;
    

    [Header("UI Elements")] public Button spinButton;

    private void Start()
    {
        _spinCounter = FindObjectOfType<SpinCounter>();
        spinButton.onClick.AddListener(SpinWheel);
        takeRewardsButton.onClick.AddListener(TakeRewards);
        _segmentCount = wheelSegments.Count;
        _segmentAngle = 360f / _segmentCount;
        SetupWheel();
    }

    public void ResetWheel()
    {
        _spinCounter.ResetSpinCount();
        wheelImage.rectTransform.rotation = Quaternion.identity;
        ChangeWheelSprite(1);
    }

    public void SpinWheel()
    {
        if (_isSpinning || _inSpriteChangeAnimation) return;
        _isSpinning = true;
        EventManager.OnWheelSpun.Invoke();
        StartSpin();
    }
    private void StartSpin()
    {
        float spinTime = Random.Range(0.25f, 0.5f); // Random spin duration
        float totalSpinAngle = spinSpeed * spinTime; // Total spin angle

        wheelImage.transform.DORotate(new Vector3(0, 0, -totalSpinAngle), spinTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                float slowDownTime = spinTime * 6;  // Slow down duration
                float finalSlowSpinAngle = Mathf.Lerp(totalSpinAngle, totalSpinAngle + 180f, 1); // Adjust for final spin

                wheelImage.transform.DORotate(new Vector3(0, 0, -finalSlowSpinAngle), slowDownTime, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutQuad) // Ease out for the slow down effect
                    .OnComplete(() =>
                    {
                        _isSpinning = false;
                        _finalRotation = wheelImage.transform.eulerAngles.z;
                        DetermineReward(_finalRotation);  // Your custom method to determine the result
                        _spinCounter.IncreaseSpinCount();
                    });
            });
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

    // Method to calculate the position4 on a circle based on an angle
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
                    takeRewardsNotifications.SetActive(false);
                    tickImage.sprite = bronzeTickImage;
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
                    takeRewardsNotifications.SetActive(true);
                    tickImage.sprite = silverTickImage;
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
                    takeRewardsNotifications.SetActive(true);
                    tickImage.sprite = goldTickImage;
                });
        }
    }

    private void TakeRewards()
    {
        takeRewardsNotifications.SetActive(false);
        takeRewardsPanel.SetActive(true);
    }
}