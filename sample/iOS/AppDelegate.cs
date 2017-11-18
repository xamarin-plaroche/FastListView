using System;
using System.Collections.Generic;
using System.Linq;
using FastListView.Renderers;
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace FLVDemo.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			global::Xamarin.Forms.Forms.Init();

			FastListViewRenderer.Init();

			CachedImageRenderer.Init();

			App.ScreenSize = new Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

			Console.WriteLine("ScreenSize {0}", App.ScreenSize);

			LoadApplication(new App());

			return base.FinishedLaunching(uiApplication, launchOptions);
		}
	}
}
