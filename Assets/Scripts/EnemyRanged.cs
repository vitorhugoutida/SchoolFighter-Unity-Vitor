using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private bool facingRight;
    private bool previousDirectionRight;

    private bool isDead;

    private Transform target;

    private float enemySpeed = 0.3f;
    private float currentSpeed;

    private float verticalForce, horizontalForce;

    private bool isWalking = false;

    private float walkTimer;

    public int maxHealth;
    public int currentHealth;

    private float staggerTime = 0.5f;
    private bool isTakingDamage = false;
    private float damageTimer;

    private float attackRate = 1f;
    private float nextAttack;

    public Sprite enemyImage;

    // Variavel para armazenar o projetil
    public GameObject projectile;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        target = FindAnyObjectByType<PlayerController>().transform;

        currentSpeed = enemySpeed;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        walkTimer += Time.deltaTime;

        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        if (isTakingDamage && !isDead)
        {
            damageTimer += Time.deltaTime;

            ZeroSpeed();

            if (damageTimer >= staggerTime)
            {
                isTakingDamage = false;
                damageTimer = 0;

                ResetSpeed();
            }
        }

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isWalking", isWalking);
    }

    private void ResetSpeed()
    {
        currentSpeed = enemySpeed;
    }

    private void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    public void DisableEnemy()
    {
        this.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        isTakingDamage = true;

        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        FindFirstObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

        if (currentHealth <= 0)
        {
            isDead = true;

            // Corrige o bug do inimgo deslizar após morto
            rb.linearVelocity = Vector2.zero;

            animator.SetTrigger("Dead");
        }
    }

    public void FixedUpdate()
    {
        if (!isDead)
        {
            Vector3 targetDistance = target.position - this.transform.position;

            if (walkTimer >= Random.Range(2.5f, 3.5f))
            {
                verticalForce = targetDistance.y / Mathf.Abs(targetDistance.y);
                horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

                walkTimer = 0;
            }

            if (Mathf.Abs(targetDistance.x) < 1f)
            {
                horizontalForce = 0;
            }

            if (Mathf.Abs(targetDistance.y) < 0.05f)
            {
                verticalForce = 0;
            }

            if (!isTakingDamage)
            {
                rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);
            }

            // Lógica do ataque
            if (Mathf.Abs(targetDistance.x) < 1.3f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
            {
                // Ataque do inimigo
                animator.SetTrigger("Attack");
                ZeroSpeed();

                nextAttack = Time.time + attackRate;
            }
        }
    }

    public void Shoot()
    {
        // Define a posição de spawn do projetil
        Vector2 spawnPosition = new Vector2(this.transform.position.x, this.transform.position.y + 0.2f);

        // Spawnar o projetil na posição definida
        GameObject shotObject = Instantiate(projectile, spawnPosition, Quaternion.identity);

        // Ativar o projetil
        shotObject.SetActive(true);

        var shotPhysics = shotObject.GetComponent<Rigidbody2D>();

        if (facingRight)
        {
            // Aplica força no projétil para ele se deslocar para a direita
            shotPhysics.AddForceX(80f);
        }
        else
        {
            // Aplica força no projétil para ele se deslocar para a esquerda
            shotPhysics.AddForceX(-80f);
        }
    }
}
