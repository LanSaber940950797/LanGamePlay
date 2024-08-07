using System;
using System.Collections.Generic;
using ET;
using TEngine;
using UnityEngine;

namespace GameLogic.Battle
{
    public enum InputType
    {
        None,
        //跳跃
        Jump,
        //攻击
        Attack,
        //技能
        Skill1,
        Skill2,
        Skill3,
    }


    public class InputControlComponent : Entity, IAwake,IUpdate
    {
      
        public Vector3 MoveDir = Vector3.zero;

        public bool IsJoystickTouch => MoveDir.sqrMagnitude > 0;
        //攻击方向
        public Vector3 AttackDir = Vector3.zero;
        public Vector3 AttackPos = Vector3.zero;
        
        
        //技能键位
        public long MyId;
        public EntityRef<ActorView> Actor;

        public Dictionary<InputType, KeyCode> InputDic;
        public InputType LastInputType;


    }
}