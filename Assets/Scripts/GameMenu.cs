using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI menuGameText;
    private Transform visualElements;
    void Start()
    {
        InputManager.ToggleShowGameMenu += DisplayGameMenu;
        CrabCollisionController.ShowGameMenu += DisplayGameMenu;

        visualElements = this.transform.Find("VisualElements");

        visualElements.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        InputManager.ToggleShowGameMenu -= DisplayGameMenu;
        CrabCollisionController.ShowGameMenu -= DisplayGameMenu;    
    }

    private void DisplayGameMenu(bool isGameOver = false)
    {
        if (isGameOver) {
            menuGameText.text = "Game Over";
        }
        visualElements.gameObject.SetActive(!visualElements.gameObject.activeSelf);
    }

    private void DisplayGameMenu()
    {
        menuGameText.text = "Game Paused";
        visualElements.gameObject.SetActive(!visualElements.gameObject.activeSelf);
    }

}
