using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Classe respons�vel por controlar qualquer tipo de �udio
    private AudioSource audioSource;

    // AudioClip � o arquivo de audio que ser� executado
    public AudioClip levelMusic;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ao iniciar o MusicController, inicia a m�sica da fase
        PlayMusic(levelMusic);
    }

    public void PlayMusic(AudioClip music)
    {
        // Define o som que ir� ser reproduzido
        audioSource.clip = music;

        // Reproduz o som
        audioSource.Play();
    }
}
