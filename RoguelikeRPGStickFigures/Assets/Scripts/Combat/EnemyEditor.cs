using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;
public class EnemyEditor : EditorWindow
{
    public CombatantStorage storage;
    [MenuItem("Enemies/EnemyEditor")]
    public static void ShowExample()
    {
        EnemyEditor wnd = GetWindow<EnemyEditor>();
        wnd.titleContent = new GUIContent("MyEditorWindow");
    }

    public void CreateGUI()
    {
        rootVisualElement.Clear();
        storage ??= new CombatantStorage();
        LoadEnemies();
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy


        foreach (var c in storage.AllCombatantTypes)
        {
            Label l = new Label(c.Name);
            root.Add(l);
            
            IntegerField strength = new IntegerField("Strength");
            strength.value = c.Strength.CurrentMinValue;
            l.Add(strength);
            IntegerField dex = new IntegerField("Dexterity");
            strength.value = c.Dexterity.CurrentMinValue;
            l.Add(dex);

        }

        // Create button
        Button button = new Button();
        button.name = "button";
        button.text = "TEST SAVE DATA";
        button.clicked += SaveEnemies;
        root.Add(button);

        // Create toggle
        Button button2 = new Button();
        button2.name = "button2";
        button2.text = "TEST LOAD DATA";
        button2.clicked += LoadEnemies;
        root.Add(button2);

        Button button3 = new Button();
        button3.name = "button2";
        button3.text = "CLEAR DATA";
        button3.clicked += ClearData;
        root.Add(button3);
        Button refresh = new Button();
        refresh.name = "refresh";
        refresh.text = "Refresh";
        refresh.clicked += CreateGUI;
        root.Add(refresh);
    }

    private void SaveEnemies()
    {
        storage ??= new CombatantStorage();
        Slime s = new Slime(new Stat(2), new Stat(2), new Stat(2), new Stat(2), new Stat(2));
        s.SetName("Slime " + storage.AllCombatantTypes.Count.ToString());
        storage.AddCombatantType(s);
        var json = JsonUtility.ToJson(storage);
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/CombatantData.json");
        writer.Write(json);
        writer.Close();

    }
    private void LoadEnemies()
    {
        if (!File.Exists(Application.persistentDataPath + "/CombatantData.json"))
        {
            Debug.LogError("Combatant Stats do not exist. Creating File");
            var stream = File.Create(Application.persistentDataPath + "/CombatantData.json");
            stream.Close();
            return;
        }

        var reader = new StreamReader(Application.persistentDataPath + "/CombatantData.json");
        var json = reader.ReadToEnd();
        reader.Close();
        if (json == "")
        {
            Debug.LogError("EMPTY DATA");
            return;
        }
        storage = JsonUtility.FromJson<CombatantStorage>(json);
        foreach (var c in storage.AllCombatantTypes)
        {
            Debug.Log(c.Name);
        }
    }
    private void ClearData()
    {
        File.Delete(Application.persistentDataPath + "/CombatantData.json");
        storage = null;
    }
}
[System.Serializable]
public class CombatantStorage
{
    public CombatantStorage()
    {
        AllCombatantTypes = new List<Combatant>();
    }
    public List<Combatant> AllCombatantTypes;
    public void AddCombatantType(Combatant c)
    {
        AllCombatantTypes.Add(c);
    }



}