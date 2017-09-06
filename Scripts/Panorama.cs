using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Panorama.asset", menuName = "Panorama")]
public class Panorama : ScriptableObject
{
    public new string name;
    public Vector3 position;
    public Texture front;
    public Texture back;
    public Texture left;
    public Texture right;
    public Texture up;
    public Texture down;
    public Sprite icon;
}