using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;

public sealed class WorldManager : MonoBehaviour
{
    [SerializeField]
    GameObject _playerGameObject;

    [SerializeField]
    Vector3 _playerStartPosition;

    [SerializeField]
    GameObject _enemyShip;

    [SerializeField]
    float _enemySpawnInterval;

    [SerializeField]
    GameObject[] _powerUps;

    [SerializeField]
    float _powerUpSpawnInterval;

    IEnumerator _enemySpawnCoroutine;
    IEnumerator _bonusSpawnCoroutine;

    public void SpawnPlayer()
    {
        Instantiate(_playerGameObject, _playerStartPosition, Quaternion.identity);
    }

    public void SpawnTwoPlayers()
    {
        // First Player
        var position = _playerStartPosition;
        position.x = -4.5f;

        var firstPlayer = Instantiate(_playerGameObject, position, Quaternion.identity);
        var renderer = firstPlayer.GetComponent<SpriteRenderer>();
        if (NullCheck.Some(renderer))
            renderer.material.SetColor("_Color", new Color(.59f, .77f, 1f, 1f));

        // Second Player
        position = _playerStartPosition;
        position.x = 4.5f;

        var secondPlayer = Instantiate(_playerGameObject, position, Quaternion.identity);
        secondPlayer.GetComponent<Player>().SetId(2);
        renderer = secondPlayer.GetComponent<SpriteRenderer>();
        if (NullCheck.Some(renderer))
            renderer.material.SetColor("_Color", new Color(.85f, .33f, .08f, 1f));
    }

    public void StartSpawning()
    {
        StartCoroutine(_enemySpawnCoroutine = SpawnEnemyCoroutine(_enemySpawnInterval));
        StartCoroutine(_bonusSpawnCoroutine = SpawnPowerUpCoroutine(_powerUpSpawnInterval));
    }

	public void StopSpawning()
	{
        if (NullCheck.Some(_enemySpawnCoroutine))
        {
            StopCoroutine(_enemySpawnCoroutine);
            _enemySpawnCoroutine = null;
        }

        if (NullCheck.Some(_bonusSpawnCoroutine))
        {
            StopCoroutine(_bonusSpawnCoroutine);
            _bonusSpawnCoroutine = null;
        }
	}

    public void DestroyAll()
    {
        var players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            if (NullCheck.Some(player))
                Destroy(player.gameObject);
        }

        var enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            if (NullCheck.Some(enemy))
                Destroy(enemy.gameObject);
        }

        var powerUps = FindObjectsOfType<PowerUpBehaviour>();
        foreach (PowerUpBehaviour powerUp in powerUps)
        {
            if (NullCheck.Some(powerUp))
                Destroy(powerUp.gameObject);
        }

        var lasers = FindObjectsOfType<LaserBehaviour>();
        foreach (LaserBehaviour laser in lasers)
        {
            if (NullCheck.Some(laser))
                Destroy(laser.gameObject);
        }
    }
	
	IEnumerator SpawnEnemyCoroutine(float interval)
    {
        while (true)
        {
            Instantiate(_enemyShip, new Vector3(Random.Range(-6, 6), 6, 0), Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator SpawnPowerUpCoroutine(float interval)
    {
        while (true)
        {
            Instantiate(_powerUps[Random.Range(0, _powerUps.Length)], new Vector3(Random.Range(-6, 6), 6, 0), Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }
}
