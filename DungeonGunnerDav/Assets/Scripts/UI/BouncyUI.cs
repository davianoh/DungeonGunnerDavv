using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BouncyUI : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        StartScreenUI.Instance.ShakeTitle();
    }

    private void Start()
    {
        StartScreenUI.Instance.IdlePlayButton();
    }

    
}
