using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.Callbacks;
using System.Reflection;
using GameLogic.Battle;

namespace Kurisu.AkiBT.Editor
{

    [DefaultAssetType(typeof(SkillTreeSO))]
    public class SkillEditorWindow : GraphEditorWindow
    {
        protected override string TreeName => "Skill Tree";
        protected override string InfoText => "技能树编辑器";
        [MenuItem("Tools/AkiBT/Skill Editor")]
        private static void ShowEditorWindow()
        {
            string path = EditorUtility.SaveFilePanel("Select ScriptableObject save path", Application.dataPath, "SkillTreeSO", "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = path.Replace(Application.dataPath, string.Empty);
            var treeSO = CreateInstance<SkillTreeSO>();
            AssetDatabase.CreateAsset(treeSO, $"Assets/{path}");
            AssetDatabase.SaveAssets();
            Show(treeSO);
        }
        
        public static void Show(IBehaviorTree bt)
        {
            var window = Create<SkillEditorWindow>(bt);
            window.Show();
            window.Focus();
        }
    }
}