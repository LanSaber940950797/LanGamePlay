using Kurisu.AkiBT;

namespace GameLogic.Battle
{
    [AkiInfo("Conditional:是否刷新")]
    [AkiLabel("Buff:刷新buff")]
    [AkiGroup("Buff")]
    public class BuffRefresh : SkillConditional
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
                buffExecution.IsNeedRefresh = false;
            }

            return ret;
        }

        protected override bool IsUpdatable()
        {
            return buffExecution.IsNeedRefresh;
        }
    }
}