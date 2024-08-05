using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using GameLogic.Battle;
namespace Kurisu.AkiBT.Editor
{
    [CustomEditor(typeof(SkillTreeViewComponent),true)]
    public class SkillTreeSOEditor : UnityEditor.Editor
    {
        private HashSet<ObserveProxyVariable> observeProxies;

        private IBehaviorTree Tree
        {
            get
            {
                var t = target as SkillTreeViewComponent;
                return t.skillTreeSO;
            }
        }
        private const string LabelText = "AkiBT BehaviorTreeSO <size=12>Version1.4.2</size>";
        private const string ButtonText = "Edit BehaviorTreeSO";
        private const string DebugText = "Debug BehaviorTreeSO";
        public override VisualElement CreateInspectorGUI()
        {
           
            var myInspector = new VisualElement();
            var label = new Label(LabelText);
            label.style.fontSize = 20;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            myInspector.Add(label);
            myInspector.styleSheets.Add(BehaviorTreeSetting.GetInspectorStyle("SkillST"));
            myInspector.Add(new Label("BehaviorTree Description"));
            var description = new PropertyField(serializedObject.FindProperty("Description"), string.Empty);
            myInspector.Add(description);
            observeProxies = BehaviorTreeEditorUtility.DrawSharedVariables(myInspector, Tree, target, this);
            var button = BehaviorTreeEditorUtility.GetButton(() =>
            {
                SkillEditorWindow.Show(Tree);
            });
            if (!Application.isPlaying)
            {
                button.style.backgroundColor = new StyleColor(new Color(140 / 255f, 160 / 255f, 250 / 255f));
                button.text = ButtonText;
            }
            else
            {
                button.text = DebugText;
                button.style.backgroundColor = new StyleColor(new Color(253 / 255f, 163 / 255f, 255 / 255f));
            }
            myInspector.Add(button);
            return myInspector;
        }
        private void OnDisable()
        {
            if (observeProxies == null) return;
            foreach (var proxy in observeProxies)
            {
                proxy.Dispose();
            }
        }
    }
}