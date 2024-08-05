using System.Collections.Generic;
using UnityEditor;
using Kurisu.AkiBT;
using Kurisu.AkiBT.Editor;
using GameLogic.Battle;
namespace Kurisu.AkiBT.Editor
{
    public class SkillTreeView : BehaviorTreeView
    {
        public SkillTreeView(IBehaviorTree bt, EditorWindow editor) : base(bt, editor)
        {
            //provider.SetShowNodeTypes(new List<string>(){"Skill","Math"});
        }
        public override string TreeEditorName=>"SkillST";
        protected override bool Validate()
        {
            var stack = new Stack<IBehaviorTreeNode>();
            bool findExit=false;
            stack.Push(root);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (!node.Validate(stack))
                {
                    return false;
                }
                if(node.GetBehavior().Equals(typeof(SkillExit))||node.GetBehavior().Equals(typeof(SkillComposite)))findExit=true;
            }
            if(!findExit)return false;
            return true;
        }
    }
}