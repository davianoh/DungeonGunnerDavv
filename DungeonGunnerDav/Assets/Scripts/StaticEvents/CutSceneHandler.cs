using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneHandler : MonoBehaviour
{
    public void ChangeSceneMainMap()
    {
        SceneManager.LoadScene("MainMapScene");
    }
}
