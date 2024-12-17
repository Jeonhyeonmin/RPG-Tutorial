using CustomNameSpace;
using UnityEngine;
using UnityEngine.AI;

public class MoveToWaypoint_New : State<EnemyController_New>
{
    private Animator anim;
    private CharacterController controller;
    private NavMeshAgent agent;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void OnInitialzed()
    {
        anim = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
        agent = context.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        if (context.targetWaypoint == null)
        {
            context.FindNextWaypoint();
        }

        if (context.targetWaypoint)
        {
            agent?.SetDestination(context.targetWaypoint.position);
            anim?.SetBool(hasMove, true);
        }
    }

    public override void Update(float delta)
    {
        Transform enemy = context.SearchEnemy();

        if (enemy)
        {
            if (context.IsAvailableAttack)
            {
                Debug.Log("공격");
                stateMachine.ChangeState<AttackState>();
            }
            else
            {
                Debug.Log("이동");
                stateMachine.ChangeState<MoveState>();
            }
        }
        else
        {
            if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
            {
                Transform nextDest = context.FindNextWaypoint();

                if (nextDest)
                {
                    agent.SetDestination(nextDest.position);
                }

                stateMachine.ChangeState<IdleState>();
            }
            else
            {
                controller.Move(agent.velocity * delta);
                anim.SetFloat(hasMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, delta);
            }
        }
    }

    public override void OnExit()
    {
        anim?.SetBool(hasMove, false);
        agent.ResetPath();
    }
}
