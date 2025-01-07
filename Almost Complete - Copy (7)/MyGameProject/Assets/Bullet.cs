using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        StartCoroutine(DestroyAfterTime(5f)); // Start the coroutine to destroy the bullet after 5 seconds
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Monster monster = hitInfo.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(damage);
        }
        Destroy(gameObject);

        FlyMonster fly = hitInfo.GetComponent<FlyMonster>();
        if (fly != null)
        {
            fly.TakeDamage(damage);
        }
        Destroy(gameObject);

        BossHealth bs = hitInfo.GetComponent<BossHealth>();
        if (bs != null)
        {
            bs.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // Wait for the specified time
        Destroy(gameObject); // Destroy the bullet
    }
}
