using System.Linq;
using NUnit.Framework;
using PlasticGui;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(CombatantData.AttackDataAnimOverrideWrapper))]
public class AttackAnimOverrideEditor : PropertyDrawer
{
    private float PropertyHeight = EditorGUIUtility.singleLineHeight+5;
    //private int index = 0;
    private AnimatorControllerParameter[] choices;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        EditorGUI.BeginProperty(position, label, property);
        var data = property.FindPropertyRelative("AttackData");
        var animController = property.FindPropertyRelative("AnimController");
        var index = property.FindPropertyRelative("parameterIndex");
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            data, new GUIContent("Attack Data"));
        EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight +5, position.width, EditorGUIUtility.singleLineHeight),
            animController, new GUIContent("Anim Controller"));

        if (animController.objectReferenceValue as AnimatorController == null)
        {
            EditorGUI.EndProperty();
            return;
        }
        string[] popupItems;
        if (choices == null)
        {
            choices = (animController.objectReferenceValue as AnimatorController).parameters;
        }
        popupItems = BuildOptions(choices);
        string s = "Animation Parameter";
        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(s));
        EditorGUI.LabelField(
            new Rect(position.x, position.y + ((EditorGUIUtility.singleLineHeight + 5) * 2), textDimensions.x + 10,
                EditorGUIUtility.singleLineHeight), s);
        index.intValue = EditorGUI.Popup(new Rect(position.x+textDimensions.x+10, position.y+((EditorGUIUtility.singleLineHeight+5)*2), position.width-(textDimensions.x+10), EditorGUIUtility.singleLineHeight),index.intValue,
            BuildOptions((animController.objectReferenceValue as AnimatorController).parameters));
        
        property.serializedObject.ApplyModifiedProperties();
        property.serializedObject.Update();
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float h = EditorGUIUtility.singleLineHeight+5;
        float height = h*2;
        var animController = property.FindPropertyRelative("AnimController");
        if (animController.objectReferenceValue as AnimatorController != null)
            height +=h;
        return height;
    }
    private string[] BuildOptions(AnimatorControllerParameter[] vals)
    {
        string[] options = new string[vals.Length];
        for (int i = 0; i < vals.Length; i++)
        {
            options[i] = vals[i].name;
        }

        return options;
    }
    
}
