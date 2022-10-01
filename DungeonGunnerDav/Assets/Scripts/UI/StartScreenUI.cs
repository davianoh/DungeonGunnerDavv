using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenUI : MonoBehaviour
{
    public void StartGame()
    {
        SaveSystem.Init();
        SceneManager.LoadScene("MainMapScene");
    }
}
