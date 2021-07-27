using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public Slider musicSlider;
    public Slider SFXSlider;
    public AudioMixer audioMixer;

    void Awake()
    {
        float ms, ss;
        audioMixer.GetFloat("MusicAttenuation", out ms);
        audioMixer.GetFloat("MusicAttenuation", out ss);

        musicSlider.value = PlayerPrefs.GetFloat("Music", ms);
        SFXSlider.value = PlayerPrefs.GetFloat("SFX", ss);
    }

    void Update()
    {
        audioMixer.SetFloat("MusicAttenuation", musicSlider.value);
        audioMixer.SetFloat("SFXAttenuation", SFXSlider.value);
    }

    public void SaveAndExit()
    {
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("SFX", SFXSlider.value);
    }
}
