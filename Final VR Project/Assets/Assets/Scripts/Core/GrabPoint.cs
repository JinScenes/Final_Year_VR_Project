using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    public HandPoseType handPoseType = HandPoseType.HandPose;
    public HandPose SelectedHandPose;

    public HandPoseId HandPose;

    [Header("Valid Hands")]
    public bool LeftHandIsValid = true;
    public bool RightHandIsValid = true;

    [Tooltip("If specified, the Hand Model will be parented here when snapped")]
    public Transform HandPosition;

    [Header("Angle Restriction")]
    [Range(0.0f, 360.0f)] public float MaxDegreeDifferenceAllowed = 360;

    [Header("Finger Blending")]
    [Range(0.0f, 1.0f)] public float IndexBlendMin = 0;
    [Range(0.0f, 1.0f)] public float IndexBlendMax = 0;
    [Range(0.0f, 1.0f)] public float ThumbBlendMin = 0;
    [Range(0.0f, 1.0f)] public float ThumbBlendMax = 0;

    Vector3 previewModelOffsetLeft = new Vector3(0.007f, -0.0179f, 0.0071f);
    Vector3 previewModelOffsetRight = new Vector3(-0.029f, 0.0328f, 0.044f);

    [Header("Editor")]
    [Tooltip("Show a green arc in the Scene view representing MaxDegreeDifferenceAllowed")]
    public bool ShowAngleGizmo = true;

    #region Editor
#if UNITY_EDITOR
    // Make sure animators update in the editor mode to show hand positions
    // By using OnDrawGizmosSelected we only call this function if the object is selected in the editor
    void OnDrawGizmosSelected()
    {
        DrawEditorArc();

        UpdatePreviews();
        //if (!Application.isPlaying) {
        //    UpdatePreviews();
        //}
    }

    // Update preview transform in editor in play mode as well
    //void Update() {
    //    UpdatePreviews();
    //}

    public void UpdatePreviews()
    {
        UpdateChildAnimators();
        UpdatePreviewTransforms();
        UpdateHandPosePreview();
        UpdateAutoPoserPreview();
    }

    /// <summary>
    /// Draw an arc in the editor representing MaxDegreeDifferenceAllowed
    /// </summary>
    public void DrawEditorArc()
    {

        // Draw arc representing the MaxDegreeDifferenceAllowed of the Grab Point
        if (ShowAngleGizmo && MaxDegreeDifferenceAllowed != 0 && MaxDegreeDifferenceAllowed != 360)
        {
            Vector3 from = Quaternion.AngleAxis(-0.5f * MaxDegreeDifferenceAllowed, transform.up) * (-transform.forward - Vector3.Dot(-transform.forward, transform.up) * transform.up);

            UnityEditor.Handles.color = new Color(0, 1, 0, 0.1f);
            UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, from, MaxDegreeDifferenceAllowed, 0.05f);
        }
    }
#endif

    bool offsetFound = false;

    public void UpdatePreviewTransforms()
    {
        Transform leftHandPreview = transform.Find("LeftHandModelsEditorPreview");
        Transform rightHandPreview = transform.Find("RightHandModelsEditorPreview");

        if (!offsetFound)
        {
            // If there is a Hand in the scene, use that offset instead of our defaults
            if (GameObject.Find("LeftController/Grabber") != null)
            {
                Grabber LeftGrabber = GameObject.Find("LeftController/Grabber").GetComponent<Grabber>();
                previewModelOffsetLeft = Vector3.zero - LeftGrabber.transform.localPosition;
                // offsetFound = true;
            }

            if (GameObject.Find("RightController/Grabber") != null)
            {
                Grabber RightGrabber = GameObject.Find("RightController/Grabber").GetComponent<Grabber>();
                previewModelOffsetRight = Vector3.zero - RightGrabber.transform.localPosition;
                // offsetFound = true;
            }
        }

        if (leftHandPreview)
        {
            leftHandPreview.localPosition = previewModelOffsetLeft;
            leftHandPreview.localEulerAngles = Vector3.zero;
        }

        if (rightHandPreview)
        {
            rightHandPreview.localPosition = previewModelOffsetRight;
            rightHandPreview.localEulerAngles = Vector3.zero;
        }
    }

    public void UpdateHandPosePreview()
    {
        if (handPoseType == HandPoseType.HandPose)
        {
            Transform leftHandPreview = transform.Find("LeftHandModelsEditorPreview");
            Transform rightHandPreview = transform.Find("RightHandModelsEditorPreview");

            if (leftHandPreview)
            {
                HandPoser hp = leftHandPreview.GetComponentInChildren<HandPoser>();
                if (hp != null)
                {
                    hp.CurrentPose = SelectedHandPose;
                }
            }

            if (rightHandPreview)
            {
                HandPoser hp = rightHandPreview.GetComponentInChildren<HandPoser>();
                if (hp != null)
                {
                    hp.CurrentPose = SelectedHandPose;
                }
            }
        }
    }

    public void UpdateAutoPoserPreview()
    {
        if (handPoseType == HandPoseType.AutoPoseContinuous || handPoseType == HandPoseType.AutoPoseOnce)
        {
            Transform leftHandPreview = transform.Find("LeftHandModelsEditorPreview");
            Transform rightHandPreview = transform.Find("RightHandModelsEditorPreview");
            // Update in editor
            if (leftHandPreview)
            {
                AutoPoser ap = leftHandPreview.GetComponentInChildren<AutoPoser>();
                if (ap != null)
                {
                    ap.UpdateContinuously = true;
                }
            }

            if (rightHandPreview)
            {
                AutoPoser ap = rightHandPreview.GetComponentInChildren<AutoPoser>();
                if (ap != null)
                {
                    ap.UpdateContinuously = true;
                }
            }
        }
        else
        {
            Transform leftHandPreview = transform.Find("LeftHandModelsEditorPreview");
            Transform rightHandPreview = transform.Find("RightHandModelsEditorPreview");
            // Update in editor
            if (leftHandPreview)
            {
                AutoPoser ap = leftHandPreview.GetComponentInChildren<AutoPoser>();
                if (ap != null)
                {
                    ap.UpdateContinuously = false;
                }
            }

            if (rightHandPreview)
            {
                AutoPoser ap = rightHandPreview.GetComponentInChildren<AutoPoser>();
                if (ap != null)
                {
                    ap.UpdateContinuously = false;
                }
            }
        }
    }

    public void UpdateChildAnimators()
    {
        var animators = transform.GetComponentsInChildren<Animator>(true);
        for (int x = 0; x < animators.Length; x++)
        {
            if (handPoseType == HandPoseType.AnimatorID)
            {
                animators[x].enabled = true;
                if (animators[x].isActiveAndEnabled)
                {
                    animators[x].Update(Time.deltaTime);
                }

#if UNITY_EDITOR && (UNITY_2019 || UNITY_2020)
                // Only set dirty if not in prefab mode
                if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null) {
                    UnityEditor.EditorUtility.SetDirty(animators[x].gameObject);
                }
#endif
            }
            // Disable the animator in editor mode if using handpose
            else if (handPoseType == HandPoseType.HandPose && SelectedHandPose != null)
            {
                animators[x].enabled = false;
            }
            // Disable the animator in editor mode if using auto pose
            else if (handPoseType == HandPoseType.AutoPoseOnce || handPoseType == HandPoseType.AutoPoseContinuous)
            {
                animators[x].enabled = false;
            }
        }
    }

    #endregion
}