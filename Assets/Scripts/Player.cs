using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public Transform respawnPoint;
    public float hunger;
    public float maxHunger = 100;
    public float hungerMarkiplier = 1.5f;
    public bool hasSlingShot;
    public bool canShoot;
    public int bulletsLeft;
    public float slingForce = 10;
    public float slingCooldown = 1;
    public GameObject bullet;
    public TMP_Text hungerText;
    public bool hungerSoundPlayed;
    public AudioSource source => GetComponent<AudioSource>();
    public AudioClip hungerSound;
    public AudioClip eatingSound;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("speedMarkiplier", rb.velocity.x / 5);

        if (hunger > maxHunger)
        {
            hunger = maxHunger;
        }
        else if (hunger <= 0)
        {
            die();
        }
        hunger -= Time.deltaTime * hungerMarkiplier;
        hungerText.text = Mathf.RoundToInt(hunger).ToString() + "%";

        if (hunger < 20 && !hungerSoundPlayed)
        {
            source.PlayOneShot(hungerSound);
            hungerSoundPlayed = true;
        }
        if (hunger > 20)
        {
            hungerSoundPlayed = false;
        }

        if (Input.GetMouseButtonDown(0) && hasSlingShot && canShoot && bulletsLeft > 0)
        {
            canShoot = false;
            ShootSling();
        }
    }

    void ShootSling()
    {
        GameObject mybullet = Instantiate(bullet, transform);
        if (GetComponent<Movement>().renderer.flipX)
        {
            mybullet.GetComponent<Rigidbody2D>().AddForce(-transform.right * slingForce + transform.up * (slingForce / 4));
        }
        else
        {
            // mybullet.GetComponent<SpriteRenderer>().flipX = true;
            mybullet.GetComponent<Rigidbody2D>().AddForce(transform.right * slingForce + transform.up * (slingForce / 4));
        }
        Destroy(mybullet, 3f);

        StartCoroutine(ResetcanShoot());
    }

    IEnumerator ResetcanShoot()
    {
        yield return new WaitForSeconds(slingCooldown);
        canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike") || other.CompareTag("Enemy"))
        {
            die();
        }

        if (other.CompareTag("Scrumptious"))
        {
            hunger += 50;
            other.gameObject.SetActive(false);
            source.PlayOneShot(eatingSound);
        }

        if (other.CompareTag("Exit"))
        {
            SceneManager.LoadScene("Level2");
        }
        if (other.CompareTag("Slingshot"))
        {
            hasSlingShot = true;
            transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            other.gameObject.SetActive(false);
        }

    }

    void die()
    {
        transform.position = respawnPoint.position;
        rb.velocity = Vector2.zero;
        hunger = 50;
    }
}
