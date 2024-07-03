using System.Collections;
using UnityEngine;

public class Animation2DCard : MonoBehaviour
{
    public SpriteRenderer rend;
    public PlayerScript _dealerscript;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

    }

    public IEnumerator RotateCard()
    {
        for (float i = 0f; i <= 180f; i += 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i == 90f)
            {
                rend.gameObject.SetActive(false);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                _dealerscript.hand[0].gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.01f);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
