using System.Linq;
using System.Windows.Controls;

namespace TaskManagerEF.Controllers
{
    class ManagePanels
    {
        public static void ClearPanel(Panel panelName)
        {
            //With this fragment of code, we clean the panel
            int childCount = panelName.Children.Count;
            for (int j = 0; j < childCount; j++)
            {
                ///Remove the old.
                panelName.Children.RemoveAt(0);
            }
        }
    }
}
