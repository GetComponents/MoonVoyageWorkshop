using System.Collections;
using System.Collections.Generic;
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
    AudioSource PlAmCursorAmbiance,
PlDeath,
PlRespawn,
PlTakeDmg,
PlChangeGravityDown,
PlChangeGravityUp,
PlFloatLoop,
PlDash,
PlFlyingAroundLoop,
PlFootstepLoop,
PlJump,
PlStickToStickySurface,
PlUnstickFromStickySurface,
PlShootGrappleShot,
PlShotHitGround,
ObPlayerHitsGround,
ObShootBubble,
ObBlockBreak,
ObKeyUse,
ObAmKey,
ObAmStardust,
ObMushroomBounce,
ObCollectStardust,
ObCollectKey,
ObBouncePlantBounce,
ObGravitySwitch,
ObCatapultPlayerStart,
ObPopBubble,
ObTeleportPlayer,
UIMenuOpenJingle,
UIMenuCloseJingle,
UIOpenSettingsJingle,
UICloseSettingsJingle,
UIButtonPress,
UIContinueGame;



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

    public void PlaySFX(eSFX sfx, bool startPlay = true, bool randomPitch = false)
    {
        AudioSource tmp;

        switch (sfx)
        {
            case eSFX.EPlAmCursorAmbiance:
                tmp = PlAmCursorAmbiance;
                break;
            case eSFX.EPlDeath:
                tmp = PlDeath;
                break;
            case eSFX.EPlRespawn:
                tmp = PlRespawn;
                break;
            case eSFX.EPlTakeDmg:
                tmp = PlTakeDmg;
                break;
            case eSFX.EPlChangeGravityDown:
                tmp = PlChangeGravityDown;
                break;
            case eSFX.EPlChangeGravityUp:
                tmp = PlChangeGravityUp;
                break;
            case eSFX.EPlFloatLoop:
                tmp = PlFloatLoop;
                break;
            case eSFX.EPlDash:
                tmp = PlDash;
                break;
            case eSFX.EPlFlyingAroundLoop:
                tmp = PlFlyingAroundLoop;
                break;
            case eSFX.EPlFootstepLoop:
                tmp = PlFootstepLoop;
                break;
            case eSFX.EPlJump:
                tmp = PlJump;
                break;
            case eSFX.EPlStickToStickySurface:
                tmp = PlStickToStickySurface;
                break;
            case eSFX.EPlUnstickFromStickySurface:
                tmp = PlUnstickFromStickySurface;
                break;
            case eSFX.EPlShootGrappleShot:
                tmp = PlShootGrappleShot;
                break;
            case eSFX.EPlShotHitGround:
                tmp = PlShotHitGround;
                break;
            case eSFX.EObPlayerHitsGround:
                tmp = ObPlayerHitsGround;
                break;
            case eSFX.EObShootBubble:
                tmp = ObShootBubble;
                break;
            case eSFX.EObBlockBreak:
                tmp = ObBlockBreak;
                break;
            case eSFX.EObKeyUse:
                tmp = ObKeyUse;
                break;
            case eSFX.EObAmKey:
                tmp = ObAmKey;
                break;
            case eSFX.EObAmStardust:
                tmp = ObAmStardust;
                break;
            case eSFX.EObMushroomBounce:
                tmp = ObMushroomBounce;
                break;
            case eSFX.EObCollectStardust:
                tmp = ObCollectStardust;
                break;
            case eSFX.EObCollectKey:
                tmp = ObCollectKey;
                break;
            case eSFX.EObBouncePlantBounce:
                tmp = ObBouncePlantBounce;
                break;
            case eSFX.EObGravitySwitch:
                tmp = ObGravitySwitch;
                break;
            case eSFX.EObCatapultPlayerStart:
                tmp = ObCatapultPlayerStart;
                break;
            case eSFX.EObPopBubble:
                tmp = ObPopBubble;
                break;
            case eSFX.EObTeleportPlayer:
                tmp = ObTeleportPlayer;
                break;
            case eSFX.EUIMenuOpenJingle:
                tmp = UIMenuOpenJingle;
                break;
            case eSFX.EUIMenuCloseJingle:
                tmp = UIMenuCloseJingle;
                break;
            case eSFX.EUIOpenSettingsJingle:
                tmp = UIOpenSettingsJingle;
                break;
            case eSFX.EUICloseSettingsJingle:
                tmp = UICloseSettingsJingle;
                break;
            case eSFX.EUIButtonPress:
                tmp = UIButtonPress;
                break;
            case eSFX.EUIContinueGame:
                tmp = UIContinueGame;
                break;
            default:
                tmp = PlDeath;
                break;
        }
        if (randomPitch)
            tmp.pitch = Random.Range(-maxPitchChange, maxPitchChange) + 1;

        if (startPlay)
            tmp.Play();
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
}


public enum eSFX
{
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
