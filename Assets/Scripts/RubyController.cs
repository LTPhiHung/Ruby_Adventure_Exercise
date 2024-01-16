using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class RubyController : MonoBehaviour
{
    // Start is called before the first frame update
    float horizontal;
    float vertical;
    private Rigidbody2D rb2d;
    public int maxHealth = 5;
    int currentHealth;
    public float speed = 2.5f;
    public int health {
        get {return currentHealth;}
        set {currentHealth = value;} 
    }    

    // Time for damage
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    // Look direction vector
    Vector2 lookDirection = new Vector2(0, 0);
    // Animator property
    Animator animator;
    public GameObject cogBulletPrefab;
    public AudioClip hitSound;
    public AudioClip throwCog;
    public AudioClip moveSound;
    public AudioSource audioSource;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(isInvincible) 
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0) 
            {
                isInvincible = false;
            }
        }

        Vector2 move = new Vector2(horizontal, vertical);
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
            PlaySound(throwCog);

        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider != null)
            {
                MyNPC character = hit.collider.GetComponent<MyNPC>();
                if(character != null)
                {
                    character.ShowDialog("Hello Ruby!");
                }
                Debug.Log("Hit " + hit.collider.name );
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.x += (speed * horizontal * Time.deltaTime);
        position.y += (speed * vertical * Time.deltaTime);
        rb2d.MovePosition(position);
    }

    public void changeHealth(int amount) {
        if(amount < 0)
        {
            animator.SetTrigger("Hit");
            if(isInvincible) 
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
            StartCoroutine(DelayedSound());
        }
        currentHealth = Math.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    IEnumerator DelayedSound()
    {
        PlaySound(hitSound);
        yield return new WaitForSeconds(1f);
    }
    void Launch() {
        GameObject cogBulletObject = Instantiate(cogBulletPrefab, rb2d.position + Vector2.up * 0.5f, Quaternion.identity);
        CogBullet cogBullet = cogBulletObject.GetComponent<CogBullet>();
        cogBullet.Launch(lookDirection, 300f);

        animator.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
