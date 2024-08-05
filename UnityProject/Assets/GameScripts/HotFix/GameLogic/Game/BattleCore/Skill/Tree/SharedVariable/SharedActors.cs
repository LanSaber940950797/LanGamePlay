using System;
using System.Collections.Generic;
using ET;
using UnityEngine;

namespace GameLogic.Battle
{
    [Serializable]
    public class SharedActors : LSEntity
    {
        public List<EntityRef<Actor>> Value = new List<EntityRef<Actor>>();
    }
}