using Microsoft.Maui.Controls;

namespace ExBookMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
           

        }
        //protected  bool OnBackButtonPressed(ShellNavigatingEventArgs args)
        //{
        //    base.OnNavigating(args);

        //    ShellNavigatingDeferral token = args.GetDeferral();
        //    args.Cancel();
        //    token.Complete();
        //    return true;
        //}
        
    }
}
