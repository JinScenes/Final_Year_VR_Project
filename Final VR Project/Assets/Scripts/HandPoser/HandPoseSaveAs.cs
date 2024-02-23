using UnityEditor;
using UnityEngine;

public class HandPoseSaveAs : EditorWindow
{
    [SerializeField] private string PoseName = "HandPose";

    private HandPoser inspectedPoser;
    private GUIStyle rt;

    public void ShowWindow(HandPoser poser)
    {
        EditorWindow.GetWindow(typeof(HandPoseSaveAs));

        inspectedPoser = poser;

        if (inspectedPoser != null && inspectedPoser.CurrentPose != null)
        {
            PoseName = inspectedPoser.CurrentPose.name;
        }
        else
        {
            PoseName = "Default";
        }

        const int width = 480;
        const int height = 220;

        var x = (Screen.currentResolution.width - width) / 2;
        var y = (Screen.currentResolution.height - height) / 2;

        GetWindow<HandPoseSaveAs>("Save HandPose As...").position = new Rect(x, y, width, height);
    }

    public void Save()
    {
        inspectedPoser.SavePoseAsScriptablObject(PoseName);
        GetWindow<HandPoseSaveAs>().Close();

        HandPose newPose = Resources.Load<HandPose>(PoseName);
        if (newPose)
        {
            inspectedPoser.CurrentPose = newPose;
        }

    }

    public void SaveAs()
    {
        GetWindow<HandPoseSaveAs>().Close();

        string path = EditorUtility.SaveFilePanelInProject("Save Hand Pose", "HandPose", "asset", "Please enter a file name to save the hand pose");
        if (path.Length != 0)
        {
            var poseObject = inspectedPoser.GetHandPoseScriptableObject();

            AssetDatabase.CreateAsset(poseObject, path);
            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();

            inspectedPoser.CurrentPose = poseObject;
        }
    }


    private void OnGUI()
    {
        GUI.changed = false;

        if (rt == null)
        {
            rt = new GUIStyle(EditorStyles.label);
            rt.richText = true;
        }

        EditorGUILayout.HelpBox("Hand poses are stored in a Resources directory. \nThis allows them to be loaded at runtime using Resources.Load().", MessageType.Info);
        EditorGUILayout.Separator();
        GUILayout.Label("Save Handpose As...", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        GUILayout.Label("Name : ", EditorStyles.boldLabel);
        PoseName = EditorGUILayout.TextField(PoseName, EditorStyles.textField);

        if (GUI.changed) { }

        string formattedName = PoseName;
        if (!string.IsNullOrEmpty(formattedName))
        {
            formattedName += ".asset";
        }

        EditorGUILayout.Separator();

        if (inspectedPoser != null)
        {
            GUILayout.Label("<b>Will save to : </b>", rt);
            GUILayout.Label("<i>" + inspectedPoser.ResourcePath + formattedName + "</i>", rt);
        }

        EditorGUILayout.Separator();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(" Save as... ", EditorStyles.miniButton))
        {
            SaveAs();
        }

        var initialColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(" Save ", EditorStyles.miniButton))
        {
            Save();
        }

        GUI.backgroundColor = initialColor;
        GUILayout.EndHorizontal();
    }
}
