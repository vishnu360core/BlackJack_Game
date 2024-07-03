using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public DeckScript deckscript;

    public int handvalue = 0;
    private int money = 1000;

    public GameObject[] hand;

    public int cardIndex = 0;

    List<CardScript> aceList = new List<CardScript>();

    public GameManager _gameManager;
    public Animation2DCard _cardAnimation;

    public void StartHand()
    {
        DOTween.Sequence()
            .AppendInterval(0.5f)
            .AppendCallback(() => GetCard())
            .AppendInterval(1f)
            .AppendCallback(() => GetCard())
            .AppendInterval(0.3f)
            .AppendCallback(() => _gameManager.DealCompletedResult())
            .AppendCallback(() => _cardAnimation.rend.gameObject.SetActive(true));
        
    }

    public int GetCard()
    {
        int cardValue = deckscript.DealCard(hand[cardIndex].GetComponent<CardScript>());

        hand[cardIndex].GetComponent<Renderer>().enabled = true;

        handvalue += cardValue;

        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }
       
        Acecheck();
        cardIndex++;
        return handvalue;
    }

    public void Acecheck()
    {
        foreach (CardScript ace in aceList)
        {
            if (handvalue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                ace.SetValue(10);
                handvalue += 10;
            }
            else if (handvalue > 21 && ace.GetValueOfCard() == 10)
            {
                ace.SetValue(1);
                handvalue -= 10;
            }
        }
    }

    public void AdjustMoney(int amount)
    {
        money += amount;
    }

    public int GetMoney()
    {
        return money;
    }

    public void ResetHand()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }

        cardIndex = 0;
        handvalue = 0;
        aceList = new List<CardScript>();
    }
}