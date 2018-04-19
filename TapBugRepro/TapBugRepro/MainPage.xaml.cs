using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TapBugRepro
{
  public partial class MainPage : ContentPage
  {
    ObservableCollection<string> _Output = new ObservableCollection<string>();
    public MainPage()
    {
      InitializeComponent();
      lvOutput.ItemsSource = _Output;
    }

    const int threshold = 100;
    void OnPan(object sender, PanUpdatedEventArgs e)
    {
      if (e.StatusType != GestureStatus.Running)
        return;

      var x = Math.Abs(e.TotalX);
      var y = Math.Abs(e.TotalY);

      if (Device.RuntimePlatform == Device.UWP && x + y < threshold)
        return;

      var isX = x >= y;
      var output = $"{(isX ? "X" : "Y")}: {(isX ? e.TotalX : e.TotalY)}";
      _Output.Add(output);
      lvOutput.ScrollTo(output, ScrollToPosition.End, false);
    }

    private void OnClear(object sender, EventArgs e) =>
      _Output.Clear();
  }
}
