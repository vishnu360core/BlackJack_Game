using UnityEngine;

[CreateAssetMenu(fileName = "NewSpriteData", menuName = "Sprite Data", order = 51)]
public class SpriteData : ScriptableObject
{
    public Sprite[] sprite;
    public Sprite[] Original_sprite;

    public int[] cardvalues = new int[53];
    public int[] Orignal_cardvalues = new int[53];
}
