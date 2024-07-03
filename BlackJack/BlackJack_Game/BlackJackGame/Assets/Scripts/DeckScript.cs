using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    int currentIndex = 0;
    public SpriteData spriteData;

    private void Start()
    {
        ResetSprites();
    }

    public void Shuffle()
    {
        for (int i = spriteData.sprite.Length - 1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * spriteData.sprite.Length - 1) + 1;

            Sprite face = spriteData.sprite[i];
            spriteData.sprite[i] = spriteData.sprite[j];
            spriteData.sprite[j] = face;

            int value = spriteData.cardvalues[i];
            spriteData.cardvalues[i] = spriteData.cardvalues[j];
            spriteData.cardvalues[j] = value;
        }
        currentIndex = 1;
    }

    public void ResetSprites()
    {
        for (int i = 0; i < spriteData.Original_sprite.Length; i++)
        {
            spriteData.sprite[i] = spriteData.Original_sprite[i];
        }

        for (int i = 0; i < spriteData.Orignal_cardvalues.Length; i++)
        {
            spriteData.cardvalues[i] = spriteData.Orignal_cardvalues[i];
        }
    }

    public int DealCard(CardScript cardscript)
    {
        cardscript.SetSprite(spriteData.sprite[currentIndex]);
        cardscript.SetValue(spriteData.cardvalues[currentIndex]);
        currentIndex++;
        return cardscript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return spriteData.sprite[0];
    }
}