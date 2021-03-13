using System;
using System.Windows;
using System.Windows.Controls;

namespace EGF_Patcher
{
    /// <summary>
    /// Interaction logic for IndexInputDialog.xaml
    /// </summary>
    public partial class IndexInputDialog : Window
    {
        public IndexInputDialog()
        {
            InitializeComponent();
        }

        private void Button_Ok(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            indexInput.SelectAll();
            indexInput.Focus();
        }

        public string gfxID
        {
            get { return indexInput.Text; }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            okButton.IsEnabled = !String.IsNullOrEmpty(indexInput.Text);
        }
    }
}
