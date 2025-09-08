using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private AudioMixer mixer;
    private AudioMixerGroup masterGroup;
    private AudioSource[] allSources;

    void Start()
    {
        PlayerPrefs.DeleteKey("audioVolume");
        mixer = (AudioMixer) Resources.Load("AudioMixer/Volume Control");
        allSources = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        masterGroup = mixer.FindMatchingGroups("Master")[0];
        foreach (AudioSource source in allSources)
        {
            source.outputAudioMixerGroup = masterGroup;
        }
        LoadVolume();
    }

    public void SetAudioVolume()
    {
        float volume = volumeSlider.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("audioVolume", volume);
    }

    private void LoadVolume()
    {
        if (!PlayerPrefs.HasKey("audioVolume"))
        {
            PlayerPrefs.SetFloat("audioVolume", 1);
        }

        volumeSlider.value = PlayerPrefs.GetFloat("audioVolume");

        SetAudioVolume();
    }
}
