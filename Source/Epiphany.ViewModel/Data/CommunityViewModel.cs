﻿using Epiphany.Model;
using Epiphany.Model.Services;
using Epiphany.ViewModel.Items;
using Epiphany.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Epiphany.ViewModel
{
    public sealed class CommunityViewModel : DataViewModel<VoidType>, ICommunityViewModel
    {
        private bool isEmpty;
        private IList<IFeedItemViewModel> items;

        private readonly IUserService userService;
        private readonly IReviewService reviewService;
        private readonly IResourceLoader resourceLoader;

        public CommunityViewModel(IUserService userService, IReviewService reviewService, IResourceLoader resourceLoader)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            if (reviewService == null)
            {
                throw new ArgumentNullException(nameof(reviewService));
            }

            if (resourceLoader == null)
            {
                throw new ArgumentNullException(nameof(resourceLoader));
            }

            this.userService = userService;
            this.reviewService = reviewService;
            this.resourceLoader = resourceLoader;
        }

        public bool IsEmpty
        {
            get
            {
                return this.isEmpty;
            }
            private set
            {
                SetProperty(ref this.isEmpty, value);
            }
        }

        public IList<IFeedItemViewModel> Items
        {
            get
            {
                return this.items;
            }
            private set
            {
                SetProperty(ref this.items, value);
            }
        }

        public async override Task LoadAsync(VoidType parameter)
        {
            if (!IsLoaded)
            {
                IsLoading = true;

                IList<FeedItemModel> items = await Task.Run(() => this.userService.GetRecentUserStatusesAsync());

                Items = new ObservableCollection<IFeedItemViewModel>();
                foreach (var item in items)
                {
                    Items.Add(new FeedItemViewModel(item, this.resourceLoader));
                }

                IsEmpty = (Items.Count == 0);
                IsLoading = false;
                IsLoaded = true;
            }
        }
    }
}