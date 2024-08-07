using ET;
using UnityEngine;

namespace GameLogic.Battle
{
    /// <summary>
    /// 所有的显示单位都要挂载的组件
    /// </summary>
    [ComponentOf]
    public class ViewComponent : Entity, IAwake<LSEntity, string>,IDestroy,IUpdate
    {
        public GameObject Go;
        public GameObject ModelGo;
        public string  ModePath = null;
        public EntityRef<LSEntity> owner;
        public LSEntity Owner => owner;
    }
}