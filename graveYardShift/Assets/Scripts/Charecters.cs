using UnityEngine;

[CreateAssetMenu(fileName = "Charecters", menuName = "Scriptable Objects/Charecters")]
public class Charecters : ScriptableObject
{
    public Sprite charecterSprite;
    public string charecterName;
    public MonsterTypes monsterType;
}
