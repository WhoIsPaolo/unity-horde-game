using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldEnemyPatrol : MonoBehaviour
{
    public float speed = 1f;
    public float maxX;
    public float minX;
    public float waitingTime = 2f;

    private GameObject _target;
    private Animator _animator;
    private Weapon _weapon;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _weapon = GetComponentInChildren<Weapon>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTarget();
        StartCoroutine("PatrolToTarget");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateTarget()
    {
        //If first time, create target in the left side
        if (_target == null)
        {
            _target = new GameObject("Target");
            _target.transform.position = new Vector2(minX, transform.position.y);
            transform.localScale = new Vector3(-1, 1, 1);
            return;
        }

        //If we are in the left, change the target to the right
        if (_target.transform.position.x == minX)
        {
            _target.transform.position = new Vector2(maxX, transform.position.y);
            transform.localScale = new Vector3(1, 1, 1);
        }

        //If we are in the right, change the target to the left
        else if (_target.transform.position.x == maxX)
        {
            _target.transform.position = new Vector2(minX, transform.position.y);
            transform.localScale = new Vector3(-1, 1, 1);
            //Debug.Log("LocalScale: " + transform.localScale);
        }
    }

    //---Coroutine to move the enemy---
    //private IEnumerator PatrolToTarget() 
    IEnumerator PatrolToTarget()
    {
        while (Vector2.Distance(transform.position, _target.transform.position) > 0.05f)
        {
            //Update animator walking
            _animator.SetBool("idle", false);
            //let's move to the target
            Vector2 direction = _target.transform.position - transform.position;
            //float xDirection = direction.x;

            transform.Translate(direction.normalized * speed * Time.deltaTime);

            yield return null; //IMPORTANT
        }

        //At this point, i've reached the target, let's set our position to the target's one
        transform.position = new Vector2(_target.transform.position.x, transform.position.y);
        //Update animator idle
        _animator.SetBool("idle", true);
        UpdateTarget();

        _animator.SetTrigger("shoot");

        //And let's wait for a moment
        yield return new WaitForSeconds(waitingTime); //IMPORTANT

        //Once waited, let's restore the patrol behaviour
        StartCoroutine("PatrolToTarget");
    }

    void CanShoot()
    {
        if (_weapon != null)
        {
            _weapon.Shoot();
        }
    }
}
