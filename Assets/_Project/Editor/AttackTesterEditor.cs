using UnityEditor;
using UnityEngine;
using CF.Testing;

[CustomEditor(typeof(AttackTester))]
public class AttackTesterEdtior : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AttackTester script = (AttackTester)target;
        
        if(GUILayout.Button("Attack"))
        {
            script.EnterAttack();
        }
        if(GUILayout.Button("Toggle Attack"))
        {
            script.ToggleAttack();
        }
    }
}
