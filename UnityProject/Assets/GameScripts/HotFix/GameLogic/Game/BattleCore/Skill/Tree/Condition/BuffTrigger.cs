using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    [AkiInfo("Conditional:是否触发buff，能的话执行一次触发")]
    [AkiLabel("Buff:触发buff")]
    [AkiGroup("Buff")]
    public class BuffTrigger : SkillConditional
    {
        public BuffExecution buffExecution;

        protected override void OnAwake()
        {
            base.OnAwake();
            buffExecution = skillTree.Execute as BuffExecution;
        }

        protected override Status OnUpdate()
        {
            var ret = base.OnUpdate();
            if (ret == Status.Success)
            {
                buffExecution.OnFinishTrigger();
            }

            return ret;
        }

        protected override bool IsUpdatable()
        {
            return buffExecution.IsCanTrigger();
        }
    }
}