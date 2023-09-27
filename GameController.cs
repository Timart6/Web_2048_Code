using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private Transform boardPrefab;

    [Space(15)]
    [SerializeField] private Scores scorePanel;
    [SerializeField] private Sight sight;
    [SerializeField] private Statistics statistics;
    [SerializeField] private AudioManager audioManager;

    [Space(15)]
    [SerializeField] private GameObject looseText;
    [SerializeField] private Background background;

    [Space(15)]
    [SerializeField] private int spawn2Chance;
    [SerializeField] private int spawn4Chance;
    [SerializeField] private float movingTime;

    [Space(15)]
    [SerializeField] private float minimumCameraSize;
    [SerializeField] private float cameraSizeMult;


    private List<Cell> cells = new List<Cell>();
    private List<Block> spawnedBlocks = new List<Block>();
    private List<Block> passedVictoryThreshold = new List<Block>();
    private int currentScoresForShot = 2;


    private enum GameState
    {
        WaitingInput,
        Moving,
        Loose
    }
    private GameState gameState; 



    private IEnumerator MovingProcess()
    {
        gameState = GameState.Moving;
        yield return new WaitForSeconds(movingTime);
        SpawnBlock();
    }



    private void SpawnGrid()
    {
        int width = GridSizeSettings.width;
        int height = GridSizeSettings.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell newCell = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity);
                cells.Add(newCell);
            }
        }

        Vector2 center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);
        Transform background = Instantiate(boardPrefab, center, Quaternion.identity);
        background.transform.localScale = new Vector2(width, height);

        int scaleMult = (width > height) ? width : height;
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        Camera.main.orthographicSize = minimumCameraSize + cameraSizeMult * scaleMult;

        scorePanel.Place(center, height, scaleMult);
        sight.SetGameBorders(width, height);
    }



    private void SpawnBlock()
    {
        if(spawnedBlocks.Count > 1)
        {
            List<Block> orderedBlocks = spawnedBlocks.OrderBy(block => block.number).Reverse().ToList();
            Block biggestBlock = orderedBlocks[0];
            if (biggestBlock.number == 2048 && !passedVictoryThreshold.Contains(biggestBlock))
            {
                Win();
                passedVictoryThreshold.Add(biggestBlock);
            }

            if (biggestBlock.number > PlayerPrefs.GetInt("Value")) statistics.ChangeMaxNumber(biggestBlock.number);
        }

        List<Cell> freeCells = new List<Cell>();
        foreach (Cell cell in cells) if (cell.containedBlock == null) freeCells.Add(cell);

        if (freeCells.Count != 0)
        {
            gameState = GameState.WaitingInput;
            int numberToSpawn = 2;
            if (UnityEngine.Random.Range(1, spawn2Chance + spawn4Chance + 1) > spawn2Chance) numberToSpawn = 4;

            int randomCell = UnityEngine.Random.Range(0, freeCells.Count);
            Block newBlock = Instantiate(blockPrefab, freeCells[randomCell].Pos, Quaternion.identity);
            newBlock.SetNumber(numberToSpawn);
            freeCells[randomCell].containedBlock = newBlock;

            spawnedBlocks.Add(newBlock);
        }
        else Loose();
    }



    private void MoveBlocks(Vector2 direction)
    {
        List<Block> orderedBlocks = new List<Block>();
        switch(direction)
        {
            case Vector2 v when v == Vector2.left:
                orderedBlocks = spawnedBlocks.OrderBy(block => block.Pos.x).ToList();
                break;

            case Vector2 v when v == Vector2.right:
                orderedBlocks = spawnedBlocks.OrderBy(block => block.Pos.x).Reverse().ToList();
                break;

            case Vector2 v when v == Vector2.down:
                orderedBlocks = spawnedBlocks.OrderBy(block => block.Pos.y).ToList();
                break;

            case Vector2 v when v == Vector2.up:
                orderedBlocks = spawnedBlocks.OrderBy(block => block.Pos.y).Reverse().ToList();
                break;
        }

        foreach(Block block in orderedBlocks)
        {
            Vector2 positionToCheck = block.Pos + direction;
            while(true)
            {
                Cell checkingCell = GetCellAtPosition(positionToCheck);
                if (checkingCell != null && checkingCell.containedBlock == null) positionToCheck += direction;
                else
                {
                    if (checkingCell != null && checkingCell.containedBlock.number == block.number)
                    {
                        GetCellAtPosition(block.Pos).containedBlock = null;
                        block.MergeWith(checkingCell.containedBlock);
                        spawnedBlocks.Remove(block);
                        scorePanel.ChangeScore(block.number * 2);
                        if (scorePanel.score > PlayerPrefs.GetInt("Score")) statistics.ChangeMaxScore(scorePanel.score);
                    }
                    else if (block.Pos != positionToCheck - direction)
                    {
                        GetCellAtPosition(block.Pos).containedBlock = null;
                        block.MoveToCell(GetCellAtPosition(positionToCheck - direction));
                    }
                    break;
                }
            }
        }

        StartCoroutine(MovingProcess());
    }



    private Cell GetCellAtPosition(Vector2 position)
    {
        return cells.FirstOrDefault(cell => cell.Pos == position);
    }



    private void Shoot()
    {
        if (scorePanel.score >= currentScoresForShot && sight.TryShooAt(out Vector2 cellPosition))
        {
            audioManager.Play_ShotSound();

            Cell shotCell = GetCellAtPosition(cellPosition);
            if (shotCell != null && shotCell.containedBlock != null)
            {
                shotCell.containedBlock.Destroy();
                spawnedBlocks.Remove(shotCell.containedBlock);
                shotCell.containedBlock = null;

                scorePanel.ChangeScore(-currentScoresForShot);
                scorePanel.IncreaseScoresForShot(scorePanel.shotScore * 4);
                currentScoresForShot *= currentScoresForShot; 
            }
        }
    }



    private void Loose()
    {
        looseText.SetActive(true);
        gameState = GameState.Loose;
    }

    private void Win()
    {
        background.ShowWin();
        statistics.AddVictory();
    }



    private void Update()
    {
        if (gameState == GameState.WaitingInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) MoveBlocks(Vector2.left);
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) MoveBlocks(Vector2.right);
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) MoveBlocks(Vector2.down);
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) MoveBlocks(Vector2.up);
            else if (Input.GetKeyDown(KeyCode.Mouse0)) Shoot();
        }
    }


    private void Start()
    {
        SpawnGrid();
        SpawnBlock();
        SpawnBlock();
    }
}
