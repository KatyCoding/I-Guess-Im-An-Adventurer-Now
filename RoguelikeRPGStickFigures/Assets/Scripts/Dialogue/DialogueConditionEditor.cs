using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using Unity.VisualScripting;
[System.Serializable]
[CustomEditor(typeof(DialogueCondition))]
public class DialogueConditionEditor : Editor
{
    [SerializeField]
    Object first;
    [SerializeField]
    Object second;
    [SerializeField]
    Object firstCache;
    [SerializeField]
    Object SecondCache;
    int firstIndex = 0;
    int firstIndexCache = 0;
    int secondIndex = 0;
    object currentSelection;
    string currentPath;
    private List<object> listedObjects = new List<object>();
    private List<string> listedPaths = new List<string>();
    private void OnEnable()
    {
        
    }
    
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var dialogue = target as DialogueCondition;
        first = EditorGUILayout.ObjectField(dialogue.First, typeof(Object), true);
        second = EditorGUILayout.ObjectField(dialogue.Second, typeof(Object), true);
        if(dialogue.First!=first||dialogue.Second!=second)
        {
            dialogue.First = first;
            dialogue.Second = second;
            EditorUtility.SetDirty(target);
            Debug.Log("SetDirty");
        }
        
        if (GUILayout.Button("Reset"))
        {
            listedObjects.Clear();
            listedPaths.Clear();
            currentPath = "";
            currentSelection = null;
            firstIndex = 0;
        }
        if (first != null)
        {
            if (currentSelection == null)
            {
                listedObjects.Add(first);
                listedPaths.Add(first.name);
                currentSelection = first;
                currentPath = first.name;
            }
            firstIndex = EditorGUILayout.Popup((currentPath == first.name)?0:1, BuildOptions(currentSelection, currentPath, (currentPath == first.name)?null:new string[1] {first.name}) );

            if (firstIndex < listedObjects.Count)
            {
                currentSelection = listedObjects[firstIndex];
                currentPath = listedPaths[firstIndex];
            }
            if (firstIndex != firstIndexCache)
            {
                listedObjects.Clear();
                listedPaths.Clear();
                listedObjects.Add(first);
                listedPaths.Add(first.name);
                listedObjects.Add(currentSelection);
                listedPaths.Add(currentPath);
                firstIndexCache = firstIndex;
            }


        }

        serializedObject.ApplyModifiedProperties();
    }

    
    string[] BuildOptions(object obj, string path, string[] addons = null)
    {
        List<string> options = new List<string>();
        if (addons != null)
        {
            options.AddRange(addons);
        }
        options.Add(path);
        var type = obj.GetType();
        if (type == typeof(GameObject))
        {
            var components = (obj as GameObject).GetComponents<Component>();
            foreach (var c in components)
            {
                options.Add(path + "/" + c.GetType().ToString());
                if (listedObjects.Contains(c))
                    continue;
                listedObjects.Add(c);
                listedPaths.Add(path + "/" + c.GetType().ToString());
            }
        }
        else
        {
            var fields = type.GetFields();
            var props = type.GetProperties();
            foreach (var f in fields)
            {

                if (f.GetAttribute<System.ObsoleteAttribute>() != null)
                    continue;
                options.Add(path + "/" + f.Name);
                if (listedObjects.Contains(f.GetValue(obj)))
                    continue;
                listedObjects.Add(f.GetValue(obj));
                listedPaths.Add(path + "/" + f.Name);

            }
            foreach (var p in props)
            {
                if (p.GetAttribute<System.ObsoleteAttribute>() != null)
                    continue;
                if (p.GetIndexParameters().Length != 0)
                {
                    continue;
                }
                options.Add(path + "/" + p.Name);
                if (listedObjects.Contains(p.GetValue(obj)))
                    continue;

                listedObjects.Add(p.GetValue(obj));
                listedPaths.Add(path + "/" + p.Name);

            }
        }
        return options.ToArray();

    }






    //drill down every field and property to find base values so any value is accessible
    //add methods with non void return values
    //figure out if i can use the method returns and drill deeper into those as options for values

    //current problem is that some objects like Vector3 contain Vector3. 
    //So when transform.position gets enumerated. one of the things it sees is position.normalized
    //which in turn tries to enumerate normalized which contains normalized which contains normalized...


    private string[] BuildOptions(Object obj)
    {
        AllOptions.Clear();
        ToExplore.Clear();
        fallback = 0;
        List<string> options = new List<string>();
        if (obj as GameObject != null)
        {
            var mono = obj as GameObject;
            var behaviors = mono.GetComponents<Component>();
            foreach (var b in behaviors)
            {
                if (b == null)
                    continue;
                var membs = GetMembers(b, b.name + "/" + b.GetType().ToString());
                if (membs == null)
                {
                    return null;
                }
                int failsafe = 0;
                ToExplore.AddRange(membs);
                while (ToExplore.Count > 0)
                {
                    failsafe++;
                    if (failsafe > 100)
                    {
                        Debug.LogWarning("stopping for possible infinite loop");
                        break;
                    }
                    var m = ToExplore[0];
                    if ((m.memberInfo as FieldInfo) is not null)
                        membs = GetMembers((m.memberInfo as FieldInfo).GetValue(m.obj), m.path);
                    else if (m.memberInfo as PropertyInfo is not null)
                        membs = GetMembers((m.memberInfo as PropertyInfo).GetValue(m.obj), m.path);
                    if (membs != null)
                        ToExplore.AddRange(membs);
                    ToExplore.Remove(m);
                }
            }
        }
        if (obj as ScriptableObject != null)
        {
            var membs = GetMembers(obj, obj.name);
            if (membs == null)
            {
                return null;
            }
            int failsafe = 0;
            ToExplore.AddRange(membs);
            while (ToExplore.Count > 0)
            {
                failsafe++;
                if (failsafe > 100)
                {
                    Debug.LogWarning("stopping for possible infinite loop");
                    break;
                }
                var m = ToExplore[0];
                if ((m.memberInfo as FieldInfo) is not null)
                    membs = GetMembers((m.memberInfo as FieldInfo).GetValue(m.obj), m.path);
                else if (m.memberInfo as PropertyInfo is not null)
                    membs = GetMembers((m.memberInfo as PropertyInfo).GetValue(m.obj), m.path);
                if (membs != null)
                    ToExplore.AddRange(membs);
                ToExplore.Remove(m);
            }

        }
        return AllOptions.ToArray();

    }
    private int fallback = 0;
    string[] DrillDownToBaseTypes(object obj, string path)
    {
        if (fallback > 100)
        {
            Debug.LogError("Recusive not work");
            return new string[0];
        }
        fallback++;
        List<string> vars = new List<string>();
        var t = obj.GetType();
        var fields = t.GetFields();
        var props = t.GetProperties();
        foreach (var f in fields)
        {

            if (f.GetAttribute<System.ObsoleteAttribute>() != null)
                continue;
            if (path.Contains("/" + f.Name + "/"))
            {
                Debug.Log("possibly stopping bad recursion?");
                continue;
            }
            if (f.FieldType.IsEnum)
            {
                //Debug.Log("Enum type encountered");
                continue;
            }
            var bt = CheckIfBaseType(f.FieldType);
            if (bt)
            {
                vars.Add(path + "/" + f.Name);
            }
            else vars.AddRange(DrillDownToBaseTypes(f.GetValue(obj), path + "/" + f.Name));

        }
        foreach (var p in props)
        {
            if (p.GetAttribute<System.ObsoleteAttribute>() != null)
                continue;
            if (path.Contains("/" + p.Name + "/"))
            {
                Debug.Log("possibly stopping bad recursion?");
                continue;
            }
            if (p.PropertyType.IsEnum)
            {
                //Debug.Log("Enum type encountered");
                continue;
            }
            var bt = CheckIfBaseType(p.PropertyType);
            if (bt)
            {
                vars.Add(path + "/" + p.Name);
            }
            else
            {
                if (p.GetIndexParameters().Length == 0)
                {
                    var val = p.GetValue(obj);
                    vars.AddRange(DrillDownToBaseTypes(val, path + "/" + p.Name));
                }

            }
        }
        return vars.ToArray();
    }
    string[] DrillDown2ElectricBoogaloo(object obj, string path)
    {
        if (obj == null)
        {
            return new string[0];
        }

        if (listedObjects.Contains(obj))
        {
            return new string[0];
        }
        listedObjects.Add(obj);
        List<string> vars = new List<string>();
        var t = obj.GetType();


        var fields = t.GetFields();
        var props = t.GetProperties();
        List<FieldInfo> usedFields = new List<FieldInfo>();
        List<PropertyInfo> usedProps = new List<PropertyInfo>();
        foreach (var f in fields)
        {
            if (f.GetAttribute<System.ObsoleteAttribute>() != null)
                continue;
            if (path.Contains(f.Name))
            {
                continue;
            }
            if (f.FieldType.IsEnum)
            {
                //Debug.Log("Enum type encountered");
                continue;
            }
            var bt = CheckIfBaseType(f.FieldType);
            if (bt)
            {
                vars.Add(path + "/" + f.Name);
            }
            else
            {
                usedFields.Add(f);
            }
        }
        foreach (var p in props)
        {
            if (p.GetAttribute<System.ObsoleteAttribute>() != null)
                continue;
            if (path.Contains(p.Name))
            {
                continue;
            }
            if (p.PropertyType.IsEnum)
            {
                //Debug.Log("Enum type encountered");
                continue;
            }

            if (p.GetIndexParameters().Length != 0)
            {
                continue;
            }
            var bt = CheckIfBaseType(p.PropertyType);
            if (bt)
            {
                vars.Add(path + "/" + p.Name);
            }
            else
            {
                usedProps.Add(p);
            }
        }
        /*
        foreach (var f in usedFields)
        {
            var toAdd = DrillDown2ElectricBoogaloo(f.GetValue(obj), path + "/" + f.Name);
            if (toAdd == null || toAdd.Length == 0)
            {
                return new string[0];
            }
            vars.AddRange(toAdd);
        }
        foreach (var p in usedProps)
        {
            var toAdd = DrillDown2ElectricBoogaloo(p.GetValue(obj), path + "/" + p.Name);
            if (toAdd==null||toAdd.Length==0)
            {
                return new string[0];
            }
            vars.AddRange(toAdd);
        }
        */
        return vars.ToArray();
    }
    private List<string> AllOptions = new List<string>();
    List<MemberInfo> asdf(object obj, string path)
    {
        var t = obj.GetType();
        var fields = t.GetFields();
        var props = t.GetProperties();
        List<FieldInfo> toRemove = new List<FieldInfo>();
        List<PropertyInfo> toRemoveProps = new List<PropertyInfo>();
        foreach (var f in fields)
        {
            if (CheckIfBaseType(f.FieldType))
            {
                toRemove.Add(f);
                continue;
            }
            AllOptions.Add(path + "/" + f.Name);
        }
        List<FieldInfo> fs = new List<FieldInfo>(fields);
        foreach (var r in toRemove)
        {
            if (fs.Contains(r))
                fs.Remove(r);
        }
        foreach (var p in props)
        {
            if (CheckIfBaseType(p.PropertyType))
            {
                toRemoveProps.Add(p);
                continue;
            }
            AllOptions.Add(path + "/" + p.Name);
        }
        List<PropertyInfo> ps = new List<PropertyInfo>(props);
        foreach (var p in toRemoveProps)
        {
            if (ps.Contains(p))
                ps.Remove(p);
        }
        fields = fs.ToArray();
        props = ps.ToArray();
        return null;
    }

    List<MemberPathWrapper> ToExplore = new List<MemberPathWrapper>();
    private class MemberPathWrapper
    {
        public MemberInfo memberInfo;
        public object obj;
        public string path;
    }
    private List<MemberPathWrapper> GetMembers(object obj, string path)
    {
        List<MemberPathWrapper> members = new List<MemberPathWrapper>();
        if (obj == null)
        {
            return null;
        }
        var t = obj.GetType();
        var fields = t.GetFields();
        var props = t.GetProperties();
        foreach (var f in fields)
        {
            if (f.GetAttribute<System.ObsoleteAttribute>() != null)
                continue;

            if (f.FieldType.IsEnum)
            {
                //Debug.Log("Enum type encountered");
                continue;
            }
            AllOptions.Add(path + "/" + f.Name);
            if (CheckIfBaseType(f.FieldType))
            {
                continue;
            }
            var wrapper = new MemberPathWrapper();
            wrapper.memberInfo = f;
            wrapper.path = path + "/" + f.Name;
            wrapper.obj = obj;
            if (ShouldStopExpandingMember(wrapper))
            {
                continue;
            }
            members.Add(wrapper);
        }
        foreach (var p in props)
        {
            if (p.GetAttribute<System.ObsoleteAttribute>() != null)
                continue;

            if (p.PropertyType.IsEnum)
            {
                //Debug.Log("Enum type encountered");
                continue;
            }
            AllOptions.Add(path + "/" + p.Name);
            if (CheckIfBaseType(p.PropertyType))
            {
                continue;
            }
            var wrapper = new MemberPathWrapper();
            wrapper.memberInfo = p;
            wrapper.path = path + "/" + p.Name;
            wrapper.obj = obj;
            if (ShouldStopExpandingMember(wrapper))
            {
                continue;
            }
            members.Add(wrapper);
        }
        return members;
    }
    bool CheckIfBaseType(System.Type t)
    {
        if (t == typeof(int)) return true;
        if (t == typeof(float)) return true;
        if (t == typeof(double)) return true;
        if (t == typeof(string)) return true;
        if (t == typeof(bool)) return true;
        if (t == typeof(uint)) return true;
        if (t == typeof(byte)) return true;
        if (t == typeof(long)) return true;
        if (t == typeof(char)) return true;
        if (t == typeof(System.Single)) return true;
        return false;
    }
    private bool ShouldStopExpandingMember(MemberPathWrapper mem)
    {
        foreach (var o in AllOptions)
        {
            if (SubstringFrequency(o, "/" + mem.memberInfo.Name + "/") > 2)
            {
                return true;
            }
        }
        return false;
    }
    private int SubstringFrequency(string og, string sub)
    {
        int count = 0;
        og = og.Replace(sub, "?");
        foreach (char c in og)
        {
            if (c == '?')
            {
                count++;
            }

        }
        return count;
    }
}
