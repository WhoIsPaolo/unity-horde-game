using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int totalHealth;

    private int _health;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
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
        if (_health <= 0)
        {
            _health = 0;
            gameObject.SetActive(false);
        }
    }

    private IEnumerator VisualFeedback()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _renderer.color = Color.white;
    }

    public void RestoreEnemyHealth()
    {
        _health = totalHealth;
        _renderer.color = Color.white;
    }
}
