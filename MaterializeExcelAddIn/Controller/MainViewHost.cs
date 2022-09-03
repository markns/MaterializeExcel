using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace MaterializeExcelAddIn.Controller
{
    [ComVisible(true)]
    public partial class MainViewHost : UserControl
    {
        public MainViewHost(UIElement control)
        {
            InitializeComponent();

            var wpfElementHost = new ElementHost
            {
                Dock = DockStyle.Fill
            };
            wpfElementHost.HostContainer.Children.Add(control);

            Controls.Add(wpfElementHost);
        }

        private void MainViewHost_Load(object sender, EventArgs e)
        {
            
        }
    }
}
