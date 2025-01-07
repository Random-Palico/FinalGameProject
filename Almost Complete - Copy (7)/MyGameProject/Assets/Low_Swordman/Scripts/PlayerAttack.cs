using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;

    private Rigidbody2D rb;

    public bool canReceiveInput;
    public bool inputReceived;
    public int damage;

    public Transform attackPoint;
    public float range;
    public LayerMask enemyLayer;

    public Transform bulletPoint;
    public GameObject bulletPrefab;
    public float timeStopDuration = 1.0f; // Duration to stop time before attack
    public int maxBullets = 3; // Maximum bullets that can be fired
    public float reloadTime = 5f; // Time in seconds to reload bullets

    private int currentBullets;
    private float nextFireTime;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        InputManager();
        Shoot();
    }

    public void isAttack()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.transform.position, range, enemyLayer);

        foreach (Collider2D enemy in enemiesHit)
        {
            // Check if the enemy has a BossHealth component
            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
            else
            {
                // If it's not a boss, handle other enemies if necessary
                Monster monster = enemy.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damage);
                }
                else
                {
                    FlyMonster fly = enemy.GetComponent<FlyMonster>();
                    if (fly != null)
                    {
                        fly.TakeDamage(damage);
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, range);
    }
    public void AddDamage(int damage)
    {
        this.damage += damage;
        Debug.Log($"New base damage: {damage}");
    }
    //Check if player had press mouse or not to set bool
    public void InputManager()
    {
        if (!inputReceived)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }
    }
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && canReceiveInput)
        {
            inputReceived = true;
            canReceiveInput = false;
        }
        else
        {
            return;
        }
    }
    public void Shoot()
    {
        if (Input.GetButton("Fire2") && Time.time >= nextFireTime)
        {
            if (currentBullets > 0)
            {
                Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
                currentBullets--;
                nextFireTime = Time.time + (reloadTime / maxBullets); // Set the next fire time
            }
            else
            {
                // Optional: Handle the case where no bullets are left
                Debug.Log("No bullets left! Wait for reload.");
            }
        }

        // Reload bullets every minute
        if (Time.time >= nextFireTime + reloadTime)
        {
            currentBullets = maxBullets; // Reset bullet count
            nextFireTime = Time.time; // Reset fire time
        }
    }

}
