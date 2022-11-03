using System;
using System.Collections.Generic;

namespace FUI
{
    public interface IBindableProperty
    {
        object Value { get; set; }
        void SetValueWithoutNotify(object value);
        void AddAdpator(IAdpator adpater);
        void RemoveAdpator(IAdpator adpater);
    }

    public interface IBindableProperty<T>
    {
        T Value { get; set; }
        void SetValueWithoutNotify(T value);
        void AddAdpator(IAdpator<T> adpator);
        void RemoveAdpator(IAdpator<T> adpator);
    }

    public class BP<T> : IBindableProperty, IBindableProperty<T>
    {
        public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        T IBindableProperty<T>.Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddAdpator(IAdpator adpater)
        {
            throw new NotImplementedException();
        }

        public void AddAdpator(IAdpator<T> adpator)
        {
            throw new NotImplementedException();
        }

        public void RemoveAdpator(IAdpator adpater)
        {
            throw new NotImplementedException();
        }

        public void RemoveAdpator(IAdpator<T> adpator)
        {
            throw new NotImplementedException();
        }

        public void SetValueWithoutNotify(object value)
        {
            throw new NotImplementedException();
        }

        public void SetValueWithoutNotify(T value)
        {
            throw new NotImplementedException();
        }
    }
    public struct BindableProperty<T>
    {
        T oldValue;
        T newValue;
        List<Action<T, T>> listeners;
        List<IAdpator<T>> adpaters;

        public T Value
        {
            get { return newValue; }
            set
            {
                if (oldValue.Equals(newValue))
                {
                    return;
                }
                SetValueWithoutNotify(value);
                Notify();
            }
        }

        public BindableProperty(T value)
        {
            this.oldValue = default;
            this.newValue = value;
            listeners = new List<Action<T, T>>();
            adpaters = new List<IAdpator<T>>();
        }

        public void SetValueWithoutNotify(T value)
        {
            oldValue = newValue;
            newValue = value;
        }

        void Notify()
        {
            foreach(var listener in listeners)
            {
                listener?.Invoke(oldValue, newValue);
            }
        }

        public void AddListener(IAdpator<T> adpater)
        {
            adpaters.Add(adpater);
        }

        public void RemoveAdpator(IAdpator<T> adpater)
        {
            adpaters.Remove(adpater);
        }

        public void RemoveAllAdpator()
        {
            adpaters.Clear();
        }

        public void AddListener(Action<T, T> listener)
        {
            if (listeners.Contains(listener))
            {
                return;
            }
            listeners.Add(listener);
        }

        public void RemoveListener(Action<T, T> listener)
        {
            listeners.Remove(listener);
        }

        public void RemoveAllListener()
        {
            listeners.Clear();
        }
    }
}
