using ET;
using UnityEngine;

namespace GameLogic.Battle
{
    [ComponentOf]
    public class ActorView : Entity, IAwake<Actor>
    {
        private EntityRef<Actor> actor;
      
        public Actor Actor
        {
            get
            {
                return actor;
            }
            set
            {
                actor = value;
            }
        }
        public float totalTime;
        public float t;
        
    }
}