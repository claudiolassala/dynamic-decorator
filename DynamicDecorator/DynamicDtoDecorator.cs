using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace DynamicDecorator
{
    public class DynamicDtoDecorator : DynamicObject, INotifyPropertyChanged
    {
        readonly object _dto;
        readonly Dictionary<string, Action> _actions = new Dictionary<string, Action>();
        readonly Dictionary<string, PropertyAccessor> _members = new Dictionary<string, PropertyAccessor>();
        readonly Dictionary<string, IEnumerable<string>> _propertyDependencies = new Dictionary<string, IEnumerable<string>>();

        public DynamicDtoDecorator(object dto)
        {
            _dto = dto;
            RegisterProperties(dto);
            RegisterActions();
        }

        public dynamic this[string propertyName] => GetProperty(propertyName);

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        public override string ToString()
        {
            return _dto.ToString();
        }

        public T GetDto<T>() where T : class
        {
            return _dto as T;
        }

        void RegisterActions()
        {
            _dto.GetType()
                .GetMethods()
                .ForEach(methodInfo =>
                {
                    methodInfo
                        .GetCustomAttributes(true)
                        .OfType<AvailableToProxyAttribute>()
                        .ForEach(attribute => { SetAction(methodInfo.Name, () => methodInfo.Invoke(_dto, null)); });
                });
        }

        protected virtual void RegisterProperties(object dto)
        {
            dto.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(prop =>
                {
                    object[] notIndexedProperty = null;
                    Func<object> valueGetter = () => prop.GetValue(dto, notIndexedProperty);
                    Action<object> valueSetter = newValue => prop.SetValue(dto, newValue, notIndexedProperty);
                    Register(prop.Name, valueGetter, valueSetter);

                    var dependsOn = prop.GetCustomAttributes(true).OfType<DependsOnAttribute>().SingleOrDefault();
                    if (dependsOn != null)
                    {
                        RegisterPropertyDependencies(prop.Name, Array.ConvertAll(dependsOn.PropertyNames.Split(','), d => d.Trim()));
                    }
                });
        }

        public void Register(string propertyName, Func<object> valueGetter, Action<object> valueSetter)
        {
            var fullPropertyName = propertyName;

            if (_members.ContainsKey(fullPropertyName))
                return;

            dynamic initialValue = valueGetter.Invoke();
            var propertyValue = new PropertyAccessor(propertyName, valueGetter, valueSetter, initialValue);
            _members.Add(fullPropertyName, propertyValue);
        }

        public void RegisterPropertyDependencies(string property, string[] dependencies)
        {
            _propertyDependencies.Add(property, dependencies);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetProperty(binder.Name, value);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_members.ContainsKey(binder.Name))
            {
                var propertyValue = _members[binder.Name];
                result = propertyValue.Get();
                return true;
            }
            result = null;
            return false;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _members.Keys;
        }

        public dynamic GetProperty(string propertyName)
        {
            if (!_members.ContainsKey(propertyName))
                throw new InvalidOperationException($"Unregistered member: {propertyName}");

            return _members[propertyName].Get();
        }

        public virtual void SetProperty(string propertyName, dynamic value)
        {
            if (!_members.ContainsKey(propertyName))
                throw new InvalidOperationException($"Unregistered member: {propertyName}");

            if (_members[propertyName].Get() == value) return;

            _members[propertyName].Set(value);
            RaisePropertyChanged(propertyName);
            RaisePropertyChangedForDependentsOn(propertyName);
        }

        void RaisePropertyChangedForDependentsOn(string propertyThatWasChanged)
        {
            _propertyDependencies.ForEach(dependentProperty =>
            {
                var dependableProperties = dependentProperty.Value;
                if (!dependableProperties.Contains(propertyThatWasChanged)) return;

                var dependentPropertyName = dependentProperty.Key;
                RaisePropertyChanged(dependentPropertyName);
            });
        }

        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetAction(string actionName, Action action)
        {
            _actions.Add(actionName, action);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;
            if (!_actions.ContainsKey(binder.Name))
                return false;

            _actions[binder.Name].Invoke();
            return true;
        }

        public IEnumerable<ChangedProperty> GetChanges()
        {
            return _members.Values
                .Where(propertyValue => propertyValue.Changed)
                .Select(property => new ChangedProperty(property.PropertyName, property.InitialValue, property.CurrentValue));
        }
    }
}
