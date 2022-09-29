using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMapBackground;
    [SerializeField] private GameObject mainMapUI;
    
    public void ExitCharacterMenuUI()
    {
        mainMapBackground.SetActive(true);
        mainMapUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
