using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicTrack_", menuName = "Scriptable Objects/Sounds/Music Track")]
public class MusicTrackSO : ScriptableObject
{
    public string musicName;
    public AudioClip musicClip;
    [Range(0, 1)]
    public float musicVolume = 1f;


}
