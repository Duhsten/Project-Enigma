using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(RoomData))]
public class RoomData_Inspector : Editor
{
    private SerializedObject m_Target;
    private SerializedProperty m_RoomData;
    void OnEnable()
    {
        m_Target = new SerializedObject(target);
        m_RoomData = m_Target.FindProperty("data");
    }
    public override void OnInspectorGUI()
    {
        // Draw default inspector properties
        DrawDefaultInspector();

        // Add custom inspector properties
        RoomData script = (RoomData)target;
        EditorGUILayout.PropertyField(m_RoomData, true);

    }
}
