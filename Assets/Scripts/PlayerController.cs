using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _acceleration;
    private float _steering;

    public Camera Camera;
    public TrailRenderer[] TrailRenderers;
    public SpriteRenderer SpriteRenderer;

    public Rigidbody2D Rigidbody;
    public GameManager GameManager;
    public GameObject Flame;

    public SpriteRenderer DamageOverlayRenderer;
    public Sprite[] DamageOverlays;
    private int _currentDamageOverlay = -1;

    [Header("Controls")]
    public float AccelerationSpeed = 3;
    public float SteeringSpeed = 3;

    [Header("Life")]
    public int MaximumHealth = 6;
    public int CurrentHealth;

    [Header("WeaponSystem")]
    public WeaponSystem[] WeaponSystems;

    private int _currentWeaponSystemIndex = 0;
    private Rect _visibleGameArea;
    private bool _resetTrails;



    private void Start()
    {
        CurrentHealth = MaximumHealth;
        GameManager.SetHealth(CurrentHealth);
        Flame.SetActive(false);

        DetermineBoundary();
    }

    private void DetermineBoundary()
    {
        var halfWidth = Camera.orthographicSize * Camera.aspect;
        var halfHight = Camera.orthographicSize;

        var shipSize = SpriteRenderer.size;

        _visibleGameArea = new Rect(-halfWidth - shipSize.x, -halfHight + shipSize.y, 2 * halfWidth + shipSize.x, 2 * halfHight + shipSize.y);
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

        if (_resetTrails)
        {
            foreach (var trailRenderer in TrailRenderers)
            {
                trailRenderer.Clear();
            }

            _resetTrails = false;
        }
    }

    private void FixedUpdate()
    {
        Rigidbody.AddRelativeForce(new Vector2(0, _acceleration * AccelerationSpeed));
        Rigidbody.AddTorque(-_steering * SteeringSpeed);

        ContainInBoundary();
    }

    private void ContainInBoundary()
    {
        var position = Rigidbody.position;
        var newPosition = Vector2.zero;

        if (position.x < _visibleGameArea.xMin)
        {
            newPosition = new Vector2(_visibleGameArea.xMax, position.y);
        }

        if (position.x > _visibleGameArea.xMax)
        {
            newPosition = new Vector2(_visibleGameArea.xMin, position.y);
        }

        if (position.y < _visibleGameArea.yMin)
        {
            newPosition = new Vector2(position.x, _visibleGameArea.yMax);
        }

        if (position.y > _visibleGameArea.yMax)
        {
            newPosition = new Vector2(position.x, _visibleGameArea.yMin);
        }

        if (newPosition != Vector2.zero)
        {
            Rigidbody.position = newPosition;
            _resetTrails = true;
        }
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
            gameObject.SetActive(false);
            GameManager.GameOver();
        }
    }
}
