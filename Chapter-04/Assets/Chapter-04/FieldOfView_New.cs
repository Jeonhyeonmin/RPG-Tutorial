using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView_New : MonoBehaviour
{
    public float viewRadius = 5f;
    [Range(0, 360)]
    public float viewAngle = 90f;

    public LayerMask targetMask;
    public LayerMask obstableMask;

    private List<Transform> visibleTarges = new List<Transform>();
    public List<Transform> VisibleTargets
    {
        get
        {
            return visibleTarges;
        }
    }
    [SerializeField] private Transform nearestTarget;
    public Transform NearestTarget => nearestTarget;
    public float delay = 0.2f;
    private float distanceToTarget = 0.0f;
    

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(delay));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        distanceToTarget = 0.0f;
        nearestTarget = null;
        visibleTarges.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; ++i)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstableMask))
                {
                    visibleTarges.Add(target);

                    if (nearestTarget == null || (distanceToTarget > dstToTarget))
                    {
                        nearestTarget = target;
                        distanceToTarget = dstToTarget;
                    }
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrres, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrres += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrres * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrres * Mathf.Deg2Rad));
    }
}
