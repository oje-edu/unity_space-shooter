using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoroidController : MonoBehaviour
{
    public int Damage = 1;
    public int Points = 1;

    public int MaximumHealth = 3;
    public int CurrentHealth;

    public GameObject ExplosionPrefab;

    private GameManager _gameManager;
    private void Start()
    {
        // Not best practice (for big games)
        _gameManager = FindObjectOfType<GameManager>();

        CurrentHealth = MaximumHealth;
        Destroy(gameObject, 30f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Ausführung wenn Kollision stattfinded und
        // IsTrigger = false
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ausführung wenn Kollision stattfinded und
        // IsTrigger = true

        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();

            playerController.TakeDamage(Damage);

            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            _gameManager.AddPoints(Points);
            Destroy(gameObject);
        }
    }
}
