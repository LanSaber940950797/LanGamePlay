using System;

namespace ET
{
    public interface ILSLateUpdate
    {
    }
    
    public interface ILSLateUpdateSystem: ISystemType
    {
        void Run(LSEntity o);
    }

    [LSEntitySystem]
    public abstract class LSLateUpdateSystem<T>: SystemObject, ILSLateUpdateSystem where T: LSEntity, ILSLateUpdate
    {
        void ILSLateUpdateSystem.Run(LSEntity o)
        {
            this.LSLateUpdate((T)o);
        }

        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(ILSUpdateSystem);
        }

        int ISystemType.GetInstanceQueueIndex()
        {
            return LSQueneUpdateIndex.LSLateUpdate;
        }

        protected abstract void LSLateUpdate(T self);
    }
}