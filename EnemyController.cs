using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D enemyRigidBody;
    Animator animator;
    public ParticleSystem smokeEffect;

    private float speed = 2.0f;
    public bool verticalEnemy;
    public float changeTime = 3.0f;
    float timer;
    int direction = 1;
    bool broken = true;
    // Start is called before the first frame update
    void Start()
    {
        
        enemyRigidBody = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer<0)
        {
            direction = -direction;
            timer = changeTime;
        }
        if(!broken)
        {
            return;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = enemyRigidBody.position;
        if (verticalEnemy)
        {
            position.y = position.y + Time.deltaTime * speed*direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + Time.deltaTime * speed*direction;
        }
        enemyRigidBody.MovePosition(position);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();
        if(player!=null)
        {
            player.ChangeHealth(-1);
        }
    }
    public void Fix()
    {
        broken = false;
        enemyRigidBody.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }
}
