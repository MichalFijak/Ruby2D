using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    Rigidbody2D rubyRigidbody;
    Animator animator;
    AudioSource audioSource;

    public GameObject projectilePrefab;
    Vector2 lookDirection = new Vector2(1, 0);
    float vertical;
    float horizontal;
    float speed = 5.0f;

    public int maxHealth = 5;
    private int currentHealth;
    public float timeInvincible = 2.0f;
    public bool isInvincble;
    private float invincibleTimer;

    public int health { get { return currentHealth; } }
    // Start is called before the first frame update
    void Start()
    {
        rubyRigidbody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        if (isInvincble)
        {
            invincibleTimer -= Time.deltaTime;
            isInvincble = false;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if(Input.GetKeyDown(KeyCode.X)) //talking with!
        {
            RaycastHit2D hit = Physics2D.Raycast(rubyRigidbody.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            NPC character = hit.collider.GetComponent<NPC>();
            if (hit.collider!=null)
            {

                character.DisplayDialog();

            }

        }
    }
    private void FixedUpdate()
    {
        Vector2 position = rubyRigidbody.position;
        position.x = position.x + horizontal * speed * Time.deltaTime;
        position.y = position.y + vertical * speed * Time.deltaTime;
        rubyRigidbody.MovePosition(position);
    }
    public void ChangeHealth(int amount)
    {
        if (amount < -0)
        {
            if (isInvincble)
                return;
            isInvincble = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UiHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rubyRigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");



    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);

    }
}
