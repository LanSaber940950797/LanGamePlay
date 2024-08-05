using UnityEngine;
namespace Kurisu.AkiBT
{
    [AkiInfo("Composite:选择器，依次遍历子节点，如果返回失败，则继续更新下一个节点，否则返回成功")]
    public class Selector : Composite
    {
        [SerializeField, Tooltip("当当前运行节点之前的子节点的判断发生变化时，中断当前运行节点，中断将影响其分支下的所有节点；" +
        "注意：在AkiBT中，动作节点的判断始终为true，只有条件节点会改变判断（CanUpdate）")]
        private bool abortOnConditionChanged = true;

        private NodeBehavior runningNode;

        public override bool CanUpdate()
        {
            // 当任何子节点可以更新时，此节点可以更新
            foreach (var child in Children)
            {
                if (child.CanUpdate())
                {
                    return true;
                }
            }
            return false;
        }

        protected override Status OnUpdate()
        {
            // 如果前一个状态为Running，则更新当前运行节点
            if (runningNode != null)
            {
                if (abortOnConditionChanged && IsConditionChanged(runningNode))
                {
                    runningNode.Abort();
                    return UpdateWhileFailure(0);
                }
                var currentOrder = Children.IndexOf(runningNode);
                var status = runningNode.Update();
                if (status == Status.Failure)
                {
                    // 更新下一个节点
                    return UpdateWhileFailure(currentOrder + 1);
                }

                return HandleStatus(status, runningNode);
            }

            return UpdateWhileFailure(0);
        }

        private bool IsConditionChanged(NodeBehavior runningChild)
        {
            // 当具有比自身优先级更高的节点的条件可以更新时
            var priority = Children.IndexOf(runningChild);
            for (var i = 0; i < priority; i++)
            {
                var candidate = Children[i];
                if (candidate.CanUpdate())
                {
                    return true;
                }
            }

            return false;
        }

        private Status UpdateWhileFailure(int start)
        {
            for (var i = start; i < Children.Count; i++)
            {
                var target = Children[i];
                var childStatus = target.Update();
                if (childStatus == Status.Failure)
                {
                    continue;
                }
                return HandleStatus(childStatus, target);
            }

            return HandleStatus(Status.Failure, null);
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