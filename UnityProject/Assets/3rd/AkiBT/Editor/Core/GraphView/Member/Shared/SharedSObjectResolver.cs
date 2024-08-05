using System.Reflection;
using UnityEditor.UIElements;
using System;
using UnityEngine.UIElements;
namespace Kurisu.AkiBT.Editor
{
    public class SharedSObjectResolver : FieldResolver<SharedSObjectField, SharedSObject>
    {
        public SharedSObjectResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
        protected override void SetTree(ITreeView ownerTreeView)
        {
            editorField.InitField(ownerTreeView);
        }
        private SharedSObjectField editorField;
        protected override SharedSObjectField CreateEditorField(FieldInfo fieldInfo)
        {
            editorField = new SharedSObjectField(fieldInfo.Name, null, fieldInfo.FieldType, fieldInfo);
            return editorField;
        }
        public static bool IsAcceptable(Type infoType, FieldInfo _) => infoType == typeof(SharedSObject);

    }
    public class SharedSObjectField : SharedVariableField<SharedSObject, object>
    {
        public SharedSObjectField(string label, VisualElement visualInput, Type objectType, FieldInfo fieldInfo) : base(label, visualInput, objectType, fieldInfo)
        {
        }
        protected override BaseField<object> CreateValueField()
        {
            Type objectType;
            try
            {
                objectType = Type.GetType(value.ConstraintTypeAQM, true);
            }
            catch
            {
                objectType = typeof(UnityEngine.Object);
            }
            return new SObjectField()
            {
                objectType = objectType
            };
        }

        protected sealed override void OnValueUpdate()
        {
            Type objectType;
            try
            {
                objectType = Type.GetType(value.ConstraintTypeAQM, true);
            }
            catch
            {
                objectType = typeof(object);
            }
            (ValueField as SObjectField).objectType = objectType;
        }
        
        
    }
    
    public class SObjectField : BaseField<object>
    {
        public Type objectType;

        public SObjectField() : base("value", null)
        {
            
        }
        public SObjectField(string label) : base(label, null)
        {
            
        }
  
    }
}