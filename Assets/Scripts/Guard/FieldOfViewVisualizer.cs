using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class FieldOfViewVisualizer : MonoBehaviour
{
    FieldOfView fieldOfView;

    void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
    }

    void OnDrawGizmos()
    {
        if (fieldOfView == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, fieldOfView.viewRadius);

        Vector3 viewAngleA = fieldOfView.DirFromAngle(-fieldOfView.viewAngle / 2, false);
        Vector3 viewAngleB = fieldOfView.DirFromAngle(fieldOfView.viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * fieldOfView.viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * fieldOfView.viewRadius);

        if (fieldOfView.HasVisibleTargets())
        {
            Gizmos.color = Color.red;
            foreach (Transform target in fieldOfView.GetVisibleTargets())
            {
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }
}
