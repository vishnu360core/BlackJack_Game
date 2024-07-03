using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Managers : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform[] playerPositions;
    public Transform[] dealerPositions;
    public Transform deckPosition;
    public Transform lastPosition;
    public float cardMoveDuration = 0.5f;
    public GameManager gameManager;
    public Transform[] playerHitPositions;
    public Transform[] dealerHitPositions;
    public PlayerScript _playerScript;
    public PlayerScript _dealerScript;

    public int playernum = 0;
    public int dealernum = 0;

    public List<GameObject> GeneratedCard = new List<GameObject>();

    public void StartFollowUp()
    {
        StartCoroutine(DistributeInitialCards());
    }

    IEnumerator DistributeInitialCards()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity);
            GeneratedCard.Add(card);
            card.transform.localScale = new Vector3(.3f, .3f, .3f);

            card.transform.DOScale(new Vector3(.8f, .8f, .8f), cardMoveDuration);


            if (i % 2 == 0)
            {
                card.transform.DOMove(playerPositions[i / 2].position, 0.4f);
            }
            else
            {
                card.transform.DOMove(dealerPositions[i / 2].position, 0.4f).OnComplete((() =>
                {
                    if (i == 1)
                    {
                        gameManager.hideCard.gameObject.SetActive(true);
                        gameManager.hideCard.GetComponent<Renderer>().enabled = true;
                    }
                }));
            }

            GeneratedCard[i].name = "Card_" + i;
            yield return new WaitForSeconds(0.45f);
            card.transform.gameObject.SetActive(false);

        }
    }

    public void PlayerDrawCard()
    {
        playernum++;
        if (playernum <= 8)
        {
            StartCoroutine(DistributeCardsWithDelay(0.3f));
        }
    }

    private IEnumerator DistributeCardsWithDelay(float delay)
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(delay);

            GameObject card = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity);
            GeneratedCard.Add(card);
            card.transform.localScale = new Vector3(.3f, .3f, .3f);


            card.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f);


            card.transform.DOMove(playerHitPositions[playernum].position, 0.3f).OnComplete(() =>
            {
                DOTween.Sequence()
                .AppendCallback(() => _playerScript.GetCard())
                .AppendInterval(.3f)
                .AppendCallback(() => gameManager.hitbtnCompleted());
            });


            card.name = "Card_" + GeneratedCard.Count;

            StartCoroutine(DisableCard(card, 0.3f));
        }
    }

    public void DealerDrawCard()
    {
        dealernum++;

        if (dealernum <= 8)
        {
            StartCoroutine(DistributeInitial());
        }
    }

    IEnumerator DistributeInitial()
    {
        for (int i = 0; i < gameManager.CountDealer_num; i++)
        {
            GameObject card = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity);
            GeneratedCard.Add(card);

            card.transform.localScale = new Vector3(.3f, .3f, .3f);

            card.transform.DOScale(new Vector3(.8f, .8f, .8f), cardMoveDuration);

            card.transform.DOMove(dealerHitPositions[i].position, 0.4f).OnComplete((() =>
            {
                if (i == 1)
                {
                    gameManager.hideCard.gameObject.SetActive(true);
                    gameManager.hideCard.GetComponent<Renderer>().enabled = true;
                }
            })); 

            yield return new WaitForSeconds(0.45f);
            card.transform.gameObject.SetActive(false);
        }
    }

    IEnumerator DisableCard(GameObject card, float delay)
    {
        yield return new WaitForSeconds(delay);
        card.SetActive(false);
    }
}