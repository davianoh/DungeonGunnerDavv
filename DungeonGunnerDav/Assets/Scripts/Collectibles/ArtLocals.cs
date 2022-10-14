using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtLocals : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    private int health;
    [SerializeField] private HealthBar healthBar;
    private bool isColliding = false;
    private float resetCooldownTime;
    private bool justCollided = true;

    private void Start()
    {
        health = startingHealth;
        resetCooldownTime = Settings.artLocalDamageResetDelay;
    }

    private void Update()
    {
        if(health <= 0f)
        {
            GameManager.Instance.gameState = GameState.gameLost;
            healthBar.gameObject.SetActive(false);
        }

        if (justCollided && isColliding)
        {
            StartCoroutine(ReceiveDamageRoutine());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            isColliding = false;
        }
    }

    private IEnumerator ReceiveDamageRoutine()
    {
        justCollided = false;
        while (isColliding)
        {
            health -= Settings.artLocalContactDamage;
            healthBar.SetHealthBarValue((float)health / (float)startingHealth);
            yield return new WaitForSeconds(resetCooldownTime);
        }
        justCollided = true;
    }

}
