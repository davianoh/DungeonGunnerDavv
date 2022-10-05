using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArtLocalsDetails_", menuName = "Scriptable Objects/ArtLocals/Art Local Details")]
public class ArtLocalsSO : ScriptableObject
{
    public int obtainInLevel;

    public Sprite artImage;
    public string nama;
    public string location;
    public string year;
    public string artist;

    public string artHistory;
    public string artistHistory;
}
