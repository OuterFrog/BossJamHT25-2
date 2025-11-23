using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public static SettingsScript singleton;

    public float mouseSensitivity;

    public Slider senseSlider;


    void Awake()
    {

        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            senseSlider.value = singleton.mouseSensitivity;
            senseSlider.onValueChanged.AddListener((value) => singleton.ChangeSense());
            singleton.senseSlider = senseSlider;
            Destroy(this);
        }

        senseSlider.value = mouseSensitivity;
    }

    public void ChangeSense()
    {
        mouseSensitivity = senseSlider.value;
        Debug.Log("new " + mouseSensitivity);
    }
}
