using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(TopDownCamera))]
public class TopDownCamera_Editor : Editor
{
    #region Variables

    private TopDownCamera TargetCamera;

    #endregion Variables

    public override void OnInspectorGUI()
    {
        TargetCamera = (TopDownCamera)target;
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        if (!TargetCamera || !TargetCamera.target)
        {
            return;
        }

        Transform cameraTarget = TargetCamera.target;
        Vector3 targetPosition = cameraTarget.position;
        targetPosition.y += TargetCamera.lookAtHeight;

        // Draw distance circle
        Handles.color = new Color(1f, 0f, 0f, 0.15f);
        Handles.DrawSolidDisc(targetPosition, Vector3.up, TargetCamera.distance);

        Handles.color = new Color(0f, 0f, 1f, 0.5f);
        Handles.DrawWireDisc(targetPosition, Vector3.up, TargetCamera.distance);

        // Creare slider handles to adjust camera properties
        Handles.color = new Color(1f, 0f, 0f, 0.5f);
        TargetCamera.distance = TargetCamera.distance = Handles.ScaleSlider(TargetCamera.distance, targetPosition, -cameraTarget.forward, Quaternion.identity, TargetCamera.distance, 0.01f);
        TargetCamera.distance = Mathf.Clamp(TargetCamera.distance, 0.1f, float.MaxValue);

        Handles.color = new Color(0f, 0f, 1f, 0.5f);

        TargetCamera.height = Handles.ScaleSlider(TargetCamera.height, targetPosition, Vector3.up, Quaternion.identity, TargetCamera.height, 0.01f);
        TargetCamera.height = Mathf.Clamp(TargetCamera.height, 0.1f, float.MaxValue);

        GUIStyle style = new GUIStyle();
        style.fontSize = 15;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperCenter;

        Handles.Label(targetPosition + (-cameraTarget.forward * TargetCamera.distance), "Distance", style);

        style.alignment = TextAnchor.MiddleRight;
        Handles.Label(targetPosition + (Vector3.up * TargetCamera.height), "Height", style);

        TargetCamera.HandleCamera();
    }
}
