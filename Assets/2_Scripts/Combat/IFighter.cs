using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFighter 
{
   public Collider MainCollider { get; }
   public GameObject GameObject { get; }
   
   public void TakeDamage();
}
