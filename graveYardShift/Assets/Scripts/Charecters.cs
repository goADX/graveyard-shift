using UnityEngine;

[CreateAssetMenu(fileName = "Charecters", menuName = "Scriptable Objects/Charecters")]
public class Charecters : ScriptableObject
{
    public Sprite charecterSprite;
    public string charecterName;
    public MonsterTypes monsterType;
    [Tooltip("if the charecter can come agian after its gone")]
    public bool repeatable;
    [Tooltip("if the charecter can come while another copy of it is still in there")]
    public bool clonable;
}
