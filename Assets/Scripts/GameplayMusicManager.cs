using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicManager : MonoBehaviour
{
    #region Singleton
    private static GameplayMusicManager _instance;
    public static GameplayMusicManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        else
            _instance = this;
    }
    #endregion

    [Header("Source")]
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource zombiesSound;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource rockCollide;
    [Header("Effect")]
    [SerializeField] private AudioClip boom;
    [SerializeField] private AudioClip humanIntoZombie;
    [SerializeField] private AudioClip henshin;
    [SerializeField] private AudioClip carExplode;
    [SerializeField] private AudioClip goldenize;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StopBGMandZombie()
    {
        BGM.Stop();
        zombiesSound.Stop();
    }

    public void PlayBGMandZombie()
    {
        BGM.Play();
        zombiesSound.Play();
    }
    public void PauseBGMandZombie()
    {
        BGM.Pause();
        zombiesSound.Pause();
    }

    public void PlayBoomSound()
    {
        soundEffect.clip = boom;
        soundEffect.Play();
    }
    public void PlayHumanIntoZombieSound()
    {
        soundEffect.clip = humanIntoZombie;
        soundEffect.Play();
    }
    public void PlayJumpSound()
    {
        jumpSound.Play();
    }
    public void PlayTransformSound()
    {
        rockCollide.Play();
        soundEffect.clip = henshin;
        soundEffect.PlayDelayed(1f);
    }
    public void PlayCarExplodeSound()
    {
        soundEffect.clip = carExplode;
        soundEffect.Play();
    }
    public void PlayGoldenizeSound()
    {
        soundEffect.clip = goldenize;
        soundEffect.Play();
    }
}
