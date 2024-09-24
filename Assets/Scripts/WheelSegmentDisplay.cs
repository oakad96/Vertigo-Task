using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSegmentDisplay : MonoBehaviour
{
    public Image segmentImage;         // The image representing the wheel segment
    public TextMeshProUGUI rewardText; // The text displaying the reward

    public void SetSegmentData(WheelSegment segmentData)
    {
        segmentImage.sprite = segmentData.segmentSprite;
        rewardText.text = segmentData.isBomb ? "x1!" : $"x{segmentData.rewardAmount}";
    }
}