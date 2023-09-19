using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    AudioMixer mixer;

    private float _BgmVolume, _EffectVolume;

    public float BgmVolume
    {
        get { return _BgmVolume; } 
        set 
        {
            _BgmVolume = value;
            mixer.SetFloat("BgmVolume", Mathf.Log10(_BgmVolume) * 20);
        }
    }
    public float EffectVolume
    {
        get { return _EffectVolume; } 
        set 
        {
            _EffectVolume = value;
            mixer.SetFloat("EffectVolume", Mathf.Log10(_EffectVolume) * 20);
        }
    }

    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    public void Init()
    {
        mixer = Managers.Resource.Load<AudioMixer>("Sounds/AudioMixer");

        BgmVolume    = 1.0f;
        EffectVolume = 1.0f;

        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for(int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = "@Sound" };
                _audioSources[i] = go.AddComponent<AudioSource>();
                _audioSources[i].outputAudioMixerGroup = mixer.FindMatchingGroups(soundNames[i])[0];
                go.transform.parent = root.transform;
            }
            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    //Dictionary�̱� ������ �޸� �Ⱥ��� ������.
    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClips.Clear();    //��ųʸ� ����
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        Play(
            GetOrAddAudioClip(path, type),
            type,
            pitch
        );
    }

    //BGM, Effect�� �����ؼ� ���带 �ʿ�ø��� ��ü���ش�.
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if(audioClip == null)
        {
            Debug.Log("AudioClip is null!");
            return;
        }

        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.volume = BgmVolume;
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];

            audioSource.volume = EffectVolume;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{type}/{path}";

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }

        else
        {
            if(_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing {path}");

        return audioClip;
    }
}
