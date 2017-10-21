using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MegaDesk_6_DaleVictor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddQuote : Page
    {
        private static List<DesktopMaterial> materials = new List<DesktopMaterial>();

        private const int MINWIDTH = 24;
        private const int MAXWIDTH = 96;
        private const int MINDEPTH = 12;
        private const int MAXDEPTH = 48;

        public AddQuote()
        {
            foreach (DesktopMaterial dm in Enum.GetValues(typeof(DesktopMaterial)))
            {
                materials.Add(dm);
            }
            InitializeComponent();
            surfaceComboBox.ItemsSource = materials;
        }

        private void cancelButton_click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void submitButton_clickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = custNameBox.Text;
                int wide = Convert.ToInt32(widthBox.Text);
                int deep = Convert.ToInt32(depthBox.Text);
                int draw = Convert.ToInt32(drawSlider.Value);
                int days;
                if (rushCheckBox.IsChecked == true) { days = Convert.ToInt32(((ComboBoxItem)rushComboBox.SelectedItem).Content); } else { days = 14; }
                DesktopMaterial surface = (DesktopMaterial)surfaceComboBox.SelectedValue;
                Deskquote dq = new Deskquote(name, days, wide, deep, draw, surface);
                dq.writeQuote();
                var dialog = new MessageDialog("Your order submitted successfully. We will complete the order in " + days + " days.", "Order Submitted");
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                var res = await dialog.ShowAsync();
                Frame.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Submit Failed");
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                var res = await dialog.ShowAsync();
            }
        }

        private void rushCheckBox_click(object sender, RoutedEventArgs e)
        {
            if ((bool)rushCheckBox.IsChecked) { rushComboBox.IsEnabled = true; }
            else { rushComboBox.IsEnabled = false; }
        }

        private void custNameBox_clear(UIElement sender, GettingFocusEventArgs args)
        {
            if (custNameBox.Text == "Customer Name") { custNameBox.Text = ""; }
        }

        private void widthBox_clear(UIElement sender, GettingFocusEventArgs args)
        {
            if (widthBox.Text == "Width") { widthBox.Text = ""; }
        }

        private void depthBox_clear(UIElement sender, GettingFocusEventArgs args)
        {
            if (depthBox.Text == "Depth") { depthBox.Text = ""; }
        }
    }
}
