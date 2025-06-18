using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InGameEvent
{
    public enum EvenetType
    {
        Unknown,
        Combat,
    }

    public IFighter Sender { get; set; }
    public IFighter Reciever { get; set; }
    public abstract EvenetType Type { get; }
}

public class CombatEvent : InGameEvent
{
    public int Damage { get; set; }
    public Vector3 HitPosition { get; set; }
    public Collider Collider { get; set; }
    
    public override EvenetType Type => EvenetType.Combat;
}