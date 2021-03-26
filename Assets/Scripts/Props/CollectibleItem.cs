using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public GameObject lightingParticles;
    public GameObject burstParticles;
    public int healthBonus = 1;

    private SpriteRenderer _renderer;
    private Collider2D _collider;
    
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("health bonus: " + healthBonus);
            //Tell player to add some health
            collision.SendMessageUpwards("AddHealth", healthBonus);

            //Disable Collider
            _collider.enabled = false;

            //Visual Effects
            _renderer.enabled = false;
            lightingParticles.SetActive(false);
            burstParticles.SetActive(true);

            Destroy(gameObject, 2f);
        }
    }
}
