using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Data/Sounds")]
public class Sounds : ScriptableObject
{
    [SerializeField] private List<Sound> sounds;

    public AudioClip GetSoundWithRandom(AudioType type)
    {
        var needTypeSounds = sounds.Where(s => s.Type == type).ToList();

        return needTypeSounds[UnityEngine.Random.Range(0, needTypeSounds.Count)].Clip;
    }

    [Serializable]
    public struct Sound
    {
        [SerializeField] private AudioType _type;
        [SerializeField] private AudioClip _clip;

        public AudioType Type => _type;
        public AudioClip Clip => _clip;
    }
}

public enum AudioType
{
    MainMusic,
    ExtractSound,
    ClickButtonSound
}