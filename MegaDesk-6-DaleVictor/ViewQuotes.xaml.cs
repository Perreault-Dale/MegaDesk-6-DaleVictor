using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ViewQuotes : Page
    {
        public ViewQuotes()
        {
            this.InitializeComponent();
            getQuotes();
        }

        private void cancelButton_click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void getQuotes()
        {
            StorageFolder path1 = ApplicationData.Current.LocalFolder;
            StorageFile sr = await path1.GetFileAsync("quotes.json");
            ObservableCollection<string> list = new ObservableCollection<string>();
            IList<String> allQuotes = await FileIO.ReadLinesAsync(sr);
            foreach (var json in allQuotes)
            {
                if (json != "")
                {
                    Deskquote dq1 = JsonConvert.DeserializeObject<Deskquote>(json);
                    string myListItem = "Order: " + dq1.custName + ", ";
                    myListItem = myListItem + dq1.custDesk.width + "'' wide X " + dq1.custDesk.depth + "'' deep, ";
                    myListItem = myListItem + dq1.custDesk.drawers + " drawers, ";
                    myListItem = myListItem + dq1.custDesk.surface.ToString() + " desktop, ";
                    myListItem = myListItem + dq1.orderDate + ", ";
                    myListItem = myListItem + "$" + dq1.price + ", ";
                    myListItem = myListItem + dq1.prodDays + " days to deliver.";
                    list.Add(myListItem);
                    // var dialog = new MessageDialog(myListItem);
                    // dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    // var res = await dialog.ShowAsync();
                }
            }
            quoteGridView.ItemsSource = list;
        }
    }
}
