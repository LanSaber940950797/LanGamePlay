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

    [DefaultAssetType(typeof(BattleAITreeSO))]
    public class BattleAIEditorWindow : GraphEditorWindow
    {
        protected override string TreeName => "BattleAI Tree";
        protected override string InfoText => "战斗AI树编辑器";
        [MenuItem("Tools/AkiBT/BattleAI Editor")]
        private static void ShowEditorWindow()
        {
            string path = EditorUtility.SaveFilePanel("Select ScriptableObject save path", Application.dataPath, "BattleAITreeSO", "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = path.Replace(Application.dataPath, string.Empty);
            var treeSO = CreateInstance<BattleAITreeSO>();
            AssetDatabase.CreateAsset(treeSO, $"Assets/{path}");
            AssetDatabase.SaveAssets();
            Show(treeSO);
        }
        
        public static void Show(IBehaviorTree bt)
        {
            var window = Create<BattleAIEditorWindow>(bt);
            window.Show();
            window.Focus();
        }
    }
}