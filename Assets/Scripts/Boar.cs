using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Boar : MonoBehaviour
{
    public bool isFlipped;
    public GameObject bloodPar;
    public SpriteRenderer renderer;
    public int startingHealth = 2;
    public int health;
    public float speed = 1f;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void Update()
    {
        if (isFlipped)
            transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
        else
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flipper"))
        {
            isFlipped = !isFlipped;
            renderer.flipX = isFlipped;
            Debug.Log("flipped");
        }
    }

    public void DecreaseHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        GameObject mypar = Instantiate(bloodPar, transform);
        Destroy(mypar, 3);
        mypar.transform.SetParent(null);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        health = startingHealth;
    }

}
