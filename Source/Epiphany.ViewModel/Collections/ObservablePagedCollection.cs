﻿using Epiphany.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Epiphany.ViewModel.Collections
{
    public sealed class ObservablePagedCollection<TViewModel, TModel> : ObservableCollection<TViewModel>, IObservablePagedCollection<TViewModel>
    {
        private readonly IPagedCollection<TModel> pagedCollection;
        private readonly IAsyncEnumerator<TModel> enumerator;
        private readonly Func<TModel, TViewModel> adapterMethod;
        private bool hasMoreItems = true;
        private int pageSize = 15;

        public event EventHandler<EventArgs> Loading;
        private void RaiseLoading() => Loading?.Invoke(this, EventArgs.Empty);

        public event EventHandler<EventArgs> Loaded;
        private void RaiseLoaded() => Loaded?.Invoke(this, EventArgs.Empty);

        public ObservablePagedCollection(IPagedCollection<TModel> pagedCollection,
            Func<TModel, TViewModel> adapterMethod)
        {
            if (pagedCollection == null)
            {
                throw new ArgumentNullException(nameof(pagedCollection));
            }

            this.pagedCollection = pagedCollection;
            this.enumerator = this.pagedCollection.GetEnumerator();
            this.adapterMethod = adapterMethod;
        }

        public bool HasMoreItems
        {
            get
            {
                return this.hasMoreItems;
            }
            private set
            {
                if (this.hasMoreItems == value) return;
                this.hasMoreItems = value;
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            CoreDispatcher dispatcher = Window.Current.Dispatcher;

            RaiseLoading();

            return Task.Run(async () =>
            {
                bool fMoveNext = false;
                int loadedCount = 0;
                IList<TModel> items = new List<TModel>();
                while (loadedCount <= pageSize && (fMoveNext = await this.enumerator.MoveNext()) == true)
                {
                    items.Add(enumerator.Current);
                    loadedCount++;
                }

                HasMoreItems = (fMoveNext != false);


                await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        foreach (TModel model in items)
                        {
                            var item = this.adapterMethod.Invoke(model);
                            if (item != null)
                            {
                                Add(this.adapterMethod.Invoke(model));
                            }
                        }
                        RaiseLoaded();
                    });

                return new LoadMoreItemsResult() { Count = Convert.ToUInt32(loadedCount) };
            }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
