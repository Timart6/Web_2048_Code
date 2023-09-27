using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    /* public Block containedBlock { get; private set; }

     public void SetBlock(Block block)
     {
        // if (containedBlock != null) containedBlock.transform.parent = null;
         //block.transform.parent = transform;
         containedBlock = block;
     } */


    [HideInInspector] public Block containedBlock;
    public Vector2 Pos => transform.position;
}
