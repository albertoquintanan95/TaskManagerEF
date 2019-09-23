using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManagerEF.Controllers
{
    class ActiveDirectoryController
    {
        DirectoryEntry rootDSE = new DirectoryEntry();
        DirectoryEntry searchRoot = new DirectoryEntry();
        DirectoryEntry userEntry = new DirectoryEntry();
        DirectorySearcher searcher = new DirectorySearcher();
        SearchResultCollection results = null;
        MetroWindow metroWindow = (Application.Current.MainWindow as MetroWindow);

        public async Task<string> SearchAD(string displayName)
        {
            rootDSE = new DirectoryEntry("LDAP://rootDSE");
            searcher = new DirectorySearcher(searchRoot);
            searcher.PropertiesToLoad.Add("displayname"); //Este es el campo del active directory para regresar el nombre del usuario
            searcher.PropertiesToLoad.Add("SamAccountName"); //Este es el campo del active directory para regresar el nombre del usuario
            searcher.Filter = string.Format("displayname={0}*", displayName);

            searcher.SearchScope = SearchScope.Subtree;
            searcher.CacheResults = false;
            results = searcher.FindAll();
            try
            {
                foreach (SearchResult result in results)
                {
                    return result.Properties["SamAccountName"][0].ToString();
                }
            }
            catch (Exception ex)
            {
                await metroWindow.ShowMessageAsync("Warning", ex.Message);
            }
            return null;
        }

        public List<string> SearchContacts(string search)
        {
            if (search.Length > 4)
            {
                List<string> s = new List<string>();
                rootDSE = new DirectoryEntry("LDAP://rootDSE");
                searcher = new DirectorySearcher(searchRoot);
                searcher.PropertiesToLoad.Add("displayname"); //Este es el campo del active directory para regresar el nombre del usuario
                searcher.Filter = string.Format("displayname={0}*", search);

                searcher.SearchScope = SearchScope.Subtree;
                searcher.CacheResults = false;
                results = searcher.FindAll();
                try
                {
                    foreach (SearchResult result in results)
                    {
                        s.Add(result.Properties["displayname"][0].ToString());
                    }
                    return s;
                }
                catch (Exception ex)
                {
                    //await metroWindow.ShowMessageAsync("Warning", ex.Message);
                    return null;
                }
            }
            return null;
        }
    }
}
