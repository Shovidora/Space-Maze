using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton

    [Header("General:")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    [Space(5)]
    public AudioClip backgroundMusic;
    public AudioClip buttonClickSound;

    [Header("Other Source:")]
    public AudioSource movementSource;
    public AudioClip movementShieldOnLoopClip;

    [Header("Player:")]
    public AudioClip playerJump;
    public AudioClip playerHit;
    public AudioClip playerChangeAbility;
    public AudioClip playerSwingSword;
    // public AudioClip playerShootBullet; // for now general sound
    public AudioClip playerSwingShield;
    // public AudioClip playerWalkingShieldOn; // [Header("Other Source:")]
    // public AudioClip playerShieldOnIdle; // not in use
    public AudioClip playerInstWallSimple;
    // public AudioClip playerInstWallBig; // for now general sound - playerInstWallSepcial
    // public AudioClip playerInstWallTranspernt; // for now general sound - playerInstWallSepcial
    public AudioClip playerInstWallSepcial;

    [Header("Walls:")]
    public AudioClip wallHit;
    public AudioClip wallDestroy;
    public AudioClip unremoveableWallHit;

    [Header("Bullet:")]
    public AudioClip bulletFired;
    public AudioClip bulletDestroy;

    [Header("Enemy:")]
    public AudioClip enemyDie;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Saving between scences
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    private void Update()
    {
        if (Cursor.visible == true && Input.GetKeyDown(KeyCode.Mouse0))
            PlaySFX(buttonClickSound);
    }


    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    /// Player:
    public void PlayPlayerChangeAbility()
    {
        PlaySFX(playerChangeAbility);
    }
    public void PlayPlayerJump()
    {
        PlaySFX(playerJump);
    }
    public void PlayPlayerHitByBullet()
    {
        PlaySFX(playerHit);
    }
    // Ability:
    public void PlayPlayerInstWallSimple()
    {
        PlaySFX(playerInstWallSimple);
    }
    public void PlayplayerInstWallSpecial()
    {
        PlaySFX(playerInstWallSepcial);
    }
    public void PlayPlayerSwingSword()
    {
        PlaySFX(playerSwingSword);
    }
    public void PlayPlayerSwingShield()
    {
        PlaySFX(playerSwingShield);
    }
    // ;
    /// ;

    /// Walls:
    public void PlayWallHit()
    {
        PlaySFX(wallHit);
    }
    public void PlayUnremoveableWallHit()
    {
        PlaySFX(unremoveableWallHit);
    }
    /// ;


    /// Other Source:
    public void PlayMovementShieldOnLoop()
    {
        if (!movementSource.isPlaying)
        {
            movementSource.clip = movementShieldOnLoopClip;
            movementSource.loop = true;
            movementSource.Play();
        }
    }
    public void StopMovementShieldOnLoop()
    {
        if (movementSource.isPlaying)
        {
            movementSource.Stop();
        }
    }

    public static void PlayClipAtPointWithVolume(AudioClip clip, Vector3 position, float volume)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.spatialBlend = 1f;
        aSource.Play();
        Destroy(tempGO, clip.length);
    }

}
