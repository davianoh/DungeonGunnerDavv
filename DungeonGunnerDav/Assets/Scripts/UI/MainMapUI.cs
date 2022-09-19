using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMapUI : MonoBehaviour
{
    public void PlayLevel()
    {
        SceneManager.LoadScene("MainGameScene");
    }


}
