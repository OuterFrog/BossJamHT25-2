using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public static SettingsScript singleton;

    public float mouseSensitivity;


    void Awake()
    {

        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        FindAnyObjectByType<Slider>().value = mouseSensitivity;
    }

    void Update()
    {
        mouseSensitivity = FindAnyObjectByType<Slider>().value;
                Debug.Log("new " + mouseSensitivity);
    }
}
