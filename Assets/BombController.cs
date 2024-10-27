using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public AudioSource bombTicking;
    public AudioSource bombExplode;

    void Update()
    {
        bombTicking = GetComponent<AudioSource>();
        bombExplode = GetComponent<AudioSource>();
        StartCoroutine(waiter());
    }
    IEnumerator waiter()
    {
        bombTicking.Play(0);
        yield return new WaitForSeconds(5);
        bombExplode.Play(0);
    }
}
