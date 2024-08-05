using ET;

namespace GameLogic
{
    public static class SSUpdaterHelper
    {
        // 回滚
        public static void Rollback(Room room, int frame)
        {
            var frameBuffer = room.StateFrameBuffer;
            // 回滚
            var authorityFrameStates = frameBuffer.FrameStates(frame);
            // 执行AuthorityFrame
            room.Update(authorityFrameStates, frame);
            
            RunLSRollbackSystem(room);
        }
        
        public static void RunLSRollbackSystem(Entity entity)
        {
            if (entity is LSEntity)
            {
                return;
            }
            
            LSEntitySystemSingleton.Instance.LSRollback(entity);
            
            if (entity.ComponentsCount() > 0)
            {
                foreach (var kv in entity.Components)
                {
                    RunLSRollbackSystem(kv.Value);
                }
            }

            if (entity.ChildrenCount() > 0)
            {
                foreach (var kv in entity.Children)
                {
                    RunLSRollbackSystem(kv.Value);
                }
            }
        }

    }
}