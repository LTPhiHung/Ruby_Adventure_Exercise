using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool vertical;
    private Rigidbody2D rb2d;
    public float speed = 2.5f;
    public int direction = 1;
    private int directionDefault = 1;
    float movingTimer;
    public float movingTime = 1.5f;
    Animator animator;
    bool broken = true;
    public ParticleSystem smokeEffect;
    public AudioClip fixSound;
    AudioSource audioSource;
    bool wasBroken = true;
    float timeSinceFixed = 0f;
    public float timeUntilReset = 10f;
    Vector3 initialPosition;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position;
        movingTimer = movingTime;
        directionDefault = direction;
    }

    void Update() 
    {
         if (!broken) {
            if (wasBroken) {
                timeSinceFixed += Time.deltaTime;

                if (timeSinceFixed >= timeUntilReset) {
                    ResetEnemy();
                }
            }
            return;
        }

        movingTimer -= Time.deltaTime;
        if(movingTimer < 0)
        {
            direction *= -1;
            movingTimer = movingTime;
        }

        if(vertical)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else 
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
    }
    
    void FixedUpdate()
    {
         if(!broken) {
            return;
        }

        Vector3 position = rb2d.position;
        if(vertical) {
            position.y += speed * Time.deltaTime * direction;
        } else {
            position.x += speed * Time.deltaTime * direction;
        }
        rb2d.MovePosition(position);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        RubyController controller = collision.GetComponent<RubyController>();
        if(controller != null) {
            controller.changeHealth(-1);
            controller.PlaySound(controller.hitSound);
        }
    
    }

    void ResetEnemy() {
        // Thiết lập lại các giá trị mặc định sau khi sửa
        broken = true;
        wasBroken = false;
        movingTimer = movingTime;
        direction = directionDefault;
        timeSinceFixed = 0f;
        rb2d.simulated = true;
        smokeEffect.Play();
        transform.position = initialPosition;
    }

    public void Fix()
    {
        broken = false;
        wasBroken = true;
        rb2d.simulated = false;
        smokeEffect.Stop();
        animator.SetTrigger("Fixed");
        PlaySound(fixSound);
    }

      public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
