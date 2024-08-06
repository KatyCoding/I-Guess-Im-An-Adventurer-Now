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
    /*
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var vis =  base.CreatePropertyGUI(property);
        return vis;
        var selection = new PopupField<AnimatorControllerParameter>();
        var animController = property.FindPropertyRelative("animController");
        if (animController == null)
            return vis;
        selection.choices = (animController.objectReferenceValue as AnimatorController).parameters.ToList();
        vis.Add(selection);
        return vis;
    }
*/
    private float PropertyHeight = EditorGUIUtility.singleLineHeight+5;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        EditorGUI.BeginProperty(position, label, property);
        var data = property.FindPropertyRelative("data");
        var animController = property.FindPropertyRelative("AnimController");
        //var param = property.FindPropertyRelative("Parameter");
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            data, new GUIContent("Attack Data"));
        EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight +5, position.width, EditorGUIUtility.singleLineHeight),
            animController, new GUIContent("Anim Controller"));
        
        if (animController.objectReferenceValue as AnimatorController == null)
            return;
        var val = EditorGUI.Popup(new Rect(position.x, position.y+((EditorGUIUtility.singleLineHeight+5)*2), position.width, EditorGUIUtility.singleLineHeight),0,
            BuildOptions((animController.objectReferenceValue as AnimatorController).parameters));
            
        EditorGUI.EndProperty();
    }
    
    /*
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        EditorGUI.BeginProperty(position, label, property);
        var data = property.FindPropertyRelative("data");
        var animController = property.FindPropertyRelative("AnimController");
        var dataRect = new Rect(position.x,position.y,position.width,EditorGUIUtility.singleLineHeight);
        var animRect = new Rect(position.x,position.y + EditorGUIUtility.singleLineHeight +5,position.width,EditorGUIUtility.singleLineHeight);
        var goRect = new Rect(position.x,position.y+((EditorGUIUtility.singleLineHeight + 5)*2),position.width,EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(animRect, animController, new GUIContent("Anim Controller"));
        EditorGUI.PropertyField(dataRect, data, new GUIContent("Attack Data"));
        EditorGUI.EndProperty();
    }
*/
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float h = EditorGUIUtility.singleLineHeight+5;
        float height = h*2;
        var animController = property.FindPropertyRelative("AnimController");
        if (animController.objectReferenceValue as AnimatorController == null)
            height +=h;
        return height;
    }
    

    /*
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var animController = property.FindPropertyRelative("AnimController");
        var rect = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.PropertyField(rect, animController,new GUIContent("Anim Controller"));
        EditorGUI.EndProperty();
    }
    */
    private string[] BuildOptions(object[] vals)
    {
        string[] options = new string[vals.Length];
        for (int i = 0; i < vals.Length; i++)
        {
            options[i] = vals[i].ToString();
        }

        return options;
    }
}
