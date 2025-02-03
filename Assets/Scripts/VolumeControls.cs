



using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeControls : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] Slider AudioSlider;

    const string MIXER_SOUND = "AllVolume";
    private void Awake()
    {
        AudioSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        _mixer.SetFloat(MIXER_SOUND, Mathf.Log10(value) * 20);
    }
}
