using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager singleton;

    [SerializeField] GameObject soundPlayerPrefab;

    [SerializeField] AudioClip[] audioClips;

    private void Awake() {
        singleton = this;
    }

    public void PlaySound(int clipID)
    {
        GameObject newSound = Instantiate(soundPlayerPrefab);
        newSound.GetComponent<SoundPlayer>().PlaySound(audioClips[clipID]);
    }

    public void PlayOneOf(params int[] clipIDs)
    {
        PlaySound(clipIDs[Random.Range(0, clipIDs.Length - 1)]);
    }

    public void PlaySound(float volume, int clipID)
    {
        GameObject newSound = Instantiate(soundPlayerPrefab);
        newSound.GetComponent<SoundPlayer>().PlaySound(audioClips[clipID], volume);
    }

    public void PlayOneOf(float volume, params int[] clipIDs)
    {
        PlaySound(volume, clipIDs[Random.Range(0, clipIDs.Length - 1)]);
    }
}
