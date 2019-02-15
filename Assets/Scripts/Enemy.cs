using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float projectileSpeed = 5.0f;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] float durationOfExplosion = 1f;

    // Vector3 projectileOffset = new Vector3(0f, -0.5f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
        
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject explosion = Instantiate(explosionParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, durationOfExplosion);
        Destroy(gameObject);
    }
}
