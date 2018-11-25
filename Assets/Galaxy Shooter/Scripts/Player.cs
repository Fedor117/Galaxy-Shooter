using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;

public class Player : MonoBehaviour
{
    [SerializeField]
    float _speed = 5f;

    [SerializeField]
    GameObject _laserPrefab;

    [SerializeField]
    GameObject _tripleShot;

    [SerializeField]
    GameObject _deathAnimation;

    [SerializeField]
    GameObject _shieldGameObject;

    [SerializeField]
    float _fireRate = .25f;
    float _nextFire = .0f;

    [SerializeField]
    int _lives;

    [SerializeField]
    GameObject[] _engineFailures;

    UiManager _uiManager;
    AudioSource _audioShot;

    bool _canTripleShot = false;
    bool _canSpeedUp = false;
    bool _canReflectShots = false;

    int _id = 1;
    int _hitCount = 0;

    public void Damage()
    {
        Damage(1);
    }

    public void Damage(int amount)
    {
        if (_canReflectShots)
        {
            _canReflectShots = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        _hitCount++;
        if (_hitCount == 1)
            _engineFailures[0].SetActive(true);
        else if (_hitCount == 2)
            _engineFailures[1].SetActive(true);

        _lives -= amount;

        if (NullCheck.Some(_uiManager))
            _uiManager.UpdateLives(_lives);

        if (_lives <= 0)
            Die();
    }

    public void TripleShotPowerUpOn()
    {
        _canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedPowerUpOn()
    {
        _canSpeedUp = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    public void ShieldPowerUpOn()
    {
        _canReflectShots = true;
        _shieldGameObject.SetActive(true);
        StartCoroutine(ShieldPowerDownRoutine());
    }

    public void SetId(int id)
    {
        _id = id;
    }

    public int GetId()
    {
        return _id;
    }

	void Start()
	{
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        if (NullCheck.Some(_uiManager))
            _uiManager.UpdateLives(_lives);

        _audioShot = GetComponent<AudioSource>();
	}

	void Update()
    {
        if (_id == 1)
            MovementPlayerOne();
        else
            MovementPlayerTwo();

        Shooting();
    }

    void MovementPlayerOne()
    {
        var horizontal = 0;
        if (Input.GetKey(KeyCode.D))
            horizontal = 1;
        else if (Input.GetKey(KeyCode.A))
            horizontal = -1;

        var vertical = 0;
        if (Input.GetKey(KeyCode.W))
            vertical = 1;
        else if (Input.GetKey(KeyCode.S))
            vertical = -1;

        Movement(horizontal, vertical);
    }

    void MovementPlayerTwo()
    {
        var horizontal = 0;
        if (Input.GetKey(KeyCode.RightArrow))
            horizontal = 1;
        else if (Input.GetKey(KeyCode.LeftArrow))
            horizontal = -1;

        var vertical = 0;
        if (Input.GetKey(KeyCode.UpArrow))
            vertical = 1;
        else if (Input.GetKey(KeyCode.DownArrow))
            vertical = -1;

        Movement(horizontal, vertical);
    }

    void Movement(float horizontal, float vertical)
    {
        var speed = _canSpeedUp ? _speed * 2f : _speed;
        var translation = new Vector3(
                    1 * speed * horizontal * Time.deltaTime,
                    1 * speed * vertical * Time.deltaTime,
                    0);

        var position = transform.position;
        if (position.y > 0)
        {
            if (translation.y > 0)
                translation.y = 0;
        }
        else if (position.y < -4.2f)
        {
            if (translation.y < 0)
                translation.y = 0;
        }

        if (position.x > 9.4f)
            transform.position = new Vector3(-9.4f, position.y);
        else if (position.x < -9.4f)
            transform.position = new Vector3(9.4f, position.y);

        transform.Translate(translation);
    }

    void Shooting()
    {
        var isPlayerOne = _id == 1;
        var keyboardShootButton = isPlayerOne ? KeyCode.Space : KeyCode.RightControl;
        var mouseShootButton = isPlayerOne ? 0 : 1;
        if (Time.time > _nextFire && (Input.GetKeyDown(keyboardShootButton) || Input.GetMouseButtonDown(mouseShootButton)))
        {
            if (_canTripleShot)
                TripleShot();
            else
                Shoot();

            _nextFire = Time.time + _fireRate;
            if (NullCheck.Some(_audioShot))
                _audioShot.Play();
        }
    }

    void Shoot()
    {
        Shoot(0f, .89f);
    }

    void Shoot(float offsetX, float offsetY)
    {
        var position = transform.position;
        position.x += offsetX;
        position.y += offsetY;
        Instantiate(_laserPrefab, position, Quaternion.identity);
    }

    void TripleShot()
    {
        Instantiate(_tripleShot, transform.position, Quaternion.identity);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _canTripleShot = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _canSpeedUp = false;
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _canReflectShots = false;
        _shieldGameObject.SetActive(false);
    }

    void Die()
    {
        Instantiate(_deathAnimation, transform.position, Quaternion.identity);

        var gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (NullCheck.Some(gameController))
            gameController.OnPlayerDied();

        Destroy(gameObject);
    }
}
