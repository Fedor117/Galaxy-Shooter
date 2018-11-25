using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private float _health = 10f;

    [SerializeField]
    private int _score = 10;

    [SerializeField]
    private GameObject _deathAnimation;

    private UiManager _uiManager;

    public void Damage()
    {
        Damage(1f);
    }

    public void Damage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
            Die();
    }

	private void Start()
	{
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
	}

	private void Update ()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        var position = transform.position;
        if (position.y < -6)
            transform.position = new Vector3(Random.Range(-6, 6), 6, position.z);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            if (NullCheck.Some(player))
                player.Damage();

            Die();
        }
    }

    private void Die()
    {
        Instantiate(_deathAnimation, transform.position, Quaternion.identity);

        if (NullCheck.Some(_uiManager))
            _uiManager.UpdateScore(_score);

        Destroy(gameObject);
    }
}
