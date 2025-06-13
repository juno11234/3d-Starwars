using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSysytem : MonoBehaviour
{
    private const int Max_Event_COUNT = 10;

    public class Callback //이벤트추가
    {
        public Action<CombatEvent> OnCombatEvent;
    }

    public static CombatSysytem Instance;

    private Dictionary<Collider, IFighter> monsterDictionary = new Dictionary<Collider, IFighter>();
    private Queue<InGameEvent> inGameEventQueue = new Queue<InGameEvent>();
    public readonly Callback Events = new Callback();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        int processCount = 0;

        while (inGameEventQueue.Count > 0 && processCount < Max_Event_COUNT)
        {
            var inGameEvent = inGameEventQueue.Dequeue();

            switch (inGameEvent.Type)
            {
                case InGameEvent.EvenetType.Combat:
                    var combatEvent = inGameEvent as CombatEvent;
                    inGameEvent.Reciever.TakeDamage(combatEvent);
                    break;
            }
            
            processCount++;
        }
    }

    public void RegisterMonster(IFighter monster)
    {
        if (monsterDictionary.TryAdd(monster.MainCollider, monster) == false)
        {
            Debug.Log("몬스터가 이미존재 덮어씀");
        }
    }
    
    
}