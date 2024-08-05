using System;
using UnityEngine;
using Object = System.Object;

namespace Kurisu.AkiBT
{
    [Serializable]
    public class SharedSObject : SharedVariable<object>
    {
        [SerializeField]
        private string constraintTypeAQM;
        public string ConstraintTypeAQM { get => constraintTypeAQM; set => constraintTypeAQM = value; }
        public SharedSObject(object value)
        {
            this.value = value;
        }
        public SharedSObject()
        {

        }
        public override object Clone()
        {
            return new SharedSObject() { Value = value, Name = Name, IsShared = IsShared, ConstraintTypeAQM = ConstraintTypeAQM, IsGlobal = IsGlobal };
        }
    }
    [Serializable]
    public class SharedSTObject<TObject> : SharedVariable<TObject>, IBindableVariable<object> 
    {
        //Special case of binding SharedTObject<T> to SharedObject
        object IBindableVariable<object>.Value
        {
            get
            {
                return Value;
            }

            set
            {
                Value = (TObject)value;
            }
        }

        public SharedSTObject(TObject value)
        {
            this.value = value;
        }
        public SharedSTObject()
        {

        }
        public override object Clone()
        {
            return new SharedSTObject<TObject>() { Value = value, Name = Name, IsShared = IsShared, IsGlobal = IsGlobal };
        }
        public override void Bind(SharedVariable other)
        {
            //Special case of binding SharedObject to SharedTObject<T>
            if (other is IBindableVariable<object> sharedObject)
            {
                Bind(sharedObject);
            }
            else
            {
                base.Bind(other);
            }
        }
        public void Bind(IBindableVariable<object> other)
        {
            Getter = () =>
            {
                var evt = other.Value;
                if (evt == null)
                {
                    return default;
                }
                return (TObject)evt;
            };
            Setter = (evt) => other.Value = evt;
        }
    }
}