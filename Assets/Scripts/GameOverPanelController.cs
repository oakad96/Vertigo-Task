using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    public WheelController wheelController;
    public Button restartButton;
    public GameObject gameOverPanel;
    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        wheelController.ResetWheel();
        gameOverPanel.SetActive(false);
    }
}
