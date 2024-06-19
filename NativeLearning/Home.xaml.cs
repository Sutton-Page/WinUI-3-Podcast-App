using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Devices.Enumeration;
using Windows.Storage;
using Windows.Security.Cryptography.Certificates;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeLearning
{
    
    public sealed partial class Home : Page
    {

        public PodcastViewModel ViewModel { get; set; }

       
        

        public Home()
        {
            this.InitializeComponent();
            this.ViewModel = new PodcastViewModel();
           
            

            
        }

        

        private void toAdd(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Search));
        }


        

        private void podContent_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = e.ClickedItem as Podcast;

            if(clicked != null)
            {
                Frame.Navigate(typeof(Content), clicked);

            }
        }
    }
}
