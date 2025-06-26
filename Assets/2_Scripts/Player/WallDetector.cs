using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    
    private bool isTouchingWall;
    private Vector3 wallNormal;
    
    void OnTriggerStay(Collider other)
    {
        if ((wallLayer.value & (1 << other.gameObject.layer)) == 0)
            return;
        Vector3 closest = other.ClosestPoint(transform.position);
        wallNormal = transform.position - closest;
        isTouchingWall = true;
    }

    void OnTriggerExit(Collider other)
    {
        if ((wallLayer.value & (1 << other.gameObject.layer)) == 0)
            return;
        isTouchingWall = false;
    }

    public bool IsTouchingWall(out Vector3 normal)
    {
        normal = wallNormal;
        return isTouchingWall;
    }
}