using UnityEngine;
using UnityEngine.SceneManagement;

public class musicHandler : MonoBehaviour
{
    public AudioSource titleMusic;  // Scene 1
    public AudioSource lvl0Music;    // Scene 0
    public AudioSource winMusic;     // Scene 2

    private AudioSource currentMusic;
    private int lastScene = -1;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        PlayForScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForScene(scene.buildIndex);
    }

    void PlayForScene(int sceneIndex)
    {
        if (sceneIndex == lastScene) return;

        lastScene = sceneIndex;

        StopAllMusic();

        switch (sceneIndex)
        {
            case 1: // lvl0
                PlayMusic(lvl0Music);
                break;

            case 0: // menu
                PlayMusic(titleMusic);
                break;

            case 2: // win
                PlayMusic(winMusic);
                break;
        }
    }

    void PlayMusic(AudioSource music)
    {
        if (music == null) return;

        currentMusic = music;

        if (!music.isPlaying)
        {
            music.loop = true;
            music.Play();
        }
    }

    void StopAllMusic()
    {
        if (titleMusic) titleMusic.Stop();
        if (lvl0Music) lvl0Music.Stop();
        if (winMusic) winMusic.Stop();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}