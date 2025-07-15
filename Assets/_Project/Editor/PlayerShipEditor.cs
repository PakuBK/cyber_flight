using UnityEditor;
using UnityEngine;
using CF.Data;

[CustomEditor(typeof(PlayerShipManager))]
public class PlayerShipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerShipManager script = (PlayerShipManager)target;

    }

}
