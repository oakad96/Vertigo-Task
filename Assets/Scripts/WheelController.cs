using UnityEngine;

public class WheelController : MonoBehaviour
{
    public float spinSpeed = 500f; // Speed of rotation
    private bool isSpinning = false;
    private float finalRotation = 0f;
    private float deltaRotation = 360f / 8f;
    private int finalZone = 0;

    // Call this method when the player presses the spin button
    public void SpinWheel()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine(Spin());
        }
    }

    private System.Collections.IEnumerator Spin()
    {
        float spinTime = Random.Range(2f, 4f); // Random spin duration
        float elapsedTime = 0f;

        while (elapsedTime < spinTime)
        {
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime); // Rotate the wheel
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Slow down the wheel
        float slowDownTime = 2f;
        while (slowDownTime > 0)
        {
            transform.Rotate(0f, 0f, Mathf.Lerp(spinSpeed, 0, 1 - slowDownTime / 2f) * Time.deltaTime);
            slowDownTime -= Time.deltaTime;
            yield return null;
        }

        isSpinning = false;
        finalRotation = transform.eulerAngles.z;
        finalZone = (int)(finalRotation % deltaRotation);
        Debug.Log(finalZone);
        // At this point, you'd check which slice the wheel stopped on
    }
}
