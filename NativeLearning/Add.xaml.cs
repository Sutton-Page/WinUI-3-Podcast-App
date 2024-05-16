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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeLearning
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Add : Page
    {
        public static String podcastSearchUrl = "https://itunes.apple.com/search?term={0}&media=podcast&limit=5";


        public Add()
        {
            this.InitializeComponent();
        }

        private void goHome(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Home));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchTerm(object sender, RoutedEventArgs e)
        {



        }
    }
}
