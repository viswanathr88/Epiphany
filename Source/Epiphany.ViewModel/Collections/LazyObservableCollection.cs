﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Epiphany.ViewModel.Collections
{
    sealed class LazyObservableCollection<TViewModel, TModel> : ObservableCollection<TViewModel>, ILazyObservableCollection<TViewModel>, ISupportIncrementalLoading
    {
        private readonly Func<IEnumerable<TModel>> collectionSourceFn;
        private readonly Func<TModel, TViewModel> adapterFn;
        private bool isLoading = false;
        private bool hasMoreItems = true;

        public LazyObservableCollection(Func<IEnumerable<TModel>> sourceFn, Func<TModel, TViewModel> adapterFn)
        {
            if (sourceFn == null)
            {
                throw new ArgumentNullException(nameof(collectionSourceFn));
            }

            this.collectionSourceFn = sourceFn;
            this.adapterFn = adapterFn;
        }


        public bool HasMoreItems
        {
            get
            {
                return this.hasMoreItems;
            }
            private set
            {
                this.hasMoreItems = value;
            }
        }

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            private set
            {
                if (this.isLoading != value)
                {
                    this.isLoading = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLoading)));
                }
            }
        }

        public event EventHandler<LoadedEventArgs> Loaded;
        private void RaiseLoaded(Exception error) => Loaded?.Invoke(this, new LoadedEventArgs(error));

        public event EventHandler<EventArgs> Loading;
        private void RaiseLoading() => Loading?.Invoke(this, EventArgs.Empty);

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            IsLoading = true;
            RaiseLoading();

            IEnumerable<TModel> collectionSource = collectionSourceFn.Invoke();
            if (Count <= 0 && collectionSource != null)
            {
                foreach (TModel model in collectionSource)
                {
                    var item = this.adapterFn.Invoke(model);
                    if (item != null)
                    {
                        Add(item);
                    }
                }
            }

            HasMoreItems = false;

            RaiseLoaded(null);
            IsLoading = false;

            return Task.FromResult<LoadMoreItemsResult>(new LoadMoreItemsResult() { Count = Convert.ToUInt32(Count) }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
