using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public int damage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        // Ao colidir com o player
        if (player)
        {
            // O player toma dano
            player.TakeDamage(damage);

            // O projetil � destruido
            Destroy(this.gameObject);
        }

        // Destruir o projetil ao colidir com os limites da fase (Left e Right)
        if (collision.CompareTag("Wall"))
        {
            // O projetil � destruido
            Destroy(this.gameObject);
        }
    }
}
