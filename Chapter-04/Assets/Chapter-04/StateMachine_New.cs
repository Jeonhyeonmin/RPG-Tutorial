using System.Collections.Generic;
using System.Diagnostics;
using CustomNameSpace;

namespace CustomNameSpace
{

    public abstract class State<T>
    {
        protected StateMachine_New<T> stateMachine;
        protected T context;

        public State()
        {

        }

        internal void SetStateMachineAndContext(StateMachine_New<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;

            OnInitialzed();
        }

        public virtual void OnInitialzed()
        {

        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public abstract void Update(float delta);
    }

    public sealed class StateMachine_New<T>
    {
        private T context;

        private State<T> currentState;
        public State<T> CurrentState => currentState;

        private State<T> priviousState;
        public State<T> PriviousState => priviousState;

        private float elapsedTimeInState = 0.0f;
        public float ElapsedTimeInState => elapsedTimeInState;

        private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

        public StateMachine_New(T context, State<T> initialState)
        {
            this.context = context;

            // Setup our initial state
            AddState(initialState);
            currentState = initialState;
            currentState.OnEnter();
        }

        public void AddState(State<T> state)
        {
            state.SetStateMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;

            currentState.Update(deltaTime);
        }

        public R ChangeState<R>() where R : State<T>
        {
            var newType = typeof(R);

            if (currentState.GetType() == newType)
            {
                return currentState as R;
            }

            if (currentState != null)
            {
                currentState.OnExit();
            }

            priviousState = currentState;
            currentState = states[newType];
            currentState.OnEnter();
            elapsedTimeInState = 0.0f;

            return currentState as R;
        }
    }
}
