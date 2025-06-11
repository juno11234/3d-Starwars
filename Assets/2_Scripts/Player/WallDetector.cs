using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    [SerializeField] private float wallCheckDis = 0.6f;
    [SerializeField] private LayerMask wallLayer;

    public bool IsTouchingWall(out Vector3 wallNormal)
    {
        RaycastHit hit;
        Vector3 origin = transform.position;

        if (Physics.Raycast(origin, Vector3.right, out hit, wallCheckDis, wallLayer))
        {
            wallNormal = hit.normal;
            return true;
        }

        if (Physics.Raycast(origin, Vector3.left, out hit, wallCheckDis, wallLayer))
        {
            wallNormal = hit.normal;
            return true;
        }

        wallNormal = Vector3.zero;
        return false;
    }
}