using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenUI : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMapMusic);
    }

    public void StartGame()
    {
        SaveSystem.Init();
    }
}
