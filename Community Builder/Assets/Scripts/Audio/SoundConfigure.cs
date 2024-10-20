using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundConfigure : MonoBehaviour
{
    public SoundType soundType;
    [SerializeField] private bool destroyOnEnd = true;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        if (destroyOnEnd)
            Destroy(gameObject, audioSource.clip.length);
    }
}
