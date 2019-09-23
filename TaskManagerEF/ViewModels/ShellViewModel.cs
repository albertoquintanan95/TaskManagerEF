using MahApps.Metro.IconPacks;
using System;
using System.Linq;

namespace TaskManagerEF.ViewModels
{
    internal class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            // Build the menus
            this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.HomeSolid }, Text = "Home", NavigationDestination = new Uri("Views/HomeView.xaml", UriKind.RelativeOrAbsolute) });
            this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.TasksSolid }, Text = "Tasks", NavigationDestination = new Uri("Views/TasksView.xaml", UriKind.RelativeOrAbsolute) });
            this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ProjectDiagramSolid }, Text = "My Projects", NavigationDestination = new Uri("Views/AddProjectsView.xaml", UriKind.RelativeOrAbsolute) });
            this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CheckCircleSolid }, Text = "My Finished Projects", NavigationDestination = new Uri("Views/FinishedProjectsView.xaml", UriKind.RelativeOrAbsolute) });
            //this.Menu.Add(new MenuItem() {Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.TruckSolid}, Text = "Vendor Changes", NavigationDestination = new Uri("Views/VendorChangesUpdate.xaml", UriKind.RelativeOrAbsolute)});

            this.OptionsMenu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CogsSolid }, Text = "Settings", NavigationDestination = new Uri("Views/SettingsView.xaml", UriKind.RelativeOrAbsolute) });
        }

        public object GetItem(object uri)
        {
            return null == uri ? null : this.Menu.FirstOrDefault(m => m.NavigationDestination.Equals(uri));
        }

        public object GetOptionsItem(object uri)
        {
            return null == uri ? null : this.OptionsMenu.FirstOrDefault(m => m.NavigationDestination.Equals(uri));
        }
    }
}
