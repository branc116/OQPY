using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BucketApp.Views
{
	public partial class EditFloorPage : ContentPage
	{
        CocosScenes.ShowFloor Floor;
        private CocosSharpView _vi;
        public EditFloorPage()
        {
            
            InitializeComponent();
            this.BackgroundColor = CocosScenes.Helper.Colors.CCColor3BXC(CocosScenes.Helper.Colors.DarkBackgroundColor);

            _vi = new CocosSharpView()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = CocosScenes.Helper.Colors.CCColor3BXC(CocosScenes.Helper.Colors.DarkBackgroundColor),
                ViewCreated = HandleViewCreated,
                DesignResolution = new Size( Width, Height)
               
            };
            this.Content = _vi;
        }
        private void HandleViewCreated(object sender, EventArgs e)
        {
            if (sender is CCGameView view)
            {
                
                Floor = new CocosScenes.ShowFloor(view);
                view.RunWithScene(Floor);
                this.Content = _vi;
            }
        }
    }
        
}
