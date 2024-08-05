using UnityEngine;
namespace Kurisu.AkiBT
{
    using UnityEngine;

    namespace Kurisu.AkiBT
    {
        [AkiInfo("Composite : Rotator, 按顺序更新子节点，每次Update只更新当前节点" +
            "节点完成后，下一个Update将继续更新下一个节点")]
        public class Rotator : Composite
        {
            [SerializeField]
            private bool resetOnAbort;

            private int targetIndex;

            private NodeBehavior runningNode;

            protected override Status OnUpdate()
            {
                // 如果上一个状态是Running，则更新正在运行的节点。
                if (runningNode != null)
                {
                    return HandleStatus(runningNode.Update(), runningNode);
                }
                return HandleStatus(Children[targetIndex].Update(), Children[targetIndex]);
            }

            private void SetNext()
            {
                targetIndex++;
                if (targetIndex >= Children.Count)
                {
                    targetIndex = 0;
                }
            }

            private Status HandleStatus(Status status, NodeBehavior updated)
            {
                if (status == Status.Running)
                {
                    runningNode = updated;
                }
                else
                {
                    runningNode = null;
                    SetNext();
                }
                return status;
            }

            public override void Abort()
            {
                if (runningNode != null)
                {
                    runningNode.Abort();
                    runningNode = null;
                }
                if (resetOnAbort)
                {
                    targetIndex = 0;
                }
            }
        }
    }
}