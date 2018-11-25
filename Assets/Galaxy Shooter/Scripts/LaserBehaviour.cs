using UnityEngine;
using UnityEditor;
using Options;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    [SerializeField]
    private float _damage = 10f;

    void Update()
    {
        if (transform.position.y > 7)
        {
            var parent = transform.parent;
            if (NullCheck.Some(parent))
                Destroy(parent.gameObject);

            Destroy(gameObject);
        }

        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            var enemy = other.GetComponent<EnemyAI>();
            if (NullCheck.Some(enemy))
                enemy.Damage(_damage);

            Destroy(gameObject);
        }
    }
}