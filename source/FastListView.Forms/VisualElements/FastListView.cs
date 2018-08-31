using System;
using System.Collections;
using System.Windows.Input;
using FastListView.Forms.Contracts;
using Xamarin.Forms;

namespace FastListView.Forms.VisualElements
{
	public class FastListView : View
	{
		public static readonly BindableProperty IsPullToRefreshEnabledProperty = BindableProperty.Create("IsPullToRefreshEnabled", typeof(bool), typeof(FastListView), false);

		public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create("IsRefreshing", typeof(bool), typeof(FastListView), false, BindingMode.TwoWay);

		public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create("RefreshCommand", typeof(ICommand), typeof(FastListView), null);

		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(FastListView), null, BindingMode.OneWayToSource);

		public static readonly BindableProperty HasUnevenRowsProperty = BindableProperty.Create("HasUnevenRows", typeof(bool), typeof(FastListView), false);

		public static readonly BindableProperty RowHeightProperty = BindableProperty.Create("RowHeight", typeof(int), typeof(FastListView), 44);

		public static readonly BindableProperty SeparatorVisibilityProperty = BindableProperty.Create("SeparatorVisibility", typeof(SeparatorVisibility), typeof(FastListView), SeparatorVisibility.Default);

		public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create("SeparatorColor", typeof(Color), typeof(FastListView), Color.LightGray);

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(FastListView), null, BindingMode.TwoWay, propertyChanged: OnItemsSourceChanged);

		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(FastListView), null, validateValue: ValidateItemTemplate);

		public static readonly BindableProperty ScrollEnabledProperty = BindableProperty.Create("ScrollEnabled", typeof(bool), typeof(FastListView), false);

		public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create("SelectedCommand", typeof(ICommand), typeof(FastListView), null);

		public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create("LoadMoreCommand", typeof(ICommand), typeof(FastListView), null);

		public ICommand LoadMoreCommand
		{
			get { return (ICommand)GetValue(LoadMoreCommandProperty); }
			set { SetValue(LoadMoreCommandProperty, value); }
		}

		public ICommand SelectedCommand
		{
			get { return (ICommand)this.GetValue(SelectedCommandProperty); }
			set { this.SetValue(SelectedCommandProperty, value); }
		}

		public bool ScrollEnabled
		{
			get { return (bool)GetValue(ScrollEnabledProperty); }
			set { SetValue(ScrollEnabledProperty, value); }
		}

		public bool HasUnevenRows
		{
			get { return (bool)GetValue(HasUnevenRowsProperty); }
			set { SetValue(HasUnevenRowsProperty, value); }
		}

		public bool IsPullToRefreshEnabled
		{
			get { return (bool)GetValue(IsPullToRefreshEnabledProperty); }
			set { SetValue(IsPullToRefreshEnabledProperty, value); }
		}

		public bool IsRefreshing
		{
			get { return (bool)GetValue(IsRefreshingProperty); }
			set { SetValue(IsRefreshingProperty, value); }
		}

		public ICommand RefreshCommand
		{
			get { return (ICommand)GetValue(RefreshCommandProperty); }
			set { SetValue(RefreshCommandProperty, value); }
		}

		public int RowHeight
		{
			get { return (int)GetValue(RowHeightProperty); }
			set { SetValue(RowHeightProperty, value); }
		}

		public object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public Color SeparatorColor
		{
			get { return (Color)GetValue(SeparatorColorProperty); }
			set { SetValue(SeparatorColorProperty, value); }
		}

		public SeparatorVisibility SeparatorVisibility
		{
			get { return (SeparatorVisibility)GetValue(SeparatorVisibilityProperty); }
			set { SetValue(SeparatorVisibilityProperty, value); }
		}

		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		public event EventHandler<ItemVisibilityEventArgs> ItemAppearing;

		public event EventHandler<ItemVisibilityEventArgs> ItemDisappearing;

		public FastListView()
		{
			VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;

			ItemAppearing += InfiniteListView_ItemAppearing;
		}

		void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			var items = ItemsSource as IList;
			if (items != null && items.Count > 2 && e.Item == items[items.Count - 1])
			{
				if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
				{
					LoadMoreCommand.Execute(null);
				}
			}
		}

		public FastViewCell CreateDefault(object item)
		{
			if (item is IFastViewCell)
			{
				var viewCell = (IFastViewCell)item;
				var template = viewCell.ItemTemplate;
				if (template != null)
				{
					var templateInstance = template.CreateContent();
					// see if it's a view or a cell
					var templateView = templateInstance as View;
					var templateCell = templateInstance as FastViewCell;
					if (templateView == null && templateCell == null)
						throw new InvalidOperationException("DataTemplate must be either a Cell or a View");
					if (templateView != null) // we got a view, wrap in a cell
						templateCell = new FastViewCell { View = templateView };
					return templateCell;
				}
			}
			else if (item is FastViewCell)
			{
				return (FastViewCell)item;
			}

			// -- Default ViewCell
			string text = null;
			if (item != null)
				text = item.ToString();

			var sl = new StackLayout();
			sl.HorizontalOptions = LayoutOptions.Fill;
			sl.VerticalOptions = LayoutOptions.Fill;
			sl.HeightRequest = 44;

			var lbl = new Label();
			lbl.Text = text;
			lbl.FontSize = 12;
			lbl.HorizontalOptions = LayoutOptions.CenterAndExpand;
			lbl.VerticalOptions = LayoutOptions.CenterAndExpand;

			sl.Children.Add(lbl);

			var vc = new FastViewCell();
			vc.View = sl;
			return vc;
		}

		static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var element = newValue as Element;
			if (element == null)
				return;
			element.Parent = (Element)bindable;
		}

		public void SendCellAppearing(Cell cell)
		{
			ItemAppearing?.Invoke(this, new ItemVisibilityEventArgs(cell.BindingContext));
		}

		public void SendCellDisappearing(Cell cell)
		{
			ItemDisappearing?.Invoke(this, new ItemVisibilityEventArgs(cell.BindingContext));
		}

		static bool ValidateItemTemplate(BindableObject b, object v)
		{
			var lv = b as FastListView;
			if (lv == null)
				return true;

			return false;
		}
	}
}	
