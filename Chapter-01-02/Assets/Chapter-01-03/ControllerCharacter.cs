using UnityEngine;
using UnityEngine.AI;

public class ControllerCharacter : MonoBehaviour
{
    #region Variables

    private CharacterController characterController;
    private NavMeshAgent navMeshAgent;
    private Camera mainCamera;
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

    #endregion Variables

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = true;

        mainCamera = Camera.main;
    }

    private void Update()
    {
       // Process mouse left button input
       if (Input.GetMouseButtonDown(0))
       {
            // Make ray from screen to world
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Check hit from ray
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
            {
                Debug.Log("We Hit : " + hit.collider.name + " " + hit.point);

                // move our character to what we hit
                navMeshAgent.SetDestination(hit.point);
            }

            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                characterController.Move(navMeshAgent.velocity * Time.deltaTime);
                Debug.Log(navMeshAgent.velocity);
            }
            else
            {
                characterController.Move(Vector3.zero);
            }
       }
    }

    private void LateUpdate()
    {
        // Update the position of the character controller to match the position of the NavMeshAgent
        transform.position = navMeshAgent.nextPosition;
    }
}
