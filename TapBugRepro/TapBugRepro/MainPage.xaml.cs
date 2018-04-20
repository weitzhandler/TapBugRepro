using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TapBugRepro
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
    }

    public int Value
    {
      get => (int)GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
    }
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(int), typeof(MainPage), default(int));


    const int threshold = 100;
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
      if (e.StatusType == GestureStatus.Completed)
        Debug.WriteLine(new string('*', 20));

      if (e.StatusType != GestureStatus.Running)
        return;

      var value = CalculateValue(e.TotalY);

      if (value == 0)
        return;

      Value += -value;
    }

    List<double> _History = new List<double>();
    int CalculateValue(double panDelta)
    {
      Debug.WriteLine(panDelta);

      _History.Add(panDelta);

      var result = _History.Sum() / GetFactor();
      if (Math.Abs(result) > 1)
      {
        _History.Clear();
        return (int)Math.Round(result);
      }
      return 0;
    }

    int GetFactor()
    {       
      switch (Device.RuntimePlatform)
      {                    
        case Device.UWP:
        case Device.WinPhone:
        case Device.WinRT:
        case Device.Android:
        case Device.iOS:
        case Device.macOS:
        default:
          return 200;
      }
    }


  }
}
