namespace CustomGarmentApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(
                "RegisterPage",
                typeof(Pages.RegisterPage));

            Routing.RegisterRoute(
                "GarmentPage",
                typeof(Pages.GarmentPage));

        }
    }
}
