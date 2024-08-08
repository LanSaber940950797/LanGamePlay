using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
namespace Kurisu.AkiBT.Editor
{
    public class SharedSTObjectResolver<T> : FieldResolver<SharedSTObjectField<T>, SharedSTObject<T>>
    {
        public SharedSTObjectResolver(FieldInfo fieldInfo) : base(fieldInfo)
        {

        }
        protected override void SetTree(ITreeView ownerTreeView)
        {
            editorField.InitField(ownerTreeView);
        }
        private SharedSTObjectField<T> editorField;
        protected override SharedSTObjectField<T> CreateEditorField(FieldInfo fieldInfo)
        {
            editorField = new SharedSTObjectField<T>(fieldInfo.Name, null, fieldInfo);
            return editorField;
        }
        public static bool IsAcceptable(Type infoType, FieldInfo _)
        {
            return FieldResolverFactory.IsSharedSTObject(infoType) && infoType.GenericTypeArguments.Length == 1 && infoType.GenericTypeArguments[0] == typeof(T);
        }
    }
    public class SharedSTObjectField<T> : BaseField<SharedSTObject<T>>, IInitField 
    {
        private readonly bool forceShared;
        private readonly VisualElement foldout;
        private readonly Toggle toggle;
        private ITreeView treeView;
        private DropdownField nameDropdown;
        private SharedVariable bindExposedProperty;
        public SharedSTObjectField(string label, VisualElement visualInput, FieldInfo fieldInfo) : base(label, visualInput)
        {
            forceShared = fieldInfo.GetCustomAttribute<ForceSharedAttribute>() != null;
            AddToClassList("SharedVariableField");
            foldout = new VisualElement();
            foldout.style.flexDirection = FlexDirection.Row;
            contentContainer.Add(foldout);
            toggle = new Toggle("Is Shared");
            toggle.RegisterValueChangedCallback(evt => { value.IsShared = evt.newValue; OnToggle(evt.newValue); NotifyValueChange(); });
            if (forceShared)
            {
                toggle.value = true;
                return;
            }
            foldout.Add(toggle);
        }
        public void InitField(ITreeView treeView)
        {
            this.treeView = treeView;
            treeView.BlackBoard.View.RegisterCallback<VariableChangeEvent>(evt =>
            {
                if (evt.ChangeType != VariableChangeType.NameChange) return;
                if (evt.Variable != bindExposedProperty) return;
                nameDropdown.value = value.Name = evt.Variable.Name;
            });
            OnToggle(toggle.value);
        }
        private static List<string> GetList(ITreeView treeView)
        {
            var list = treeView.SharedVariables
            .Where(x => x is SharedSObject sharedObject && sharedObject.ConstraintTypeAQM == typeof(T).AssemblyQualifiedName)
            .Select(v => v.Name)
            .ToList();
            list.Add(string.Empty);
            return  list;
        }
        private void BindProperty()
        {
            if (treeView == null) return;
            bindExposedProperty = treeView.SharedVariables
            .Where(x => x is SharedSObject sharedObject && sharedObject.ConstraintTypeAQM == typeof(T).AssemblyQualifiedName && x.Name.Equals(value.Name))
            .FirstOrDefault();
        }
        private void OnToggle(bool IsShared)
        {
            if (IsShared)
            {
                RemoveNameDropDown();
                if (value != null && treeView != null) AddNameDropDown();
                RemoveValueField();
            }
            else
            {
                RemoveNameDropDown();
                RemoveValueField();
                AddValueField();
            }
        }
        private void AddNameDropDown()
        {
            nameDropdown = new DropdownField($"Shared {typeof(T).Name}", GetList(treeView), value.Name ?? string.Empty);
            nameDropdown.RegisterCallback<MouseEnterEvent>((evt) => { nameDropdown.choices = GetList(treeView); });
            nameDropdown.RegisterValueChangedCallback(evt => { value.Name = evt.newValue; BindProperty(); NotifyValueChange(); });
            foldout.Insert(0, nameDropdown);
        }
        private void RemoveNameDropDown()
        {
            if (nameDropdown != null) foldout.Remove(nameDropdown);
            nameDropdown = null;
        }
        private void RemoveValueField()
        {
            if (ValueField != null) foldout.Remove(ValueField);
            ValueField = null;
        }
        private void AddValueField()
        {
            ValueField = new SObjectField()
            {
                objectType = typeof(T)
            };
            ValueField.RegisterValueChangedCallback(evt => { value.Value = (T)evt.newValue; NotifyValueChange(); });
            if (value != null) ValueField.value = value.Value;
            foldout.Insert(0, ValueField);
        }
        public sealed override SharedSTObject<T> value
        {
            get => base.value; set
            {
                if (value != null) base.value = value.Clone() as SharedSTObject<T>;
                else base.value = new SharedSTObject<T>();
                if (forceShared) base.value.IsShared = true;
                Repaint();
            }
        }
        private SObjectField ValueField { get; set; }
        public void Repaint()
        {
            toggle.value = value.IsShared;
            if (ValueField != null) ValueField.value = value.Value;
            BindProperty();
            OnToggle(value.IsShared);
            NotifyValueChange();
        }
        protected void NotifyValueChange()
        {
            using ChangeEvent<SharedSTObject<T>> changeEvent = ChangeEvent<SharedSTObject<T>>.GetPooled(value, value);
            changeEvent.target = this;
            SendEvent(changeEvent);
        }
    }
}
