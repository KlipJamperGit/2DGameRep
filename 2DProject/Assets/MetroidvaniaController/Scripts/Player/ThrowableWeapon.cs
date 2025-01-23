using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
    public Vector2 direction;
    public bool hasHit = false;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // Mirror the object based on the direction
        if (direction.x < 0)
        {
            // Flip horizontally (mirror object)
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // Ensure it's mirrored
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // Ensure it's in the correct facing direction
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasHit)
        {
            GetComponent<Rigidbody2D>().linearVelocity = direction * speed; // Move the object
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
