using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float longIdleTime = 5f;
    public float speed = 2.5f;
    public float jumpForce = 2.5f;
   
    //Variables for Jump
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    //LongIdle
    private float _longIdleTimer;
    //Movement
    private Vector2 _movement;
    private bool _facingRight = true;
    private bool _isGrounded;

    //Attack
    private bool _isAttacking;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update es el mejor momento para capturar entradas de teclas.
    void Update()
    {
        //---MOVEMENT---
        if (_isAttacking == false) 
        {
            //float horizontalInput = Input.GetAxis("Horizontal"); //Devuelve el valor del movimiento horizonal desde -1 a 1
            float horizontalInput = Input.GetAxisRaw("Horizontal"); //Devuelve el valor del movimiento horizonal final -1 o 1
            _movement = new Vector2(horizontalInput, 0f);

            //Flip Character
            if (horizontalInput < 0f && _facingRight == true)
            {
                Flip();
            }
            else if (horizontalInput > 0f && _facingRight == false)
            {
                Flip();
            }
        }
        
        //isGrounded?
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //isJumping?
        if (Input.GetButtonDown("Jump") && _isGrounded == true && _isAttacking == false){
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //Wanna Attack?
        if(Input.GetButtonDown("Fire1") && _isGrounded == true && _isAttacking == false){
            _movement = Vector2.zero;
            _rigidbody.velocity = Vector2.zero;
            _animator.SetTrigger("attack");
        }
    }

    // FixedUpdate es el mejor momento para mover el rigidbody
    private void FixedUpdate()
    {
        if (_isAttacking == false)
        {
            float horizontalVelocity = _movement.normalized.x * speed;
            _rigidbody.velocity = new Vector2(horizontalVelocity, _rigidbody.velocity.y);
        }
    }

    // LateUpdate es el mejor momento para tratar con animaciones
    private void LateUpdate()
    {
        _animator.SetBool("idle", _movement == Vector2.zero);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("verticalVelocity", _rigidbody.velocity.y);

        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
            _isAttacking = true;
        }else{
            _isAttacking = false;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")){
            _longIdleTimer += Time.deltaTime;
            if (_longIdleTimer >= longIdleTime){
                _animator.SetTrigger("longIdle");
            }
        }else{
            _longIdleTimer = 0f;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        // Se modifica LocalScaleX para cambiar de sentido la animación del personaje
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
}
