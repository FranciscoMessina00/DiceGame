using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRandomizer : MonoBehaviour
{
    public Dice dice;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    private IEnumerator coroutine;
    Coroutine co;
    private void Awake()
    {
        coroutine = DoCheck();
        dice = gameObject.GetComponent<Dice>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCo();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeSprite(dice.face - 1);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    coroutine = DoCheck();
    //}
    public void ChangeSprite(int number)
    {
        spriteRenderer.sprite = spriteArray[number];
    }
    IEnumerator DoCheck()
    {
        while (true)
        {
            if (gameObject.GetComponent<DiceRandomizer>().enabled)
            {
                if (Input.GetMouseButton(0))
                {
                    ChangeSprite(dice.RandomFace());
                }
                yield return new WaitForSeconds(.05f);
            }
            yield return null;
        }
    }
    public void StartCo()
    {
        co = StartCoroutine(coroutine);
        Debug.Log("Iniziooooo");
    }
    public void StopCo()
    {
        StopCoroutine(co);
        Debug.Log("Finitoooo");
    }
}
