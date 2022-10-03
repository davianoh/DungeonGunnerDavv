using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private CharacterSelectorUI characterSelectorUI;
    [SerializeField] private TextMeshProUGUI buttonCharacterText;
    private Image buttonCharacterImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        characterSelectorUI.SelectCharacter();
    }

    private void Awake()
    {
        buttonCharacterImage = GetComponent<Image>();
    }

    private void Update()
    {
        if(characterSelectorUI.selectedPlayerIndex == CharacterMenuUI.Instance.selectedPlayerIndex)
        {
            buttonCharacterImage.color = Color.green;
            buttonCharacterText.text = "SELECTED";
        }
        else if (CharacterMenuUI.Instance.characterOwnedList.Contains(characterSelectorUI.selectedPlayerIndex))
        {
            buttonCharacterImage.color = Color.red;
            buttonCharacterText.text = "SELECT";
        }
        else
        {
            buttonCharacterImage.color = Color.red;
            buttonCharacterText.text = "BUY 100G";
        }
    }


}
