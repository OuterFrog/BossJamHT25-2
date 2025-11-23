using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public static SettingsScript singleton;

    public float mouseSensitivity;

    [SerializeField] Slider senseSlider;

    void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }

        senseSlider.value = mouseSensitivity;
    }

    public void ChangedSense()
    {
        mouseSensitivity = senseSlider.value;
    }
}
