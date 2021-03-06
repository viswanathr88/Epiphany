﻿using Epiphany.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Epiphany.ViewModel
{
    

    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        private readonly string name;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase()
        {
            this.name = GetType().ToString();
        }
        public void SetValue(string propertyName, object value)
        {
            PropertyInfo info = GetProperty(propertyName);
            
            if (info == null)
                return;

            Type propertyType = info.PropertyType;
            object typedValue = Convert.ChangeType(value, propertyType, System.Globalization.CultureInfo.CurrentCulture);
            info.SetValue(this, typedValue, null);
        }

        public bool SetProperty<T>(ref T field, T value, [CallerMemberName] string member = null)
        {
            bool fResult = false;
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(member);
                fResult = true;
            }

            return fResult;
        }
        protected string GetName()
        {
            return this.name;
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> expr)
        {
            string propertyName = GetProperty(expr).Name;
            RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                Logger.LogError("Property Name is Empty");
                return;
            }

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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

        public virtual void Dispose()
        {
        }
    }
}
