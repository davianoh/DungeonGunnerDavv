using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class EnemyTypeDetailUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI detailText;

    [SerializeField] private int levelValue;
    [SerializeField] private int healthValue;
    [SerializeField] private int attackValue;
    [SerializeField] private Sprite enemyIconValue;

    private void Start()
    {
        GetComponent<Image>().sprite = enemyIconValue;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        detailText.text = "Lvl: " + levelValue.ToString() + "     Health: " + healthValue.ToString() + "     Attack: " + attackValue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailText.text = "Detail Musuh";
    }
}
