using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSizeSettings : MonoBehaviour
{
    private const int MAX_SIZE = 10;
    private const int MIN_SIZE = 4;

    [SerializeField] private Button increaseW;
    [SerializeField] private Button decreaseW;
    [SerializeField] private TextMeshProUGUI wText;

    [Space(15)]
    [SerializeField] private Button increaseH;
    [SerializeField] private Button decreaseH;
    [SerializeField] private TextMeshProUGUI hText;

    public static int width { get; private set; }
    public static int height { get; private set; }



    private void Awake()
    {
        increaseW.onClick.AddListener(() =>
        {
            if (width < MAX_SIZE)
            {
                width++;
                wText.text = width.ToString();
            }
        });

        decreaseW.onClick.AddListener(() =>
        {
            if(width > MIN_SIZE)
            {
                width--;
                wText.text = width.ToString();
            }
        });

        increaseH.onClick.AddListener(() =>
        {
            if (height < MAX_SIZE)
            {
                height++;
                hText.text = height.ToString();
            }
        });

        decreaseH.onClick.AddListener(() =>
        {
            if (height > MIN_SIZE)
            {
                height--;
                hText.text = height.ToString();
            }
        });

        if (width < MIN_SIZE) width = MIN_SIZE;
        if (height < MIN_SIZE) height = MIN_SIZE;

        wText.text = width.ToString();
        hText.text = height.ToString();
    }
}
