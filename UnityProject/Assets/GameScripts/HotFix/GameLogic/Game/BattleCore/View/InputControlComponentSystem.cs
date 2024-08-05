using System;
using System.Collections.Generic;
using ET;
using UnityEngine;

namespace GameLogic.Battle
{
    [EntitySystemOf(typeof(InputControlComponent))]
    public static partial class InputControlComponentSystem
    {
        [EntitySystem]
        public  static void Awake(this InputControlComponent self)
        {
            self.MyId = self.Root().GetComponent<PlayerComponent>().PlayerId;
            self.InputDic = new Dictionary<InputType, KeyCode>
            {
                { InputType.Jump, KeyCode.Space },
                { InputType.Attack, KeyCode.Mouse0 },
                { InputType.Skill1, KeyCode.Q },
                { InputType.Skill2, KeyCode.E },
            };
        }
        
        [EntitySystem]
        public static void Update(this InputControlComponent self)
        {
          
            self.ReflshMoveDir();
            self.ReflshKeyCode();
            self.ReflshKeyCode();
            //RefreshAttackDir();
            //CheckCastSkill();
        }
        
       
        
        // private static void RefreshAttackDir(this InputControlCompoent self)
        // {
        //     self.attackPos = Input.mousePosition;
        //     self.attackPos.z = 10;
        //     self.attackPos = Camera.main.ScreenToWorldPoint(self.attackPos);
        //     //self.attackDir = self.attackPos - actor.transformComponet.GetCenterPosition();
        //     self.attackDir.z = 0;
        //     self.attackDir.Normalize();
        // }
        
        private static void ReflshMoveDir(this InputControlComponent self)
        {
          
            //获取输入
            var keyH = Input.GetAxis("Horizontal");
            var keyV = Input.GetAxis("Vertical");

            var keyControls = Math.Abs(keyH) > 0.01f || Math.Abs(keyV) > 0.01f;
            if (keyControls)
            {
                self.MoveDir = new Vector3(keyH, 0, keyV);
                //移动方向是相对相机的，获取实际方向
                self.MoveDir = Camera.main.transform.TransformDirection(self.MoveDir);
            }
            else
            {
                self.MoveDir = Vector3.zero;
            }
        }

      
        
        private static void ReflshKeyCode(this InputControlComponent self)
        {
            foreach (var it in self.InputDic)
            {
                if (Input.GetKey(it.Value))
                {
                    self.LastInputType = it.Key;
                    break;
                }
            }
        }


        


        public static void LSLateUpdate(this InputControlComponent self)
        {
            self.LastInputType = InputType.None;
        }
        
    }
}