using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boolet : MonoBehaviour
{
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public GameObject rockBreakPar;

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Boar>().DecreaseHealth(1);
            Destroy(gameObject);

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            GameObject myPar = Instantiate(rockBreakPar, transform.position, Quaternion.identity);
            myPar.transform.rotation = Quaternion.Euler(other.contacts[0].normal);
            Destroy(gameObject);

        }

    }
}
