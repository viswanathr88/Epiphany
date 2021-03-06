﻿using Epiphany.ViewModel.Items;
using Epiphany.WP81;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Epiphany.View.Controls
{
    public sealed partial class BookStack : UserControl
    {
        public BookStack()
        {
            this.InitializeComponent();
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(BookStack), new PropertyMetadata(false));


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(BookStack), new PropertyMetadata(string.Empty));


        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(BookStack), new PropertyMetadata(null));

        private async void Book_Clicked(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                var bookItemVM = e.ClickedItem as IBookItemViewModel;
                await App.Navigate(typeof(BookPage), bookItemVM.Item);
            }
        }
    }
}
