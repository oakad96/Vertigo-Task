using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // Define custom events for different actions
    public static UnityEvent OnWheelSpun = new UnityEvent();
    public static UnityEvent OnRewardHit = new UnityEvent();
    public static UnityEvent OnBombHit = new UnityEvent();

    // You can add more events based on game logic
}