using System.Collections.Generic;
using UnityEngine;
namespace Kurisu.AkiBT
{
    [AkiInfo("Composite: 加权随机，根据权重随机选择，等待节点完成运行后重新选择下一个节点")]
    public class WeightedRandom : Composite
    {
        private NodeBehavior runningNode;
        [SerializeField, Tooltip("节点权重列表，当列表长度大于子节点数量时，多余部分的权重将不会被包含在内")]
        private List<float> weights = new();

        protected override Status OnUpdate()
        {
            // 如果前一个状态是Running，则更新正在运行的节点。
            if (runningNode != null)
            {
                return HandleStatus(runningNode.Update(), runningNode);
            }

            var result = GetNext();
            var target = Children[result];
            return HandleStatus(target.Update(), target);
        }

        private Status HandleStatus(Status status, NodeBehavior updated)
        {
            runningNode = status == Status.Running ? updated : null;
            return status;
        }

        private int GetNext()
        {
            float total = 0;
            int count = Mathf.Min(weights.Count, Children.Count);

            for (int i = 0; i < count; i++)
            {
                total += weights[i];
            }

            float random = UnityEngine.Random.Range(0, total);

            for (int i = 0; i < count; i++)
            {
                if (random < weights[i])
                {
                    return i;
                }

                random -= weights[i];
            }

            return 0;
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