using ET;
using TEngine;
namespace GameLogic.Battle
{
    public class EventComponent : Entity, IAwake,IDestroy
    {
        public ActorEventDispatcher EventDispatcher { get;  set; }
        
      
    }
}