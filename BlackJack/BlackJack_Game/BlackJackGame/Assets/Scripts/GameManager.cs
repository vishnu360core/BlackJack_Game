using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;
    private int standClicks = 0;

    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    public Text scoretext;
    public Text dealerscoretext;
    public Text betstext;
    public Text cashtext;
    public Text maintext;
    public Text standBtnText;

    public GameObject PopUp_Winner;
    public GameObject hideCard;

    int pot = 0;
    public int CountDealer_num = 0;

    public Managers managers;
    public Animation2DCard _animator;

    private void Start()
    {
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => hitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        betBtn.onClick.AddListener(() => BetClicked());
    }

    private void DealClicked()
    {
        playerScript.ResetHand();
        dealerScript.ResetHand();
        managers.playernum = 0;
        managers.dealernum = 0;
        dealerScript.hand[0].gameObject.SetActive(false);
        scoretext.text = null;
        dealerscoretext.text = null;

        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        StartCoroutine(ButtonDisable());
        PopUp_Winner.SetActive(false);
        maintext.gameObject.SetActive(false);
        dealerscoretext.gameObject.SetActive(false);
        dealBtn.gameObject.SetActive(false);

        pot = 40;
        betstext.text = "Bets : $" + pot.ToString();
        playerScript.AdjustMoney(-20);
        cashtext.text = playerScript.GetMoney().ToString();

        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();

        managers.StartFollowUp();
        DOTween.Sequence()
            .AppendCallback(() => playerScript.StartHand())
            .AppendInterval(0.35f)
            .AppendCallback(() => dealerScript.StartHand());
    }

    public void DealCompletedResult()
    {
        scoretext.text = playerScript.handvalue.ToString();
        dealerscoretext.text = dealerScript.handvalue.ToString();
        if (playerScript.handvalue > 21) RoundOver();
    }

    IEnumerator ButtonDisable()
    {
        yield return new WaitForSeconds(2f);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
    }

    private void hitClicked()
    {
        if (playerScript.cardIndex <= 10)
        {
            /* DOTween.Sequence()
            .AppendCallback(() => managers.PlayerDrawCard())
            .AppendInterval(.3f)
            .AppendCallback(() => playerScript.GetCard())
            .AppendInterval(.3f)
            .AppendCallback(() => hitbtnCompleted());*/

            managers.PlayerDrawCard();
        }
    }

    public void hitbtnCompleted()
    {
        scoretext.text = playerScript.handvalue.ToString();
        if (playerScript.handvalue > 20)
        {
            RoundOver();
            StartCoroutine(_animator.RotateCard());
        }
    }

    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1)
        {
            RoundOver();
        }
        HitDealer();
    }

    private void HitDealer()
    {
        StartCoroutine(_animator.RotateCard());

        while (dealerScript.handvalue < 16 && dealerScript.cardIndex < 10)
        {
            CountDealer_num++;
           // managers.DealerDrawCard();
            dealerScript.GetCard();
            dealerscoretext.text = dealerScript.handvalue.ToString();
            if (playerScript.handvalue > 20) RoundOver();
        }
        StartCoroutine(waitfordeclair());
    }

    IEnumerator waitfordeclair()
    {
        yield return new WaitForSeconds(.5f);

        if (standClicks == 1)
        {
            StandClicked();
            HitDealer();
        }
    }

    public void RoundOver()
    {
        Debug.Log("RoundOver");
        bool playerBust = playerScript.handvalue > 21;
        bool dealerBust = dealerScript.handvalue > 21;
        bool player21 = playerScript.handvalue == 21;
        bool dealer21 = dealerScript.handvalue == 21;

        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;

        if (playerBust && dealerBust)
        {
            maintext.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else if (playerBust || (!dealerBust && dealerScript.handvalue > playerScript.handvalue))
        {
            maintext.text = "Dealer Win";
        }
        else if (dealerBust || playerScript.handvalue > dealerScript.handvalue)
        {
            maintext.text = "You win";
            playerScript.AdjustMoney(pot);
        }
        else if (playerScript.handvalue == dealerScript.handvalue)
        {
            maintext.text = "Push: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }

        if (roundOver)
        {
            GameObject.Find("Deck").GetComponent<DeckScript>().ResetSprites();
            CountDealer_num = 0;
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            PopUp_Winner.SetActive(true);
            maintext.gameObject.SetActive(true);
            dealerscoretext.gameObject.SetActive(true);
            cashtext.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
        }
    }

    void BetClicked()
    {
        playerScript.AdjustMoney(20);
        cashtext.text = "$" + playerScript.GetMoney().ToString();
        pot = 20;
        betstext.text = "Bets : $" + pot.ToString();
    }
}