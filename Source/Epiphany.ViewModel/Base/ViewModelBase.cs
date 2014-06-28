﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Epiphany.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public void SetValue(string propertyName, string value)
        {
            PropertyInfo info = GetProperty(propertyName);
            
            if (info == null)
                return;

            Type propertyType = info.PropertyType;
            object typedValue = Convert.ChangeType(value, propertyType, System.Globalization.CultureInfo.CurrentCulture);
            info.SetValue(this, typedValue, null);
        }

        private PropertyInfo GetProperty<T>(Expression<Func<T>> expr)
        {
            var member = expr.Body as MemberExpression;
            if (member == null)
                throw new InvalidOperationException("Expression is not a member access expression.");
            var property = member.Member as PropertyInfo;
            if (property == null)
                throw new InvalidOperationException("Member in expression is not a property.");
            return property;
        }

        private PropertyInfo GetProperty(string propertyName)
        {
            PropertyInfo pInfo = this.GetType().GetRuntimeProperty(propertyName);
            return pInfo;
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> expr)
        {
            string propertyName = GetProperty(expr).Name;
            RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Dispose()
        {
        }
    }
}