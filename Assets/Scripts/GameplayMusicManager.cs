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
    [Header("Effect")]
    [SerializeField] private AudioClip collectCoin;
    [SerializeField] private AudioClip boom;
    [SerializeField] private AudioClip humanIntoZombie;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip henshin;
    [SerializeField] private AudioClip carExplode;
    [SerializeField] private AudioClip goldenize;

    //private Queue<AudioSource> coinSounds = new Queue<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCoinSound()
    {
        soundEffect.clip = collectCoin;
        soundEffect.Play();
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
        soundEffect.clip = jump;
        soundEffect.Play();
    }
    public void PlayTransformSound()
    {
        soundEffect.clip = henshin;
        soundEffect.Play();
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
