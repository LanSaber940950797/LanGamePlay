using System.Collections.Generic;
using ET;
using TEngine;
using Log = TEngine.Log;

namespace GameLogic.Battle
{
   

    [EntitySystemOf(typeof(FrameStateComponent))]
    public static partial class FrameStateComponentSystem
    {

        [EntitySystem]
        public static void Awake(this FrameStateComponent self)
        {
            self.InputFrames.Clear();
        }
        
        public static void Update(this FrameStateComponent self, OneFrameStates oneFrameStates, int frame,
            long noUpdateId = -1)
        {
            foreach (var kv in oneFrameStates.States)
           {
               var states = kv.Value;
               if (states.States.Count == 0)
               {
                   continue;
               }
               
               if (kv.Key == noUpdateId)
               {
                   continue;
               }
               
               self.UpdateState(states);
           }
        }

        public static void LateUpdate(this FrameStateComponent self, OneFrameStates oneFrameStates, long playerId)
        {
            if (self.InputFrames.Count <= 0)
            {
                return;
            }
            if (!oneFrameStates.States.TryGetValue(playerId, out var states))
            {
                states = new OneFrameState();
                oneFrameStates.States.Add(playerId, states);
            }
            states.States.AddRange(self.InputFrames);
            self.InputFrames.Clear();
        }
        
        // public static void SendFrameState<T>(this T self, StateFrame state) where T : LSEntity, IActionExecution
        // {
        //     state.ActorId = self.ActionAbilityAbility.Owner.Id;
        //     state.Frame = self.Frame();
        //     self.LSWorld().GetComponent<FrameStateComponent>().SendFrameStateInner(state);
        // }
        
        public static void SendFrameState<T, TSnapshot>(this T self, TSnapshot snapshot) 
            where T : LSEntity, IActionExecution where TSnapshot : ISnapshot
        {
            StateFrame state = new StateFrame();
            state.ActorId = self.ActionAbilityAbility.Owner.Id;
            state.Frame = self.Frame();
            snapshot.WriteTo(state);
            self.LSWorld().GetComponent<FrameStateComponent>().SendFrameStateInner(state);
        }
        private static void SendFrameStateInner(this FrameStateComponent self, StateFrame state)
        {
            state.Frame = self.LSWorld().Frame;
            self.InputFrames.Add(state);
        }
        
        private static void UpdateState(this FrameStateComponent self, OneFrameState states)
        {
            var actorComponent = self.Parent.GetComponent<ActorComponent>();
            foreach (var state in states.States)
            {
                var actor = actorComponent.GetChild<Actor>(state.ActorId);
                if (actor == null)
                {
                    Log.Error($"actor not found: {state.ActorId}");
                    continue;
                }

                if (!actor.OnFrameState(state))
                {
                    Log.Error($"actor OnFrameState fail {state}");
                }
              
            }
        }

        private static bool OnFrameState(this Actor self, StateFrame state)
        {
            switch (state.Type)
            {
                case  FrameStateType.StateMove:
                    self.GetComponent<MoveAbility>().OnFrameState(state.StateMove);
                    break;
                case  FrameStateType.StateCreateActor:
                    self.GetComponent<CreateActorAbility>().OnFrameState(state.StateCreateActor);
                    break;
                case  FrameStateType.StateCastSkill:
                    self.GetComponent<SpellActionAbility>().OnFrameState(state.StateCastSkill);
                    break;
                case  FrameStateType.StateCure:
                    self.GetComponent<CureActionAbility>().OnFrameState(state.StateCure);
                    break;
                case   FrameStateType.StateDamage:
                    self.GetComponent<DamageActionAbility>().OnFrameState(state.StateDamage);
                    break;
                case   FrameStateType.StateAddBuff:
                    self.GetComponent<AddBuffActionAbility>().OnFrameState(state.StateAddBuff);
                    break;
                default:
                    throw new System.Exception($"not support state: {state.Type}");
            }

            return true;
        }


        [EntitySystem]
        public static void Deserialize(this FrameStateComponent self)
        {
            self.InputFrames = new List<StateFrame>();
        }
        
        
        
    }
}