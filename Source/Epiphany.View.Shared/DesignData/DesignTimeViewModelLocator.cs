﻿using Epiphany.View.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epiphany.ViewModel;
using Epiphany.ViewModel.Services;
using Epiphany.ViewModel.Items;
using Epiphany.Model;

namespace Epiphany.View.DesignData
{
    public sealed class DesignTimeViewModelLocator : IViewModelLocator
    {
        public AboutViewModel About
        {
            get
            {
                return new AboutViewModel();
            }
        }

        public AddBookViewModel AddBook
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public AuthorViewModel Author
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BookViewModel Book
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BooksViewModel Books
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public EventsViewModel Events
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public FriendsViewModel Friends
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public HomeViewModel Home
        {
            get
            {
                var vm = new HomeViewModel();
                vm.SetValue(nameof(HomeViewModel.IsLoading), true);
                vm.SetValue(nameof(HomeViewModel.IsLoggedIn), false);
                vm.SetValue(nameof(HomeViewModel.Opacity), 0.15);
                return vm;
            }
        }

        public LogonViewModel Logon
        {
            get
            {
                var vm = new LogonViewModel();
                vm.SetValue(nameof(LogonViewModel.IsWaitingForUserInteraction), false);
                vm.SetValue(nameof(LogonViewModel.IsSignInTakingLonger), true);
                vm.SetValue(nameof(LogonViewModel.Error), new object());
                return vm;
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return null;
            }
        }

        public ProfileViewModel Profile
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public SearchViewModel Search
        {
            get
            {
                var vm = new SearchViewModel();
                vm.SetValue(nameof(SearchViewModel.HasResults), true);

                return vm;
            }
        }

        public SettingsViewModel Settings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
