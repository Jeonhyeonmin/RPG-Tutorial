using UnityEngine;

namespace CustomNameSpace
{
    public class EnemyController_New : MonoBehaviour
    {
        #region Variables

        protected StateMachine_New<EnemyController_New> stateMachine;
        public StateMachine_New<EnemyController_New> StateMachine => stateMachine;

        private FieldOfView_New fov;

        //public LayerMask targetMask;
        //public Transform target;
        //public float viewRaius;
        public float attackRange;
        public Transform Target => fov?.NearestTarget;
        public Transform[] waypoints;
        [HideInInspector]
        public Transform targetWaypoint = null;
        private int waypointIndex = 0;

        private CharacterController controller;

        #endregion Variables

        #region Unity Methods

        private void Start()
        {
            stateMachine = new StateMachine_New<EnemyController_New>(this, new MoveToWaypoint_New());
            IdleState idleState = new IdleState();
            idleState.isPatrol = true;
            stateMachine.AddState(idleState);
            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            fov = GetComponent<FieldOfView_New>();
        }

        private void Update()
        {
            stateMachine.Update(Time.deltaTime);

            Debug.Log(stateMachine.CurrentState);
        }

        #endregion Unity Methods

        #region Other Methods
        
        /// <summary>
        /// Gets a value indicating whether the enemy is available to attack the target.
        /// </summary>
        /// <value>
        /// <c>true</c> if the enemy is within attack range of the target; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                {
                    return false;
                }

                float distance = Vector3.Distance(transform.position, Target.position);
                Debug.Log(distance <= attackRange);
                return (distance <= attackRange);
            }
        }

        public Transform SearchEnemy()
        {
            return Target;
            // Target = null;
            // Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRaius, targetMask);

            // if (targetInViewRadius.Length > 0)
            // {
            //     target = targetInViewRadius[0].transform;
            // }

            // return target;
        }

        #endregion Other Methods

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(transform.position, viewRaius);

        //     Gizmos.color = Color.green;
        //     Gizmos.DrawWireSphere(transform.position, attackRange);
        // }

        public Transform FindNextWaypoint()
        {
            targetWaypoint = null;

            if (waypoints.Length > 0)
            {
                targetWaypoint = waypoints[waypointIndex];
            }

            waypointIndex = (waypointIndex + 1) % waypoints.Length;

            return targetWaypoint;
        }
    }
}
