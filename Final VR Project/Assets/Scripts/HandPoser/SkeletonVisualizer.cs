using UnityEngine;

[ExecuteInEditMode]
public class SkeletonVisualizer : MonoBehaviour
{
    [SerializeField] private bool ShowGizmos = true;

    [Range(0.0f, 1f), SerializeField] private float JointRadius = 0.00875f;
    [Range(0.0f, 5f), SerializeField] private float BoneThickness = 3f;

    [SerializeField] private Color GizmoColor = new Color(255f, 255f, 255f, 0.5f);

    private bool ShowTransformNames = false;
    private bool isQuiting;

    private void OnApplicationQuit()
    {
        isQuiting = true;
    }

    private void OnDestroy()
    {
        if (isQuiting)
        {
            return;
        }

        if (Application.isEditor && !Application.isPlaying)
        {
            ResetEditorHandles();

            UnityEditor.SceneView.RepaintAll();
        }
    }

    private void OnDrawGizmos()
    {
        if (ShowGizmos)
        {
            Transform[] children = GetComponentsInChildren<Transform>();

            int childCount = children.Length;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = children[i];

                if (child == transform)
                {
                    continue;
                }

                bool isFingerTip = IsTipOfBone(child);
                float opacity = isFingerTip ? 0.5f : 0.1f;
                EditorHandle handle = child.gameObject.GetComponent<EditorHandle>();

                if (handle == null)
                {
                    handle = child.gameObject.AddComponent<EditorHandle>();

                    child.gameObject.hideFlags = HideFlags.None;
                    handle.hideFlags = HideFlags.HideInInspector;
                }

                handle.Radius = JointRadius / 1000f;
                handle.BaseColor = new Color(GizmoColor.r, GizmoColor.g, GizmoColor.b, opacity);
                handle.ShowTransformName = ShowTransformNames;

                if (child.parent != null)
                {
                    if (Vector3.Distance(child.position, child.parent.position) > 0.001f)
                    {
                        UnityEditor.Handles.DrawBezier(child.position, child.parent.position, child.position, child.parent.position, GizmoColor, null, BoneThickness);
                    }
                }
            }
        }
        else
        {
            ResetEditorHandles();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
    }

    public virtual bool IsTipOfBone(Transform fingerJoint)
    {
        if (fingerJoint.childCount == 0 && fingerJoint != transform && fingerJoint.parent != transform)
        {
            return true;
        }

        return false;
    }

    public void ResetEditorHandles()
    {
        EditorHandle[] handles = GetComponentsInChildren<EditorHandle>();
        for (int i = 0; i < handles.Length; i++)
        {
            if (handles[i] != null && handles[i].gameObject != null)
            {
                GameObject.DestroyImmediate((handles[i]));
            }
        }
    }
}