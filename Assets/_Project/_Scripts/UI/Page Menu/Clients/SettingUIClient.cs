using UnityEngine;
using System;
using System.Collections; 
using CF.Data;
using CF.Audio;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace CF.UI {
public class SettingUIClient : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    Hashtable m_VolumeTable;
    #region Sliders
    [Header("Sliders")]
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider soundTrackSlider;
    [SerializeField]
    private Slider enemySlider;
    [SerializeField]
    private Slider enemyWeaponSlider;
    [SerializeField]
    private Slider playerSlider;
    [SerializeField]
    private Slider playerWeaponSlider;
    [SerializeField]
    private Slider UISlider;
    #endregion

    private void OnEnable() {
        m_VolumeTable = DataController.LoadVolumeData();
        var masterVolume = DataController.LoadMasterVolume();
        masterSlider.value = masterVolume;
        ApplyMasterVolumeChange(masterVolume);

        Configure();
    }

    private void OnDisable() {
        DataController.SaveMasterVolume(masterSlider.value);
        DataController.SaveVolumeData(m_VolumeTable);
    }

    public void OnVolumeChange(string _type) {
        switch (_type)
        {
            case "master":
                ApplyMasterVolumeChange(masterSlider.value);
                break;
            case "soundtrack":
                ApplyVolumeChange(AudioTrackType.SoundTrack, soundTrackSlider.value);
                break;
            case "player":
                ApplyVolumeChange(AudioTrackType.PlayerSFX, playerSlider.value);
                break;
            case "playerweapon":
                ApplyVolumeChange(AudioTrackType.PlayerWeaponSFX, playerWeaponSlider.value);
                break;
            case "enemy":
                ApplyVolumeChange(AudioTrackType.EnemySFX, enemySlider.value);
                break;
            case "enemyweapon":
                ApplyVolumeChange(AudioTrackType.EnemyWeaponSFX, enemyWeaponSlider.value);
                break;
            case "ui":
                ApplyVolumeChange(AudioTrackType.UI_SFX, UISlider.value);
                break;
            case "environment":
                //ApplyVolumeChange(AudioTrackType.EnvironmentSFX, soundTrackSlider.value);
                break;
            default:
                break;
        }
    }

    private void ApplyVolumeChange(AudioTrackType _type, float _value) {
        m_VolumeTable[_type] = _value;
    }

    private void ApplyMasterVolumeChange(float _rawValue) {
        mixer.SetFloat("MasterVolume", Mathf.Log10(_rawValue) * 20);
    }

    private void Configure() {
        soundTrackSlider.value = (float) m_VolumeTable[AudioTrackType.SoundTrack];
        playerSlider.value = (float) m_VolumeTable[AudioTrackType.PlayerSFX];
        playerWeaponSlider.value = (float) m_VolumeTable[AudioTrackType.PlayerWeaponSFX];
        enemySlider.value = (float) m_VolumeTable[AudioTrackType.EnemySFX];
        enemyWeaponSlider.value = (float) m_VolumeTable[AudioTrackType.EnemyWeaponSFX];
        UISlider.value = (float) m_VolumeTable[AudioTrackType.UI_SFX];
    }
}
}
