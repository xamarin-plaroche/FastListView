using System;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FastListView.Renderers
{
    public class RecyclerViewHolder : RecyclerView.ViewHolder, Android.Views.View.IOnClickListener
    {
		private ViewCell _viewCell;
        FastListView.Forms.VisualElements.FastListView _listview;

        public RecyclerViewHolder(Android.Views.View itemView) : base(itemView)
        {
            ItemView = itemView;
            //ItemView.SetTag(1110010101, this);
            itemView.SetOnClickListener(this);
        }

        public void OnClick(Android.Views.View v)
        {
            Console.WriteLine("OnClick!");
            //int position = AdapterPosition;

            if (_listview.SelectedCommand != null && _listview.SelectedCommand.CanExecute(_viewCell.BindingContext))
            {
                _listview.SelectedCommand.Execute(_viewCell.BindingContext);
            }
        }

        public void UpdateUi(ViewCell viewCell, object dataContext, FastListView.Forms.VisualElements.FastListView listview, int height)
        {
            var contentLayout = (FrameLayout)ItemView;

            _listview = listview;
            _viewCell = viewCell;

            viewCell.BindingContext = dataContext;
            viewCell.Parent = listview;

            var metrics = Resources.System.DisplayMetrics;
            // Layout and Measure Xamarin Forms View

            var _view = Platform.CreateRendererWithContext(viewCell.View, Android.App.Application.Context);
            SizeRequest elementSizeRequest = _view.Element.Measure(Android.App.Application.Context.FromPixels(listview.Width), double.PositiveInfinity, MeasureFlags.IncludeMargins);
            var pixel_height = (int)Android.App.Application.Context.ToPixels(_viewCell.Height > 0 ? _viewCell.Height : elementSizeRequest.Request.Height);

            //var elementSizeRequest = viewCell.View.Measure(listview.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);

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
                Platform.SetRenderer(viewCell.View, Platform.CreateRendererWithContext(viewCell.View, Android.App.Application.Context));
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
