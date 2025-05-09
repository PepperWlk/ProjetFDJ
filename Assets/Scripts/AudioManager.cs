using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip sound;

    [Range(0,1f)]
    public float volume;

    [Range(0.1f, 2.5f)]
    public float pitch;

    private AudioSource source;

    void Awake()
    {
        
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
