using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Animator))]
public class Block : MonoBehaviour
{
    private const int DARK_TEXT_BELOW = 8;

   // public EventHandler OnMerge;

    [SerializeField] private List<ColorSO> colorSOList;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro numberText;
    [SerializeField] private Color lightTextColor;
    [SerializeField] private Color darkTextColor;

    [Space(15)]
    [SerializeField] private float movingTime;
    [SerializeField] private float destroyTime;

    private int currentNumber;
    private Animator animator;


    private IEnumerator Moving(Vector2 target, bool destroyAfter = false)
    {
        float distance = Vector2.Distance(target, Pos);

        while(Pos != target)
        {
            transform.position = Vector2.MoveTowards(Pos, target, distance / 1 / movingTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (destroyAfter) Destroy(gameObject);
    }


    public Vector2 Pos => transform.position;
    public int number => currentNumber;

    public void SetNumber(int number)
    {
        /* foreach (ColorSO colorSO in colorSOList)
         {
             if (colorSO.number == number)
             {
                 currentNumber = number;
                 spriteRenderer.color = colorSO.color;
                 numberText.text = number.ToString();
                 if (number < DARK_TEXT_BELOW) numberText.color = darkTextColor;
                 else numberText.color = lightTextColor;
                 break;
             }
         }*/

        currentNumber = number;
        numberText.text = number.ToString();

        if (number < DARK_TEXT_BELOW) numberText.color = darkTextColor;
        else numberText.color = lightTextColor;

        if (number <= 2048)
        {
            foreach (ColorSO colorSO in colorSOList)
            {
                if (colorSO.number == number)
                {
                    spriteRenderer.color = colorSO.color;
                    break;
                }
            }
        }
        else spriteRenderer.color = Color.black;
    }


    public void MoveToCell(Cell target)
    {
        StartCoroutine(Moving(target.Pos));
        target.containedBlock = this;
    }

    public void MergeWith(Block block)
    {
        StartCoroutine(Moving(block.Pos, true));
        block.SetNumber(number * 2);
        block.Play_MergeAnimation();
        //OnMerge?.Invoke(this, EventArgs.Empty);
    }

    public void Play_MergeAnimation() => animator.Play("Merge", 0, 0);


    public void Destroy()
    {
        animator.Play("Destroy");
        Destroy(gameObject, destroyTime);
    }


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
}
