using UnityEngine;
using CustomNameSpace;

namespace CustomNameSpace
{
    public class IdleState : State<CustomNameSpace.EnemyController_New>
    {
        public bool isPatrol = true;
        private float minIdleTime = 0.0f;
        private float maxIdleTime = 3.0f;
        private float idleTime = 0.0f;

        private Animator anim;
        private CharacterController controller;

        protected int hasMove = Animator.StringToHash("Move");
        protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");

        public override void OnInitialzed()
        {
            anim = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
        }

        public override void OnEnter()
        {
            anim?.SetBool(hasMove, false);
            anim?.SetFloat(hasMoveSpeed, 0);
            controller?.Move(Vector3.zero);

            if (isPatrol)
            {
                idleTime = Random.Range(minIdleTime, maxIdleTime);
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
            else if (isPatrol && stateMachine.ElapsedTimeInState > idleTime)    // 순찰 상태에서 일정 시간이 지나면 다음 순찰로 이동
            {
                stateMachine.ChangeState<MoveToWaypoint_New>();
            }
        }

        public override void OnExit()
        {

        }
    }
}
