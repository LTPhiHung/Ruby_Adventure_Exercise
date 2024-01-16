using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogBullet : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb2d;
    private BoxCollider2D collider2D;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider2D =GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude > 20)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rb2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log("123");
        EnemyController enemy = other.collider.GetComponent<EnemyController>();
        if (enemy != null) {
            enemy.Fix();
        }
        Destroy(gameObject);    
    }

     private void OnTriggerEnter2D(Collider2D collision) 
    {
        Debug.Log("456");   
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if(enemy != null) {
            enemy.Fix();
        }
        Destroy(gameObject);    
    }
}
