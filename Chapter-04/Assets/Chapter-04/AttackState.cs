using UnityEngine;
using CustomNameSpace;

namespace CustomNameSpace
{
    public class AttackState : State<EnemyController_New>
    {
        private Animator anim;
        private int hashAttack = Animator.StringToHash("Attack");

        public override void OnInitialzed()
        {
            anim = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            if (context.IsAvailableAttack)
            {
                anim?.SetTrigger(hashAttack);
                Debug.Log(hashAttack);
            }
            else
            {
                stateMachine.ChangeState<IdleState>();
            }
        }

        public override void Update(float deltaTime)
        {
            
        }
    }
}
