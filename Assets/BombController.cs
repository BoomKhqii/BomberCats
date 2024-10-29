using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public AudioSource bombExplode;

    public int range = 1;
    public float speed = 2;

    public MeshRenderer bomb;

    [SerializeField] ParticleSystem explosion = null;

    public SphereCollider body;

    void Start()
    {
        bombExplode = GetComponent<AudioSource>();
        bomb = GetComponent<MeshRenderer>();
        body = GetComponent<SphereCollider>();
        StartCoroutine(waiter());

    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3f);
        bombExplode.Play();
        yield return new WaitForSeconds(0.2f);
        bomb.enabled = !enabled;
        explosion.Play();
        Explode(range, speed);
        yield return new WaitForSeconds(0);
        body.enabled = !enabled;

    }

    void Explode(int range, float speed)
    {

    }
}
