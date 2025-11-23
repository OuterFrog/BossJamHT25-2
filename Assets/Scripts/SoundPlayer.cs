using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void PlaySound(AudioClip clip, float volume = 1)
    {
        AudioSource src = GetComponent<AudioSource>();
        src.clip = clip;
        src.volume = volume;
        src.Play();
        Invoke(nameof(DestroySelf), clip.length + 0.5f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
