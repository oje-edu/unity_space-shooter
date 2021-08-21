using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public Camera Camera;
    public GameObject Player;
    public GameObject[] MeteoroidPrefabs;

    public float SpawnRateMinimum = 0.5f;
    public float SpawnRateMaximum = 1.5f;

    public float MeteoroidRotationMinimum = 0.5f;
    public float MeteoroidRotationMaximum = 1.5f;

    public float MeteoroidSpeedMinimum = 1;
    public float MeteoroidSpeedMaximum = 3;

    private float _nextSpawnTime;

    private void DetermineNextSpawnTime()
    {
        _nextSpawnTime = Time.time + Random.Range(SpawnRateMinimum, SpawnRateMaximum);
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnMeteoroid();
            DetermineNextSpawnTime();
        }
    }

    private void Start()
    {
        DetermineNextSpawnTime();
    }

    private void SpawnMeteoroid()
    {
        var prefabIndexToSpawn = Random.Range(0, MeteoroidPrefabs.Length);
        var prefabToSpawn = MeteoroidPrefabs[prefabIndexToSpawn];

        var meteoroid = Instantiate(prefabToSpawn, transform);

        var placeVertical = Random.Range(0, 2) == 0;
        float yPosition;
        float xPosition;

        if (placeVertical)
        {
            var halfWidth = Camera.orthographicSize * Camera.aspect;
            xPosition = Random.Range(-halfWidth, halfWidth);

            var sign = Random.Range(0, 2) == 0 ? -1 : 1;

            yPosition = sign * (Camera.orthographicSize * Camera.aspect + 1);

        }
        else
        {
            var halfHeight = Camera.orthographicSize;
            yPosition = Random.Range(-halfHeight, halfHeight);

            var sign = Random.Range(0, 2) == 0 ? -1 : 1;
            xPosition = sign * (Camera.orthographicSize * Camera.aspect + 1);
        }

        var position = new Vector3(xPosition, yPosition);
        meteoroid.transform.position = position;

        var direction = position - Player.transform.position;
        var speed = Random.Range(MeteoroidSpeedMinimum, MeteoroidSpeedMaximum);

        var rigidbody = meteoroid.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(-direction.normalized * speed, ForceMode2D.Impulse);

        var rotation = Random.Range(MeteoroidRotationMinimum, MeteoroidRotationMaximum);
        rigidbody.AddTorque(rotation);
    }
}
