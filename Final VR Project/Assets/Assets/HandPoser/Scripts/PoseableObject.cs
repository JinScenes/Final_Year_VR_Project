using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseableObject : MonoBehaviour {

    [Header("Pose Type")]
    public PoseType poseType = PoseType.HandPose;

    [Header("Hand Pose Properties")]
    [Tooltip("Set this HandPose on the Handposer when PoseType is set to 'HandPose'")]
    public HandPose EquipHandPose;

    [Header("Auto Pose Properties")]
    [Tooltip("If PoseType = AutoPoseOnce, AutoPose will be run for this many seconds")]
    public float AutoPoseDuration = 0.15f;

    [Header("Animator Properties")]
    [Tooltip("Set animator ID to this value if PoseType is set to 'Animator'")]
    public int HandPoseID;

    public enum PoseType {
        HandPose,
        AutoPoseOnce,
        AutoPoseContinuous,
        Animator,
        Other,
        None
    }
}
