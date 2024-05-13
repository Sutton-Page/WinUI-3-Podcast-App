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
    public sealed partial class Content : Page
    {
        Podcast podcastData;
        public Content()
        {
            this.InitializeComponent();

            

        }

        String lg = "Explore true stories of the dark side of the Internet with host Jack Rhysider \n as he takes you on a journey through the chilling world of hacking, \n data breaches, and cyber crime.";


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is  Podcast)
            {
                podcastData = e.Parameter as Podcast;
            }

            desc.Text = lg;

            base.OnNavigatedTo(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Home));
        }
    }
}
