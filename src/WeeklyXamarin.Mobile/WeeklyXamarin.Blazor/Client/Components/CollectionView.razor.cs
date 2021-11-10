﻿using Microsoft.AspNetCore.Components;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using WeeklyXamarin.Core.Helpers;

namespace WeeklyXamarin.Blazor.Client.Components
{
    public partial class CollectionView<TItem> : ComponentBase
    {
        [Parameter]
        public RenderFragment<TItem>? ChildContent { get; set; }
        [Parameter]
        public IEnumerable<TItem>? ItemsSource { get; set; }
        [Parameter]
        public ListState CurrentState { get; set; }
        [Parameter]
        public string? EmptyText { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (ItemsSource != null && ItemsSource is ObservableRangeCollection<TItem> collection)
            {
                collection.CollectionChanged += CollectionChanged;
            }
        }

        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }
    }
}
