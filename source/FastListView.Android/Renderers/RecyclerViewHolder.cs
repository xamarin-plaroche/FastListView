using Android.Content.Res;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FastListView.Renderers
{
	public class RecyclerViewHolder : RecyclerView.ViewHolder
	{
		private ViewCell _viewCell;

		public RecyclerViewHolder(Android.Views.View itemView) : base(itemView)
		{
			ItemView = itemView;
			//ItemView.SetTag(1110010101, this);
		}

		public void UpdateUi(ViewCell viewCell, object dataContext, Forms.VisualElements.FastListView listview, int height)
		{
			var contentLayout = (FrameLayout)ItemView;

			viewCell.BindingContext = dataContext;
			viewCell.Parent = listview;

			var metrics = Resources.System.DisplayMetrics;
			// Layout and Measure Xamarin Forms View

			var elementSizeRequest = viewCell.View.Measure(listview.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);

			var _height = (int)((elementSizeRequest.Request.Height + viewCell.View.Margin.Top + viewCell.View.Margin.Bottom) * metrics.Density);
			//var _height = (int)((height + viewCell.View.Margin.Top + viewCell.View.Margin.Bottom) * metrics.Density);
			var _width = (int)((listview.Width + viewCell.View.Margin.Left + viewCell.View.Margin.Right) * metrics.Density);

			viewCell.View.Layout(new Rectangle(0, 0, listview.Width, elementSizeRequest.Request.Height));

			//viewCell.View.Layout(new Rectangle(0, 0, listview.Width, _height));

			// Layout Android View
			var layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
			{
				Height = _height,
				Width = _width
			};

			if (Platform.GetRenderer(viewCell.View) == null)
			{
				Platform.SetRenderer(viewCell.View, Platform.CreateRenderer(viewCell.View));
			}
			var renderer = Platform.GetRenderer(viewCell.View);

			//var viewGroup = renderer.ViewGroup;
			var viewGroup = renderer.View;
			viewGroup.LayoutParameters = layoutParams;
			viewGroup.Layout(0, 0, _width, _height);
			contentLayout.RemoveAllViews();
			contentLayout.AddView(viewGroup);
		}
	}
}
