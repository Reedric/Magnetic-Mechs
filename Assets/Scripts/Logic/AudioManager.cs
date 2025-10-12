using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private AudioMixer mixer;
    private AudioMixerGroup sfxGroup;
    private AudioMixerGroup musicGroup;
    private AudioSource[] allSources;
    private bool volumeLoaded;

    void Start()
    {
        PlayerPrefs.DeleteKey("audioVolume");
        mixer = (AudioMixer) Resources.Load("AudioMixer/Volume Control");
        allSources = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        sfxGroup = mixer.FindMatchingGroups("SFX")[0];
        musicGroup = mixer.FindMatchingGroups("Music")[0];
        foreach (AudioSource source in allSources)
        {
            if (source.gameObject.tag != "MusicSource")
            {
                source.outputAudioMixerGroup = sfxGroup;
            }
            else
            {
                source.outputAudioMixerGroup = musicGroup;
            }
        }
        LoadVolume();
    }

    public void SetAudioVolume()
    {
        if (!volumeLoaded) {
            return;
        }

        float sfxVolume = sfxSlider.value;
        float musicVolume = musicSlider.value;
        mixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    private void LoadVolume()
    {
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 1.0f);
        }

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
        }

        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        volumeLoaded = true;
        SetAudioVolume();
    }
}
