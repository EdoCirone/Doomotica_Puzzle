using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovingForniture : GenericForniture
{
    [Header("Waypoint Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float arriveThreshold = 0.3f;   // distanza considerata “arrivato”
    [SerializeField] private float rotationSpeed = 180f;     // gradi per secondo
    [SerializeField] private float waitBeforeMove = 0.3f;    // tempo di pausa tra arrivo e ripartenza

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    private bool isRotating = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // disattiva la rotazione automatica
        agent.updateUpAxis = true;

        if (waypoints.Length > 0)
        {
            MoveToNextWaypoint();
        }
    }

    protected void OnMouseDown()
    {
        if (isMoving || isRotating) return; // evitiamo di interrompere la sequenza
        if (waypoints.Length == 0) return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        StartCoroutine(MoveAndRotateSequence());
    }

    private IEnumerator MoveAndRotateSequence()
    {
        //  Muovi verso waypoint corrente
        yield return MoveTo(waypoints[currentWaypointIndex].position);

        //  Attendi un po’
        yield return new WaitForSeconds(waitBeforeMove);

        //  Calcola il prossimo waypoint (per sapere dove ruotare)
        int nextIndex = (currentWaypointIndex + 1) % waypoints.Length;

        //  Ruota verso il prossimo
        yield return RotateTowards(waypoints[nextIndex].position);
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        isMoving = true;
        agent.isStopped = false;
        agent.SetDestination(destination);

        // finché non arriva
        while (Vector3.Distance(transform.position, destination) > arriveThreshold)
        {
            yield return null;
        }

        agent.isStopped = true;
        isMoving = false;
    }

    private IEnumerator RotateTowards(Vector3 targetPos)
    {
        isRotating = true;

        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
            yield return null;
        }

        isRotating = false;
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length < 2) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 from = waypoints[i].position;
            Vector3 to = waypoints[(i + 1) % waypoints.Length].position;
            Gizmos.DrawLine(from, to);
            Gizmos.DrawSphere(waypoints[i].position, 0.1f);
        }
    }
}
