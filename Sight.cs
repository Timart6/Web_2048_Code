using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sight : MonoBehaviour
{
    [SerializeField] private Texture2D sightTexture;
    [SerializeField] private float reloadTime;

    private int borderX;
    private int borderY;
    private bool isSight;
    private bool readyToShoot = true;



    private IEnumerator Reloading()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(reloadTime);
        readyToShoot = true;
    }



    public void SetGameBorders(int width, int height)
    {
        borderX = width;
        borderY = height;
    }

    private bool InGameField()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mouseWorldPosition.x >= -1 && mouseWorldPosition.x <= borderX - 1 && mouseWorldPosition.y >= 0 && mouseWorldPosition.y <= borderY;
    }


    public bool TryShooAt(out Vector2 closestCellPosition) 
    {
        if (isSight && readyToShoot)
        {
            StartCoroutine(Reloading());
            closestCellPosition = new Vector2(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + 0.5f), Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f));
            return true;
        }
        else 
        {
            closestCellPosition = Vector2.zero;
            return false;
        }
    }
  
    

    private void Update()
    {
        if (!isSight && InGameField())
        {
            isSight = true;
            Cursor.SetCursor(sightTexture, Vector2.zero, CursorMode.ForceSoftware);
        }
        else if (isSight && !InGameField())
        {
            isSight = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
