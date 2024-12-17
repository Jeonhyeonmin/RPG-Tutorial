using UnityEngine;
using CustomNameSpace;
using UnityEngine.AI;

namespace CustomNameSpace
{
    public class MoveState : State<CustomNameSpace.EnemyController_New>
    {
        private Animator anim;
        private CharacterController controller;
        private NavMeshAgent agent;

        private int hashMove = Animator.StringToHash("Move");
        private int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

        public override void OnInitialzed()
        {
            anim = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
            agent = context.GetComponent<NavMeshAgent>();
        }

        public override void OnEnter()
        {
            agent?.SetDestination(context.Target.position);
            anim?.SetBool(hashMove, true);
        }

        public override void Update(float deltaTime)
        {
            Transform enemy = context.SearchEnemy();

            if (enemy)
            {
                Debug.Log("적 발견");
                agent.SetDestination(context.Target.position);

                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    controller.Move(agent.velocity * deltaTime);
                    anim.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, deltaTime);
                    return;
                }
            }

            if (!enemy)
            {
                Debug.Log("사정거리 접촉");
                stateMachine.ChangeState<IdleState>();
            }
        }

        public override void OnExit()
        {
            anim?.SetBool(hashMove, false);
            anim?.SetFloat(hashMoveSpeed, 0);
            agent.ResetPath();
        }
    }
}
