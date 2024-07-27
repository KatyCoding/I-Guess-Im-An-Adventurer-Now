using UnityEngine;

public class CombatArena : MonoBehaviour
{
    public static CombatArena Instance;
    public CombatSlot TeamOneFirst;
    public CombatSlot TeamOneSecond;
    public CombatSlot TeamOneThird;
    public CombatSlot TeamTwoFirst;
    public CombatSlot TeamTwoSecond;
    public CombatSlot TeamTwoThird;
    public Transform CameraFocus;
    [System.Serializable]
    public class CombatSlot
    {
        public Transform transform;
        public bool occupied;
    }
    public void Awake()
    {
        Instance = this; 
    }
}
