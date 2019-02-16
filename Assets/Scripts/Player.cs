using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Configuration Parameters
    [Header("Game")]
    [SerializeField] GameObject gameManager;
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] float health = 200f;
    [SerializeField] List<AudioClip> explosionSounds;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [Header("Projectile")]
    [SerializeField] GameObject playerLaser;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] List<AudioClip> weaponsFire;
    [SerializeField] [Range(0, 1)] float weaponsVolume = 0.75f;


    float xMin, xMax, yMin, yMax;
    bool canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    public float GetHealth()
    {
        return health;
    }

    private void Fire()
    {
        if (Input.GetButton("Fire1"))
        {
            if (canFire)
            {
                canFire = false;
                GameObject laser = Instantiate(playerLaser, transform.position, Quaternion.identity) as GameObject;
                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
                AudioClip clip = weaponsFire[UnityEngine.Random.Range(0, weaponsFire.Count)];
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, weaponsVolume);
                StartCoroutine(FireContinuously());
            }
            
        }
    }

    IEnumerator FireContinuously()
    {
        yield return new WaitForSeconds(projectileFiringPeriod);
        canFire = true;
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
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
        AudioClip clip = explosionSounds[UnityEngine.Random.Range(0, explosionSounds.Count)];
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, deathSoundVolume);
        Destroy(gameObject);
        FindObjectOfType<Level>().LoadGameOver();
    }
}
