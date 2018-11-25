using UnityEngine;
using System.Collections;
using Options;

public class AnimationAutoDestroy : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
