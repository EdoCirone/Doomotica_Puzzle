using UnityEngine;
using UnityEngine.Audio;

public class LevelMixer : MonoBehaviour
{
    public AudioMixer mixer;
    
    public void SetSFXLVL(float sfxlvl)
    {
        mixer.SetFloat("sfxVolume", sfxlvl);
    }
    public void SetMusicLVL(float musiclvl)
    {
        mixer.SetFloat("musicVolume", musiclvl);
    }


}

