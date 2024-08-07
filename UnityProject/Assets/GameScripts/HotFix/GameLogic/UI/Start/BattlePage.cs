using Cinemachine;
using ET;
using GameLogic.Battle;
using TEngine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameLogic
{
    [Window(UILayer.UI, "BattlePage")]
    public partial class BattlePage : UIWindow
    {
        protected override void OnRefresh()
        {
            UpdateData();
        }

        protected override void RegisterEvent()
        {
          
        }
        
        protected EntityRef<ActorView> MyActor;
        protected Vector2 lookDir = Vector2.zero;
        protected Vector3 mousePosition = Vector3.zero;
        protected bool isInitCamera;
        protected bool IsInitInput;
        protected void UpdateData()
        {
            var root = GameHeler.Root();
            var room = root.GetComponent<Room>();
            m_textRoomId.text = $"房间：{room.Id}";
        }

        protected override void OnUpdate()
        {

            if (!isInitCamera)
            {
                InitCamera();
            }

            if (!IsInitInput)
            {
                InitInput();
            }

        }

        private void InitCamera()
        {
            var root = GameHeler.Root();
            var room = GameHeler.Room();
            ActorView myActor = MyActor;
            if (myActor == null)
            {
                var myId = root.GetComponent<PlayerComponent>().PlayerId;
                Actor actor = room.LSWorld.GetComponent<ActorComponent>().GetPlayerActor(myId);
                if (actor != null)
                {
                    MyActor = room.GetComponent<ActorViewComponent>().GetChild<ActorView>(actor.Id);
                    myActor = MyActor;
                }
              
            }

            if (myActor == null)
            {
                return;
            }

            var view = myActor.GetComponent<ViewComponent>();
            if (view.Go != null)
            {
                //PlayerFollowCamera
                //当前场景找到PlayerFollowCamera节点
                var obj = GameObject.Find("PlayerFollowCamera");
                var playerFollowCamera = view.Go.transform.Find("PlayerCameraRoot");
                var camera = obj.GetComponent<CinemachineVirtualCamera>();
                camera.Follow = playerFollowCamera.transform;
                isInitCamera = true;
            }
        }

        public void InitInput()
        {
            ActorView actorView = MyActor;
            if (actorView == null)
            {
                return;
            }

            // if (actorView.Inputs == null)
            // {
            //     return;
            // }
            //
            // var playerInput = actorView.Inputs.GetComponent<PlayerInput>();
            // playerInput.enabled = true;
            IsInitInput = true;
        }
    }
}