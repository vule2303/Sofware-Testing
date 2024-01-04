using System.Windows.Controls;

namespace TestBuilder.View.UserControls
{
  
    public partial class buttonPreviousNav : UserControl
    {
        public buttonPreviousNav()
        {
            InitializeComponent();
        }
        private string labelHeading;

        public string LabelHeading
        {
            get { return labelHeading; }
            set { 
                labelHeading = value;
                lbHeading.Text = labelHeading;
            }
        }
    }
   

}
