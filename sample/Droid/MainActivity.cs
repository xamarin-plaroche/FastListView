using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;
using Android.Graphics.Drawables;

namespace FLVDemo.Droid
{
	[Activity(Label = "FLVDemo.Droid", Icon = "@mipmap/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			this.Window.SetBackgroundDrawable(new ColorDrawable() { Color = Android.Graphics.Color.White });

			FastListView.Renderers.FastListViewRenderer.Init(); 

			CachedImageRenderer.Init();

			App.ScreenSize = new Xamarin.Forms.Size(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density,
														Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
			Console.WriteLine("App.ScreenSize : {0}", App.ScreenSize);

			LoadApplication(new App());
		}
	}
}
