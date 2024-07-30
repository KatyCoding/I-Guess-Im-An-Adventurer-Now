using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using System.Collections.Generic;
[CustomEditor(typeof(DialogueCondition))]
public class DialogueConditionEditor : Editor
{
    Object first;
    Object second;

    int firstIndex = 0;
    int secondIndex = 0;


    private void OnEnable()
    {

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        first = EditorGUILayout.ObjectField(first, typeof(Object), true);
        second = EditorGUILayout.ObjectField(second, typeof(Object), true);

        if (first != null)
        {

            firstIndex = EditorGUILayout.Popup(firstIndex, BuildOptions(first));


        }


    }

    //drill down every field and property to find base values so any value is accessible
    //add methods with non void return values
    //figure out if i can use the method returns and drill deeper into those as options for values


    private string[] BuildOptions(Object obj)
    {
        List<string> options = new List<string>();   
        if(obj as GameObject!=null)
        {
            var mono = obj as GameObject;
            var behaviors = mono.GetComponents<Behaviour>();
            foreach(var b in behaviors)
            {
                var t = b.GetType();
                var fields = t.GetFields();
                var props = t.GetProperties();

                foreach(var f in fields)
                {
                    options.Add(t.ToString() + "/" + f.Name);
                }
                foreach(var p in props)
                {
                    options.Add(t.ToString() + "/" + p.Name);
                }
            }    
        }
        if(obj as ScriptableObject!=null)
        {
            var t = obj.GetType();
            var fields = t.GetFields();
            var props = t.GetProperties();

            foreach (var f in fields)
            {
                options.Add(t.ToString() + "/" + f.Name);
            }
            foreach (var p in props)
            {
                options.Add(t.ToString() + "/" + p.Name);
            }

        }
        return options.ToArray();
      
    }

}
