using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [HideInInspector] public Block containedBlock;
    public Vector2 Pos => transform.position;
}
