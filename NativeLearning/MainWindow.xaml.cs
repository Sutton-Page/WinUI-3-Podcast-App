 using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeLearning
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MainWindow : Window
    {
        private bool _clicked;
        public PodcastViewModel ViewModel {get; set; }

        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public MainWindow()
        {
            this.InitializeComponent();
            this.ViewModel = new PodcastViewModel();

            setupConfig();
            setupApp();


            
        }

        private async void setupConfig()
        {
            try
            {


                CStorage config = new CStorage();

                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("config.json", CreationCollisionOption.FailIfExists);

                var handle = await file.OpenStreamForWriteAsync();

                await JsonSerializer.SerializeAsync(handle, config);

            }

            catch(System.Exception ex)
            {

            }

        }

        private void setupApp()
        {

            nav.Navigate(typeof(Home));
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        
        


        /*
        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            // myButton.Content = "Clicked";

            if (_clicked)
            {
                myButton.Content = "Clicked";

            }
            else
            {
                myButton.Content = "Click Me";
            }

            this._clicked = !this._clicked;


        }*/
    }
}
