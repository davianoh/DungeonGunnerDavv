using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMapUI : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMapMusic, 0f, 2f);
        //SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
    }

    public void PlayLevel()
    {
        MapManager.Instance.Save();
        SceneManager.LoadScene("MainGameScene");
    }


}
