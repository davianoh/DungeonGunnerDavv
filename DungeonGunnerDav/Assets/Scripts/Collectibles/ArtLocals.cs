using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtLocals : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    private int health;
    [SerializeField] private HealthBar healthBar;
    private bool isColliding = false;
    private bool isGameOver = false;

    private void Start()
    {
        health = startingHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding) return;
        ContactDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isColliding) return;
        ContactDamage(collision);
    }

    private void ContactDamage(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            isColliding = true;
            Invoke("ResetContactCollision", Settings.artLocalDamageResetDelay);
            health -= Settings.artLocalContactDamage;
            healthBar.SetHealthBarValue((float)health / (float)startingHealth);
        }
        if(health <= 0)
        {
            isGameOver = true;
            GameManager.Instance.gameState = GameState.gameLost;
        }
    }

    private void ResetContactCollision()
    {
        if (isGameOver) return;
        isColliding = false;
    }
}
