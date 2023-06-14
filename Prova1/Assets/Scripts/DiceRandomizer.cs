using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRandomizer : MonoBehaviour
{
    private readonly System.Random rand = new System.Random();
    public int diceNumber;
    public Dice dice;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;


    private void Awake()
    {
        dice = gameObject.GetComponent<Dice>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        diceNumber = rand.Next(0, 6);
        ChangeSprite(diceNumber);
        StartCoroutine("DoCheck");
    }

    // Update is called once per frame
    //void Update()
    //{
    //}
    void ChangeSprite(int number)
    {
        spriteRenderer.sprite = spriteArray[number];
    }
    IEnumerator DoCheck()
    {
        while (true)
        {
            if (gameObject.GetComponent<DiceRandomizer>().enabled)
            {
                yield return new WaitForSeconds(.05f);
                if (Input.GetMouseButton(0))
                {
                    ChangeSprite(dice.RandomFace());
                    diceNumber = dice.face;
                }
            }
            yield return null;
        }
    }
}
