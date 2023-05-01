using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveController : MonoBehaviour
{
    public bool canMove = true;
    public float timeCantMove = 0f;
    public float slowdownTime = 0f;
    public float slowdownPower = 0f;
}
