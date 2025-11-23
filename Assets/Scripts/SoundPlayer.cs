using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void PlaySound(AudioClip clip)
    {
        AudioSource src = GetComponent<AudioSource>();
        src.clip = clip;
        src.Play();
        Invoke(nameof(DestroySelf), 2);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
