using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using UnityEditor.SceneTemplate;

public class CombatMaster : MonoBehaviour
{
    public static CombatMaster instance;
    private int turnIndex = 0;
    private List<Combatant> CombatOrder = new List<Combatant>();
    public Action<Combatant> OnPlayerTurnStart;
    private Vector3 playerPositionCache;

    private Vector3 cameraPositionCache;
    //todo not this
    [SerializeField] private CombatantBehavior TEMPPlayerRef;
    public bool IsInCombat { get; private set; }
    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }
    #endregion

    public void Start()
    {
        List<Combatant> participants = new List<Combatant>();
        var combatants = FindObjectsByType<CombatantBehavior>(FindObjectsSortMode.None).ToList();
        foreach (var c in combatants)
        {
            participants.Add(c.combatant);
        }
        //EnterCombat(participants);
    }
    public void EnterCombat(List<Combatant> combatants)
    {
        if (IsInCombat)
            return;
        IsInCombat = true;
        CombatOrder.Clear();
        foreach (var c in combatants)
        {
            var initiativeRoll = DiceRoller.Instance.RollD20();
            c.CurrentInitiative = initiativeRoll + c.InitiativeBonus;
            c.OnDeath += OnCombantantDeath;
        }
        combatants.Sort((a, b) => { return b.CurrentInitiative.CompareTo(a.CurrentInitiative); });
        CombatOrder.AddRange(combatants);
        ScreenFade.instance.FadeToColorAndBack(Color.black, 1, 1, TransitionToCombatArena, CombatEntered);
    }
    private void TransitionToCombatArena()
    {
        playerPositionCache = TEMPPlayerRef.combatant.EntityTransformRef.position;
        
        foreach (var c in CombatOrder)
        {
            if (c.Team == 0)
            {
                c.EntityTransformRef.position = (CombatArena.Instance.TeamOneFirst.occupied) ?
                    (CombatArena.Instance.TeamOneSecond.occupied) ?
                    CombatArena.Instance.TeamOneThird.transform.position :
                    CombatArena.Instance.TeamOneSecond.transform.position :
                    CombatArena.Instance.TeamOneFirst.transform.position;
            }
            else
            {

                c.EntityTransformRef.position = (CombatArena.Instance.TeamTwoFirst.occupied) ?
                    (CombatArena.Instance.TeamTwoSecond.occupied) ?
                    CombatArena.Instance.TeamTwoThird.transform.position :
                    CombatArena.Instance.TeamTwoSecond.transform.position :
                    CombatArena.Instance.TeamTwoFirst.transform.position;

            }
        }
        cameraPositionCache = Camera.main.transform.position;
        Camera.main.transform.position =new Vector3(CombatArena.Instance.CameraFocus.position.x,CombatArena.Instance.CameraFocus.position.y,Camera.main.transform.position.z);
    }

    public static List<Combatant> GetPlayerTeam()
    {
        if (instance.TEMPPlayerRef == null)
            return null;
        List<Combatant> combatants = new List<Combatant>();
        combatants.Add(instance.TEMPPlayerRef.combatant);
        return combatants;
    }

    private void TransitionOutOfCombatArena()
    {
        TEMPPlayerRef.combatant.EntityTransformRef.position = playerPositionCache;
        Camera.main.transform.position = cameraPositionCache;
    }
    private void CombatEntered()
    {
        StartCombatTurn();

    }

    private void ExitCombat()
    {
        StartCoroutine(ExitCombatRoutine());
    }

    private IEnumerator ExitCombatRoutine()
    {
        yield return new WaitForSeconds(3);
        ScreenFade.instance.FadeToColorAndBack(Color.black,1,1,TransitionOutOfCombatArena);
        IsInCombat = false;

    }
    public void EndTurn()
    {
        turnIndex++;
        StartCombatTurn();
    }
    private void StartCombatTurn()
    {
        if (turnIndex > CombatOrder.Count - 1)
            turnIndex = 0;
        var currentCombatant = CombatOrder[turnIndex];
        currentCombatant.StartTurn();
    }

    public static void PlayerTurnStart(Combatant data)
    {
        instance.OnPlayerTurnStart?.Invoke(data);
    }
   
    public static List<Combatant> GetOpposingTeam(int team)
    {
        List<Combatant> otherTeam = new List<Combatant>();
        foreach (var c in instance.CombatOrder)
        {
            if(c.Team!=team)
            {
                otherTeam.Add(c);
            }
        }
        return otherTeam;
    }

    private void OnCombantantDeath(Combatant dead)
    {
    
        if(CombatOrder.Contains(dead))
            CombatOrder.Remove(dead);
        int team1Count = 0;
        int team2Count = 0;
        foreach (var c in CombatOrder)
        {
            if(c.Team ==0)
                team1Count++;
            else
                team2Count++;
            
        }

        if (team1Count == 0)
        {
            Debug.Log("Enemy wins combat");
            //combat lost
            return;
        }

        if (team2Count == 0)
        {
            
            //combat won
            ExitCombat();
        }
    }
}
