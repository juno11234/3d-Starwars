using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    private CapsuleCollider triggerCollider;
    private bool isTouchingWall;
    private Vector3 wallNormal;

    void Awake()
    {
        triggerCollider = GetComponent<CapsuleCollider>();
        triggerCollider.isTrigger = true;
        // 트리거 크기는 Inspector에서 조정
    }

    void OnTriggerStay(Collider other)
    {
        if ((wallLayer.value & (1 << other.gameObject.layer)) == 0)
            return;
        Vector3 closest = other.ClosestPoint(transform.position);
        Vector3 dir = transform.position - closest;
        wallNormal = new Vector3(dir.x, 0f, dir.z).normalized;
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