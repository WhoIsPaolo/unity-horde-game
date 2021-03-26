using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{
    // Start is called before the first frame update
    public float speed = 2f;
    public Vector2 direction;
    public int damage = 1;

    public float livingTime = 3f;
    public Color initialColor = Color.white;
    public Color finalColor;

    private SpriteRenderer _renderer;
    private float _startingTime;
    private Rigidbody2D _rigidbody;
    private bool _returning;

    private void Awake(){
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start(){
        //---Save initial time---
        _startingTime = Time.time;

        //---Destroy the bullet  after some time---
        Destroy(this.gameObject, livingTime);
    }

    // Update is called once per frame
    void Update(){
        //---Change Bullet Color Over Time---
        float _timeSinceStarted = Time.time - _startingTime;
        float _percentageCompleted = _timeSinceStarted / livingTime;
        _renderer.color = Color.Lerp(initialColor, finalColor, _percentageCompleted); //Linear Interpolation

        //---Move Object With Transform---
        //Vector2 movement = direction.normalized * speed * Time.deltaTime;
        //transform.position = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);
        //transform.Translate(movement);
    }

    private void FixedUpdate()
    {
        //---Move Object With Rigidbody---
        Vector2 movement = direction.normalized * speed;
        _rigidbody.velocity = movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_returning == false && collision.CompareTag("Player")) {
            // Tell Player to get hurt
            //collision.gameObject.GetComponent<PlayerHealth>().AddDamage(damage); //Es correcto, pero se debe saber el componente exacto del elemento.
            collision.SendMessageUpwards("AddDamage", damage); //Es correcto, pero no es necesario saber el componente exacto.

            Destroy(this.gameObject);
        }

        if (_returning == true && collision.CompareTag("Enemy")){
            collision.SendMessageUpwards("AddDamage", damage);
            Destroy(gameObject);
        }
    }

    public void ParryBullet(){
        _returning = true;
        direction = direction * -1f;
    }
}
