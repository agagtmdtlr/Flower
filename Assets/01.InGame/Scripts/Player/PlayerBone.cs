using System.Collections;
using System.Collections.Generic;
using Obi;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerBone : MonoBehaviour
{
    public RotateBone rotateBone;
    public BoneMovementController movementController;

    public PlayerBone attached { get; set; }
}
