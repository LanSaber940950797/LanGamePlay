using System;
using System.Reflection;
using ET;
using Sirenix.Serialization.Utilities;
using TEngine;
using UnityEngine;
using Log = TEngine.Log;

namespace GameLogic
{
    public class GameEntrySystem :BaseLogicSys<GameEntrySystem>
    {
        private EntityRef<Scene> root;

        public Scene Root
        {
            get
            {
                return this.root;
            }
            set
            {
                this.root = value;
            }
        }

       
        public  bool Start()
        {
            StartAsync().NoContext();
            return true;
        }
        
        private async ETTask StartAsync()
        {
            GameObject EntityRoot = new GameObject("EntityRoot");
            GameObject.DontDestroyOnLoad(EntityRoot);
            
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { ET.Log.Error(e.ExceptionObject.ToString()); };
            // 注册Entity序列化器
            EntitySerializeRegister.Init();
            World.Instance.AddSingleton<ET.Logger>().Log = new TELogger();
            ETTask.ExceptionHandler += ET.Log.Error;
            
            World.Instance.AddSingleton<TimeInfo>();
            World.Instance.AddSingleton<FiberManager>();

            await ETTask.CompletedTask;

            Log.Debug($"StartAsync");

            // GameNetty.Runtime (Assembly)
            Assembly runtime = typeof(Entry).Assembly;

            World.Instance.AddSingleton<ET.CodeTypes, Assembly[]>(new[]
            {
                // GameNetty.Runtime (Assembly)
                typeof(Entry).Assembly,
                // Assembly-CSharp (Assembly)
                //typeof(Init).Assembly,
                // GameProto (Assembly)
                typeof(ConfigSystem).Assembly,
                // GameLogic (Assembly)
                typeof(GameEntrySystem).Assembly
            });
           
            IStaticMethod start = new StaticMethod(runtime, "ET.Entry", "Start");
            start.Run();
        }
        
        public override void OnUpdate()
        {
            TimeInfo.Instance.Update();
            FiberManager.Instance.Update();
        }
        
        public override void OnLateUpdate()
        {
            FiberManager.Instance.LateUpdate();
        }
    }
    
    
}