using Xamarin.Forms;

namespace FastListView.Forms.VisualElements
{
	public class FastViewCell : ViewCell
	{
		public FastViewCell() { }

		public void SendAppearing() => OnAppearing();

		public void SendDisappearing() => OnDisappearing();
	}
}