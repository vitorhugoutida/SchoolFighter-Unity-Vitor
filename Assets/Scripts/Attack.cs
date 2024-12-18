using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    private AudioSource audioSource;
    public AudioClip hitSound;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ao colidir, salva na variavel enemy, o inimigo que foi colidido
        EnemyMeleeController enemy = collision.GetComponent<EnemyMeleeController>();

        // ao colidir, salva na variavel player, o player que foi atingido
        PlayerController player = collision.GetComponent<PlayerController>();

        EnemyRanged enemyRanged = collision.GetComponent<EnemyRanged>();

        if (enemyRanged != null)
        {
            // Inimigo recebe dano
            enemyRanged.TakeDamage(damage);

            audioSource.clip = hitSound;
            audioSource.Play();
        }


        // Se a colisão foi com um inimigo
        if (enemy != null)
        {
            // Inimigo recebe dano
            enemy.TakeDamage(damage);

            audioSource.clip = hitSound;
            audioSource.Play();
        }

        // Se a colisão foi com um player
        if (player != null)
        {
            // Player recebe dano
            player.TakeDamage(damage);

            audioSource.clip = hitSound;
            audioSource.Play();
        }
    }

}
