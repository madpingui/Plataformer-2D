using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject[] hearts = new GameObject[3];
    public static bool PlayerTakeDamage;
    public float InvulnerabilityTime;
    public static bool beingImmune;
    private int heartCount;

    void Awake()
    {
        pausePanel.SetActive(false);
        heartCount = 3;
        beingImmune = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!pausePanel.activeInHierarchy) 
                PauseGame();
            else
                ContinueGame();
        }
        if(PlayerTakeDamage){
            //maybe a taking damage animation
            beingImmune = true;
            PlayerTakeDamage = false;
            heartCount --;
            hearts[heartCount].SetActive(false);
            Invoke("Vulnerability", InvulnerabilityTime);
        }
        if(!hearts[0].activeInHierarchy)
            ReloadGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    } 
    private void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public static void ReloadGame(){
        SceneManager.LoadScene("SampleScene");
    }

    public void ReloadGameForButton(){
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    private void Vulnerability(){
        beingImmune = false;
    }
}