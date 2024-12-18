using Assets.Scripts;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyArray;

    public int numberOfEnemies;
    private int currentEnemies;

    public float spawnTime;

    public string nextSection;

    void Update()
    {
        // Caso atinja o número máximo de inimigos spawnados
        if (currentEnemies >= numberOfEnemies)
        {
            // Contar a quantidade de inimgos ativos na cena
            int enemies = FindObjectsByType<EnemyMeleeController>(FindObjectsSortMode.None).Length;

            if (enemies <= 0)
            {
                // Avança de seção
                LevelManager.ChangeSection(nextSection);

                // Desabilitar o spawner
                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemy()
    {
        // Posição de Spawn do inimigo
        Vector2 spawnPosition;

        // Limites Y
        // -0.36
        // -0.95
        spawnPosition.y = Random.Range(-0.95f, -0.36f);

        // Posição X máximo (direita) do confiner da camera + 1 de distancia
        // Pegar RightBound (limite direito) da Section (Confiner) como base
        float rightSectionBound = LevelManager.currentConfiner.BoundingShape2D.bounds.max.x;

        // Define o x do spawnPosition, igual ao ponto da DIREITA do confiner
        spawnPosition.x = rightSectionBound;

        // Instancia ("Spawna") os inimigos
        // Pega um inimigo aleatório da lista de inimigos
        // Spawna na posição spawnPosition
        // Quaternion é uma classe utilizada para trabalhar com rotações
        Instantiate(enemyArray[Random.Range(0, enemyArray.Length)], spawnPosition, Quaternion.identity).SetActive(true);

        // Incrementa o contador de inimigos do Spawner
        currentEnemies++;

        // Se o numero de inimigos atualmente na cena for menor que o numero máximo de inimgos,
        // Invoca novamente a função de spawn
        if (currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player)
        {
            // Desativa o colisor para iniciar o Spawning apenas uma vez
            // ATENÇÃO: Desabilita o collider, mas o objeto Spawner continua ativo
            this.GetComponent<BoxCollider2D>().enabled = false;

            // Invoca pela primeira vez a função SpawnEnemy
            SpawnEnemy();
        }
    }
}
