using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 1;

    private bool _isAttacking;
    private Animator _animator;
    private AudioSource _audio;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        //Animator
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
            _isAttacking = true;
            //_audio.Play();
}
        else {
            _isAttacking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isAttacking == true)
        {
            if(collision.CompareTag("Enemy")){
                collision.SendMessageUpwards("AddDamage", damage);
            }

            if (collision.CompareTag("BigBullet")){
                collision.SendMessageUpwards("ParryBullet");
            }
        }
    }

    public void playAttackingAudio()
    {
        _audio.Play();
    }
}
