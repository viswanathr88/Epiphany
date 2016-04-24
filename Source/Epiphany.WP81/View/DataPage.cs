﻿using Epiphany.Logging;
using Epiphany.ViewModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using Windows.Phone.UI.Input;

namespace Epiphany.View
{
    public class DataPage : Page
    {
        public DataPage()
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (DataContext == null)
            {
                Logger.LogError("DataPage cannot have a null DataContext");
                return;
            }

            IDataViewModel vm = DataContext as IDataViewModel;
            if (vm == null)
            {
                Logger.LogError("DataContext does not implement IDataViewModel");
                return;
            }

            await vm.LoadAsync(e.Parameter);
            Logger.LogDebug("LoadAsync returned");
        }

        protected void RegisterDoneEvent()
        {
            IDataViewModel vm = DataContext as IDataViewModel;
            if (vm == null)
            {
                Logger.LogError("DataContext does not implement IDataViewModel");
                return;
            }

            vm.Done += OnViewModelDone;
        }

        protected void RegisterPropertyChanged()
        {
            IDataViewModel vm = DataContext as IDataViewModel;
            if (vm == null)
            {
                Logger.LogError("DataContext does not implement IDataViewModel");
                return;
            }
            vm.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected virtual void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Nothing to do here
        }

        protected virtual void OnViewModelDone(object sender, EventArgs e)
        {
            // Nothing to do here
        }

        protected void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
    }
}
