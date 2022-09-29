using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCharacterSelection : MonoBehaviour
{
    public CharacterSelectorUI characterSelector;
    public int arrowID;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (arrowID == 0)
            {
                characterSelector.NextCharacter();
            }
            else if (arrowID == 1)
            {
                characterSelector.BackCharacter();
            }
        }
    }

    private void OnMouseEnter()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.08627451f, 0.5294118f, 0.09803922f, 1);
    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.6039216f, 0.145098f, 0.145098f, 1);
    }
}
