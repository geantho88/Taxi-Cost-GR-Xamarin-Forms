using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiCostXamarin.ViewModel;
using Xamarin.Forms;

namespace TaxiCostXamarin
{
    public partial class MenuPage : TabbedPage
	{
        ObservableCollection<CatalogItem> CatalogItems = new ObservableCollection<CatalogItem>();

		public MenuPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            GetCatalog();
		}

        void GetCatalog()
        {
            CatalogItem catalogItem1 = new CatalogItem
            {
                CatalogIcon = "flag.png",
                CatalogDescription = "Πτώση σημαίας (Αρχική Χρέωση)",
                CatalogPrice = "1.29 €"
            };

            CatalogItem catalogItem2 = new CatalogItem
            {
                CatalogIcon = "sun.png",
                CatalogDescription = "Χρέωση χιλιομέτρου (Ταρίφα 1) ισχύει απο τις 05:00 το πρωί μέχρι τα μεσάνυκτα",
                CatalogPrice = "0.74 €"
            };

            CatalogItem catalogItem3 = new CatalogItem
            {
                CatalogIcon = "moon.png",
                CatalogDescription = "Χρέωση χιλιομέτρου (Ταρίφα 2) ισχύει απο τα μεσάνυκτα μέχρι τις 05:00 το πρωί",
                CatalogPrice = "1.29 €"
            };

            CatalogItem catalogItem4 = new CatalogItem
            {
                CatalogIcon = "group.png",
                CatalogDescription = "Ελάχιστη χρέωση (Αθήνα μόνο)",
                CatalogPrice = "3.44 €"
            };

            CatalogItem catalogItem5 = new CatalogItem
            {
                CatalogIcon = "nature.png",
                CatalogDescription = "Ελάχιστη χρέωση (Υπόλοιπη Ελλάδα)",
                CatalogPrice = "3.69 €"
            };

            CatalogItem catalogItem6 = new CatalogItem
            {
                CatalogIcon = "travel.png",
                CatalogDescription = "Από και προς αεροδρόμιο Αθήνας  - έξτρα χρέωση",
                CatalogPrice = "4.18 €"
            };

            CatalogItem catalogItem7 = new CatalogItem
            {
                CatalogIcon = "travel.png",
                CatalogDescription = "Από και προς αεροδρόμιο Θεσσαλονίκης  - έξτρα χρέωση",
                CatalogPrice = "3.44 €"
            };

            CatalogItem catalogItem8 = new CatalogItem
            {
                CatalogIcon = "travel.png",
                CatalogDescription = "Από και προς αεροδρόμιο επαρχίας  - έξτρα χρέωση",
                CatalogPrice = "2.83 €"
            };

            CatalogItem catalogItem9 = new CatalogItem
            {
                CatalogIcon = "bags.png",
                CatalogDescription = "Αποσκευή (Άνω των 10 kg / τεμάχιο) - έξτρα χρέωση",
                CatalogPrice = "0.43 €"
            };

            CatalogItem catalogItem10 = new CatalogItem
            {
                CatalogIcon = "transport.png",
                CatalogDescription = "Από και πρός Λιμάνια / ΚΤΕΛ / Σιδ. Σταθμοί  - έξτρα χρέωση",
                CatalogPrice = "1.17 €"
            };

            CatalogItem catalogItem11 = new CatalogItem
            {
                CatalogIcon = "travel.png",
                CatalogDescription = "Από και προς Αεροδρόμιο Αθήνας προς Κέντρο (05:00 – 24:00) - Κλειστή τιμή",
                CatalogPrice = "35.00 €"
            };

            CatalogItem catalogItem12 = new CatalogItem
            {
                CatalogIcon = "travel.png",
                CatalogDescription = "Από και προς Αεροδρόμιο Αθήνας προς Κέντρο (24:00 – 05:00) - Κλειστή τιμή",
                CatalogPrice = "50.00 €"
            };

            CatalogItems.Add(catalogItem1);
            CatalogItems.Add(catalogItem2);
            CatalogItems.Add(catalogItem3);
            CatalogItems.Add(catalogItem4);
            CatalogItems.Add(catalogItem5);
            CatalogItems.Add(catalogItem6);
            CatalogItems.Add(catalogItem7);
            CatalogItems.Add(catalogItem8);
            CatalogItems.Add(catalogItem9);
            CatalogItems.Add(catalogItem10);
            CatalogItems.Add(catalogItem11);
            CatalogItems.Add(catalogItem12);

            CatalogList.ItemsSource = CatalogItems;
        }
	}
}
