namespace Kurisu.AkiBT
{
    [AkiInfo("组合节点：随机，随机更新一个子节点并重新选择下一个节点")]
    public class Random : Composite
    {
        private NodeBehavior runningNode;

        protected override Status OnUpdate()
        {
            // 如果前一个状态是Running，则更新正在运行的节点。
            if (runningNode != null)
            {
                return HandleStatus(runningNode.Update(), runningNode);
            }

            var result = UnityEngine.Random.Range(0, Children.Count);
            var target = Children[result];
            return HandleStatus(target.Update(), target);
        }

        private Status HandleStatus(Status status, NodeBehavior updated)
        {
            runningNode = status == Status.Running ? updated : null;
            return status;
        }

        public override void Abort()
        {
            if (runningNode != null)
            {
                runningNode.Abort();
                runningNode = null;
            }
        }
    }
}