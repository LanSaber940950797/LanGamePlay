using ET;
using UnityEngine;

namespace GameLogic.Battle
{
    [ComponentOf]
    public class ActorView : Entity, IAwake<Actor>,IUpdate
    {
        public GameObject GameObject { get; set; }
        public Transform Transform { get; set; }
        private EntityRef<Actor> actor;
        public ThirdPersonSystem Controller;
        public StarterAssetsInputs Inputs;
        public bool IsInitPos = false;

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
        public Vector3 Position;
        public Quaternion Rotation;
        public float totalTime;
        public float t;
        
    }
}