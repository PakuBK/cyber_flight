using UnityEditor;
using UnityEngine;
using CF.Data;

[CustomEditor(typeof(AssetCreator))]
public class AssetCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AssetCreator script = (AssetCreator)target;

        if(GUILayout.Button("Create Shield Asset Data"))
        {
            script.CreateShieldAssets();
        }

        if(GUILayout.Button("Create Special Asset Data"))
        {
            script.CreateSpecialAssets();
        }

    }

}
