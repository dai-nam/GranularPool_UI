using UnityEngine;
using UnityEditor;

    [CustomEditor(typeof(ViewManager))]
    public class ViewManagerEditor : Editor
    {
    public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ViewManager viewManager = (ViewManager) target;
            if (GUILayout.Button("Change View"))
            {
                viewManager.InvokeViewChange();
            }
        }
    }
