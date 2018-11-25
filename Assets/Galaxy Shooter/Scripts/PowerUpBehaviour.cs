using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private long powerUpID;

    [SerializeField]
    private AudioClip _powerUpAudio;

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            if (Options.NullCheck.Some(player))
            {
                if (powerUpID == 0)
                    player.TripleShotPowerUpOn();
                else if (powerUpID == 1)
                    player.SpeedPowerUpOn();
                else if (powerUpID == 2)
                    player.ShieldPowerUpOn();

                AudioSource.PlayClipAtPoint(_powerUpAudio, Camera.main.transform.position, .5f);

                Destroy(gameObject);
            }
        }
    }
}
