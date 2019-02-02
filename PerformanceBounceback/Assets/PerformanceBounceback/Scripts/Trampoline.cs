using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {
    private ParticleSystem pSystem;
    private AudioSource audioSource;
    private string throwableTag = "Throwable";
    public GameManager gameManager;
    public AudioClip ballHitSound;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        pSystem = GetComponentInChildren<ParticleSystem>();
	}
	
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(throwableTag))
        {
            gameManager.IncrementScore();

            pSystem.Play();

            audioSource.PlayOneShot(ballHitSound);
        }
    }
}
