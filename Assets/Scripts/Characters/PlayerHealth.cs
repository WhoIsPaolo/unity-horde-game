using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int totalHealth = 5;
    public RectTransform heartUI;
    public Transform startingPoint;

    //Game Over
    public RectTransform gameOverMenu;
    public GameObject hordes;

    private int _health;
    private float _heartSize = 15.66667f;

    private SpriteRenderer _renderer;
    private Animator _animator;
    private PlayerController _controller;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _health = totalHealth;
    }

    public void AddDamage(int amount)
    {
        _health -= amount;

        //Visual Feedback
        StartCoroutine("VisualFeedback");

        //Game Over
        if(_health <= 0){
            _health = 0;
            gameObject.SetActive(false);
        }

        heartUI.sizeDelta = new Vector2(_heartSize * _health, _heartSize);
        //Debug.Log("Player got some damaged. His current health is " + _health);
    }

    public void AddHealth(int amount)
    {
        _health += amount;

        //Max Health
        if (_health > totalHealth)
        {
            _health = totalHealth;
        }

        heartUI.sizeDelta = new Vector2(_heartSize * _health, _heartSize);
        //Debug.Log("Player got some health. His current health is " + _health);
    }

    private IEnumerator VisualFeedback()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _renderer.color = Color.white;
    }

    private void OnEnable()
    {
        _health = totalHealth;
        gameObject.transform.position = startingPoint.position;
    }

    private void OnDisable()
    {
        gameOverMenu.gameObject.SetActive(true);
        hordes.SetActive(false);
        _animator.enabled = false;
        _controller.enabled = false;
    }
}
