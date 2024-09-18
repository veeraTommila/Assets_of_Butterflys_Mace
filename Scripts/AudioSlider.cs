using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    /*
     * This creates a private field for the slider element, makes it visible in the Unity Editor,
     * and allows one to assign the slider trhough the Inspector.
     */
    [SerializeField] private Slider volumeSlider;
    
    // A private method called when the script instance is loaded.
    private void Start()
    {
        // To set the slider's value to the saved volume level from PlayerPrefs, or to 1 if no value is found.
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);

        // To make sure that the global volume level corresponds the slider's value.
        AudioListener.volume = volumeSlider.value;
    }

    // a public method to change the volume.
    public void ChangeVolume(float volume)
    {
        // To set the global volume level to the specified value.
        AudioListener.volume = volume;

        // To save the specified volume level to PlayerPrefs for later retrieveing.
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
