using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _acceleration;
    private float _steering;

    public float AccelerationSpeed = 3;
    public float SteeringSpeed = 3;

    public int MaximumHealth = 6;
    public int CurrentHealth;

    public WeaponSystem[] WeaponSystems;
    private int _currentWeaponSystemIndex = 0;

    public GameObject Flame;

    public Rigidbody2D Rigidbody;
    public GameManager GameManager;

    public SpriteRenderer DamageOverlayRenderer;
    public Sprite[] DamageOverlays;
    private int _currentDamageOverlay = - 1;

    private void Start()
    {
        CurrentHealth = MaximumHealth;
        GameManager.SetHealth(CurrentHealth);
        Flame.SetActive(false);
    }
    private void Update()
    {
        _acceleration = Math.Max(0, Input.GetAxis("Vertical"));
        _steering = Input.GetAxis("Horizontal");

        Flame.SetActive(_acceleration > 0);

        if (Input.GetKey(KeyCode.Space))
        {
            WeaponSystems[_currentWeaponSystemIndex].Fire();
        }

        // change weapon
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentWeaponSystemIndex--;
            if (_currentWeaponSystemIndex < 0)
            {
                _currentWeaponSystemIndex = WeaponSystems.Length - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentWeaponSystemIndex++;
            if (_currentWeaponSystemIndex >= WeaponSystems.Length)
            {
                _currentWeaponSystemIndex = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        Rigidbody.AddRelativeForce(new Vector2(0, _acceleration * AccelerationSpeed));
        Rigidbody.AddTorque(-_steering * SteeringSpeed);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        GameManager.SetHealth(CurrentHealth);

        _currentDamageOverlay++;
        _currentDamageOverlay = Math.Min(_currentDamageOverlay, DamageOverlays.Length - 1);
        DamageOverlayRenderer.sprite = DamageOverlays[_currentDamageOverlay];

        if (CurrentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
