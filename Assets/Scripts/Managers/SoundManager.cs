using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public List<AudioSource> SoundEffects;

    [SerializeField]
    private AudioMixer soundMixer;
    [SerializeField]
    AudioMixerGroup effectsGroup, soundtrackGroup;

    [SerializeField]
    float maxPitchChange;

    public AudioSource Soundtrack;
    [SerializeField]
    SoundEffect[] allSoundEffects;



    public bool PlaySoundtrack
    {
        get => playSoundtrack;
        set
        {
            if (playSoundtrack == value || Soundtrack == null)
                return;
            if (value)
            {
                Soundtrack.volume = startVolume;
            }
            else
            {
                Soundtrack.volume = 0;
            }
            playSoundtrack = value;
        }
    }
    float startVolume;
    private bool playSoundtrack;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (Soundtrack != null)
            startVolume = Soundtrack.volume;
        soundMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1)) * 20);
        soundMixer.SetFloat("EffectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsVolume", 1)) * 20);
        soundMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1)) * 20);
        DontDestroyOnLoad(gameObject);
    }

    //public void PlayEffect(AudioClip effect, float volume = 1)
    //{
    //    for (int i = 0; i < SoundEffects.Count; i++)
    //    {
    //        if (!SoundEffects[i].isPlaying)
    //        {
    //            SoundEffects[i].volume = volume;
    //            SoundEffects[i].clip = effect;
    //            SoundEffects[i].Play();
    //            return;
    //        }
    //    }
    //    AudioSource tmp = gameObject.AddComponent<AudioSource>();
    //    tmp.outputAudioMixerGroup = effectsGroup;
    //    tmp.clip = effect;
    //    tmp.volume = volume;
    //    tmp.Play();
    //    SoundEffects.Add(tmp);
    //}

    public bool SFXIsPlaying(eSFX sfx, GameObject origin)
    {
        AudioSource tmp = null;
        if (origin != null)
        {
            for (int i = 0; i < allSoundEffects.Length; i++)
            {
                if (allSoundEffects[i].callText != sfx)
                {
                    continue;
                }
                AudioSource[] aSources = origin.GetComponents<AudioSource>();
                bool foundPair = false;
                for (int j = 0; j < allSoundEffects[i].AttachedAudioSources.Count; j++)
                {
                    for (int k = 0; k < aSources.Length; k++)
                    {
                        if (aSources[k] == allSoundEffects[i].AttachedAudioSources[j])
                        {
                            foundPair = true;
                            tmp = allSoundEffects[i].AttachedAudioSources[j];
                            return tmp.isPlaying;
                        }
                    }
                    if (foundPair == true)
                        return false;
                }
            }
        }
        return false;
    }

    public void PlaySFX(eSFX sfx, GameObject origin, bool startPlay = true, bool randomPitch = false)
    {
        AudioSource tmp = null;
        if (origin != null)
        {
            for (int i = 0; i < allSoundEffects.Length; i++)
            {
                if (allSoundEffects[i].callText != sfx)
                {
                    continue;
                }
                AudioSource[] aSources = origin.GetComponents<AudioSource>();
                bool foundPair = false;
                for (int j = 0; j < allSoundEffects[i].AttachedAudioSources.Count; j++)
                {
                    for (int k = 0; k < aSources.Length; k++)
                    {
                        if (aSources[k] == allSoundEffects[i].AttachedAudioSources[j])
                        {
                            foundPair = true;
                            tmp = allSoundEffects[i].AttachedAudioSources[j];
                            break;
                        }
                    }
                    if (foundPair == true)
                        break;
                }
                if (!foundPair)
                {
                    tmp = origin.AddComponent<AudioSource>();
                    CopyComp.CopyAudioSource(allSoundEffects[i].source[UnityEngine.Random.Range(0, allSoundEffects[i].source.Length)], tmp);
                    //CopyComp.GetCopyOf(allSoundEffects[i].source[UnityEngine.Random.Range(0, allSoundEffects[i].source.Length)], tmp);
                    allSoundEffects[i].AttachedAudioSources.Add(tmp);
                }
                else if (allSoundEffects[i].source.Length >= 2)
                {
                    CopyComp.CopyAudioSource(allSoundEffects[i].source[UnityEngine.Random.Range(0, allSoundEffects[i].source.Length)], tmp);
                }
                break;
            }
            if (tmp == null)
            {
                //Debug.Log($"Inspector doenst have setup for eSFX '{sfx}'");
                return;
            }
        }
        else
        {
            for (int i = 0; i < allSoundEffects.Length; i++)
            {
                if (allSoundEffects[i].callText != sfx)
                {
                    continue;
                }
                tmp = allSoundEffects[i].source[UnityEngine.Random.Range(0, allSoundEffects[i].source.Length)];
            }
            if (tmp == null)
            {
                //Debug.Log($"Inspector doenst have setup for eSFX '{sfx}'");
                return;
            }
        }



        //PlaySound(sfx);
        if (randomPitch)
            tmp.pitch = UnityEngine.Random.Range(-maxPitchChange, maxPitchChange) + 1;

        if (startPlay)
        {
            tmp.Play();
            //Debug.Log("Playing sfx: " + sfx + "on " + origin);
        }
        else
            tmp.Stop();
    }

    public void ChangeMasterVolume(float amount)
    {
        soundMixer.SetFloat("MasterVolume", Mathf.Log10(amount) * 20);
        PlayerPrefs.SetFloat("MasterVolume", amount);
    }

    public void ChangeSoundEffectVolume(float amount)
    {
        soundMixer.SetFloat("EffectsVolume", Mathf.Log10(amount) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", amount);
    }

    public void ChangeMusicVolume(float amount)
    {
        soundMixer.SetFloat("MusicVolume", Mathf.Log10(amount) * 20);
        PlayerPrefs.SetFloat("MusicVolume", amount);
    }

    public void PlaySound(eSFX sound)
    {
        if (sound == eSFX.NONE)
            return;
        for (int i = 0; i < allSoundEffects.Length; i++)
        {
            if (allSoundEffects[i].callText == sound)
            {
                allSoundEffects[i].source[UnityEngine.Random.Range(0, allSoundEffects[i].source.Length)].Play();
                return;
            }
        }

        Debug.Log("Could not find sound: " + sound);
    }
}

[System.Serializable]
struct SoundEffect
{
    public eSFX callText;
    public AudioSource[] source;
    [HideInInspector]
    public List<AudioSource> AttachedAudioSources;
    [HideInInspector]
    public float[] StandardVolume;

}

public static class CopyComp
{
    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static void CopyAudioSource(AudioSource origin, AudioSource destination)
    {
        destination.clip = origin.clip;
        destination.volume = origin.volume;
        destination.playOnAwake = origin.playOnAwake;
        destination.rolloffMode = origin.rolloffMode;
        destination.minDistance = origin.minDistance;
        destination.maxDistance = origin.maxDistance;
        destination.spatialBlend = origin.spatialBlend;
        destination.loop = origin.loop;
        destination.outputAudioMixerGroup = origin.outputAudioMixerGroup;

    }
}


public enum eSFX
{
    NONE,
    EPlAmCursorAmbiance,
    EPlDeath,
    EPlRespawn,
    EPlTakeDmg,
    EPlChangeGravityDown,
    EPlChangeGravityUp,
    EPlFloatLoop,
    EPlDash,
    EPlFlyingAroundLoop,
    EPlFootstepLoop,
    EPlJump,
    EPlStickToStickySurface,
    EPlUnstickFromStickySurface,
    EPlShootGrappleShot,
    EPlShotHitGround,
    EObPlayerHitsGround,
    EObShootBubble,
    EObBlockBreak,
    EObKeyUse,
    EObAmKey,
    EObAmStardust,
    EObMushroomBounce,
    EObCollectStardust,
    EObCollectKey,
    EObBouncePlantBounce,
    EObGravitySwitch,
    EObCatapultPlayerStart,
    EObPopBubble,
    EObTeleportPlayer,
    EUIMenuOpenJingle,
    EUIMenuCloseJingle,
    EUIOpenSettingsJingle,
    EUICloseSettingsJingle,
    EUIButtonPress,
    EUIContinueGame
}
