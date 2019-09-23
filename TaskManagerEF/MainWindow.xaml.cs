using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using TaskManagerEF.ViewModels;
using MenuItem = TaskManagerEF.ViewModels.MenuItem;

namespace TaskManagerEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Navigation.Navigation.Frame = new Frame() { NavigationUIVisibility = NavigationUIVisibility.Hidden };
            Navigation.Navigation.Frame.Navigated += SplitViewFrame_OnNavigated;

            // Navigate to the home page.
            this.Loaded += (sender, args) => Navigation.Navigation.Navigate(new Uri("Views/HomeView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void SplitViewFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            HamburgerMenuControl.Content = e.Content;
            HamburgerMenuControl.SelectedItem = e.ExtraData ?? ((ShellViewModel)this.DataContext).GetItem(e.Uri);
            HamburgerMenuControl.SelectedOptionsItem = e.ExtraData ?? ((ShellViewModel)this.DataContext).GetOptionsItem(e.Uri);
            GoBackButton.Visibility = Navigation.Navigation.Frame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            Navigation.Navigation.GoBack();
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            var menuItem = e.InvokedItem as MenuItem;
            if (menuItem != null && menuItem.IsNavigation)
            {
                Navigation.Navigation.Navigate(menuItem.NavigationDestination, menuItem);
            }
        }
    }
}
