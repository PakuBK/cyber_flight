using UnityEditor;
using UnityEngine;
using CF.Data;

[CustomEditor(typeof(PlayerWeaponManager))]
public class PlayerWeaponEditor : Editor
{
        public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerWeaponManager script = (PlayerWeaponManager)target;
        
        if(GUILayout.Button("Create Weapons"))
        {
            script.CreateAllWeapons();
        }

        if(GUILayout.Button("Load Weapons"))
        {
            script.LoadPlayerWeapons();
        }
    }
}