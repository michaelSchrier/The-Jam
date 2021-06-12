using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lazy.StateManagement
{
    [System.Serializable]
    public class StateMachine
    {
        private IState _currentState;
        public IState CurrentState 
        { 
            get
            {
                return _currentState;
            } 
        }

        private Dictionary<IState, List<ITransition>> _transitions = new Dictionary<IState, List<ITransition>>();
        private List<ITransition> _currentTransitions = new List<ITransition>();
        private List<ITransition> _anyTransitions = new List<ITransition>();

        private static List<ITransition> EmptyTransitions = new List<ITransition>(0);

        [SerializeField] private int framesSinceStateChange = 0;
        public int FramesSinceStateChange { get => framesSinceStateChange; }

        [SerializeField] private float timeSinceStateChange = 0;
        public float TimeSinceStateChange { get => timeSinceStateChange; }

        public Action<IState> OnStateChange;
        public Func<IState> BaseState;

        public bool pauseMachine = false;
        public bool redoCurrentState = false;
        [Space]
        [TextArea]
        public string debugState;

        public void Tick(float delta)
        {
            if (pauseMachine)
                return;

            var transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To, transition);

            _currentState?.OnUpdate(delta);

            timeSinceStateChange += delta;
            framesSinceStateChange++;
        }

        public IState ChangeState(IState state, ITransition fromTransition = null)
        {
            if (state == _currentState && !redoCurrentState)
                return _currentState;

            _currentState?.OnExit();

            if(fromTransition != null)
            {
                fromTransition.OnTransition();
            }
            OnStateChange?.Invoke(state);

            framesSinceStateChange = 0;
            timeSinceStateChange = 0;

            _currentState = state;

            _transitions.TryGetValue(state, out _currentTransitions);
            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;

            debugState = _currentState.ToString();
            _currentState.OnEnter();

            return _currentState;
        }

        public IState ToBaseState()
        {
            if(BaseState != null)
            {
                return ChangeState(BaseState());
            }

            return null;
        }

        public void AddTransition(IState from, IState to, Func<bool> condition, Action onTransition = null)
        {
            AddTransition(from, new Transition(to, condition, onTransition));
        }

        public void AddTransition(IState from, IState to, ICondition condition, Action onTransition = null)
        {
            AddTransition(from, new Transition(to, condition, onTransition));
        }
        
        public void AddTransition(IState from, ITransition toTransition)
        {
            if (_transitions.TryGetValue(from, out var transitions) == false)
            {
                transitions = new List<ITransition>();
                _transitions[from] = transitions;
            }

            transitions.Add(toTransition);
        }

        public void AddAnyTransition(IState to, Func<bool> condition, Action onTransition = null)
        {
            AddAnyTransition(new Transition(to, condition, onTransition));
        }

        public void AddAnyTransition(IState to, ICondition condition, Action onTransition = null)
        {
            AddAnyTransition(new Transition(to, condition, onTransition));
        }

        public void AddAnyTransition(ITransition newTransition)
        {
            _anyTransitions.Add(newTransition);
        }

        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition.Validate())
                    return transition;
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition.Validate())
                    return transition;
            }

            return null;
        }

        public class Transition : ITransition
        {
            public ICondition Condition { get; set; }
            public IState To { get; set; }
            public Action OnTransitionCall;

            public Transition(IState to, ICondition condition, Action onTransition = null)
            {
                To = to;
                Condition = condition;
                OnTransitionCall = onTransition;
            }

            public Transition(IState to, Func<bool> predicate, Action onTransition = null) : this(to, new Condition(predicate), onTransition) { }

            public void OnTransition()
            {
                OnTransitionCall?.Invoke();
            }
        }

        public class Condition : ICondition
        {
            public Func<bool> Value { get; private set; }

            public virtual bool Validate()
            {
                return Value.Invoke();
            }

            public Condition(Func<bool> predicate)
            {
                Value = predicate;
            }
        }
    }
}