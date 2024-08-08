
using ET;
using TEngine;
using UnityEngine;


namespace GameLogic.Battle
{
    [ComponentOf(typeof(ActorView))]
    public class ActorAnimationComponent : Entity,IAwake, IDestroy
    {
       
       
        public IActorAnimation _actorAnimation;

        public IActorAnimation ActorAnimation;
       
    }


}