using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public Transform respawnPoint;
    public int lives;
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
    public TMP_Text livesText;
    public bool hungerSoundPlayed;
    public AudioSource source => GetComponent<AudioSource>();
    public AudioClip hungerSound;
    public AudioClip eatingSound;
    public Animator anim;
    public GameObject variablesParents;

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
        livesText.text = "Lives: " + lives;

        if (hunger < 20 && !hungerSoundPlayed)
        {
            source.PlayOneShot(hungerSound);
            hungerSoundPlayed = true;
        }
        if (hunger > 20)
        {
            hungerSoundPlayed = false;
        }

        if (lives <= 0)
        {
            die();
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
            lives--;
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
        lives = 3;
        transform.position = respawnPoint.position;
        rb.velocity = Vector2.zero;
        hunger = 50;

        for (int i = 0; i < variablesParents.transform.childCount; i++)
        {
            for (int j = 0; j < variablesParents.transform.GetChild(i).childCount; j++)
            {
                variablesParents.transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                variablesParents.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);

            }
        }
        hasSlingShot = false;
        transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }
}
