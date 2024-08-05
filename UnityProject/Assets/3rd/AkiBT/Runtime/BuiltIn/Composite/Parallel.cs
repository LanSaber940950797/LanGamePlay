using System.Collections.Generic;
namespace Kurisu.AkiBT
{
    [AkiInfo("复合节点：并行执行，运行所有子节点，如果返回错误则退出并返回失败，否则等待所有子节点同时返回正确；" +
     "注意：Parallel和Sequence都会按照子节点的顺序遍历，但是Sequence会在子节点出现Running状态时等待，后续节点将不会被更新，" +
     "而Parallel则始终运行所有节点")]
    public class Parallel : Composite
    {

        private List<NodeBehavior> runningNodes;

        protected override void OnAwake()
        {
            runningNodes = new List<NodeBehavior>();
        }

        /// <summary>
        /// 更新所有节点。
        /// - 任何一个节点处于Running状态 -> Running
        /// - 任何一个节点返回失败 -> Failure
        /// - 否则 -> Success
        /// </summary>
        protected override Status OnUpdate()
        {
            runningNodes.Clear();
            var anyFailed = false;
            foreach (var c in Children)
            {
                var result = c.Update();
                if (result == Status.Running)
                {
                    runningNodes.Add(c);
                }
                else if (result == Status.Failure)
                {
                    anyFailed = true;
                }
            }
            if (runningNodes.Count > 0)
            {
                return Status.Running;
            }

            if (anyFailed)
            {
                runningNodes.ForEach(e => e.Abort());
                return Status.Failure;
            }

            return Status.Success;
        }

        public override void Abort()
        {
            runningNodes.ForEach(e => e.Abort());
            runningNodes.Clear();
        }

    }
}