using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSegmentDisplay : MonoBehaviour
{
    public Image segmentImage;
    public TextMeshProUGUI rewardText;

    public void SetSegmentData(WheelSegment segmentData)
    {
        segmentImage.sprite = segmentData.segmentSprite;
        rewardText.text = segmentData.isBomb ? "x1!" : $"x{segmentData.rewardAmount}";
    }

    public void PlayRewardTakenAnimation()
    {
        segmentImage.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
    }
}