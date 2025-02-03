



using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioMixer : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] Slider AudioSlider;

    const string MIXER_SOUND = "AllVolume";
    private void Awake()
    {
        AudioSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        //mixer.SetFloat(MIXER_SOUND, Mathf.Log10(value) * 20);
    }
}
