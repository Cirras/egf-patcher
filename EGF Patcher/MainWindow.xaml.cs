using EGF_Patcher.Properties;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;

namespace EGF_Patcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string OriginalWorkingDirectory = Directory.GetCurrentDirectory(); 
        private string egfPath;
        private string[] bmpPaths;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_SelectEGFFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "EGF files (*.egf)|*.egf|All files (*.*)|*.*"
            };

            String egfDirectory = (String) Settings.Default["EGFPath"];
            if (Directory.Exists(egfDirectory))
            {
                openFileDialog.InitialDirectory = egfDirectory;
            }
            
            if (openFileDialog.ShowDialog() == true)
            {
                Settings.Default["EGFPath"] = Path.GetDirectoryName(openFileDialog.FileName);
                Settings.Default.Save();

                selectedEGFLabel.Content = Path.GetFileName(openFileDialog.FileName);
                egfPath = openFileDialog.FileName;
                ResetWorkingDirectory();
                UpdatePatchButtonEnableState();
            }
        }

        private void Button_SelectBMPFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "BMP files (*.bmp)|*.bmp",
                Multiselect = true
            };

            String bmpDirectory = (String)Settings.Default["BMPPath"];
            if (Directory.Exists(bmpDirectory))
            {
                openFileDialog.InitialDirectory = bmpDirectory;
            }

            if (openFileDialog.ShowDialog() == true)
            {
                Settings.Default["BMPPath"] = Path.GetDirectoryName(openFileDialog.FileName);
                Settings.Default.Save();

                selectedBMPsLabel.Content = $"{openFileDialog.FileNames.Length} BMPs selected";
                bmpPaths = openFileDialog.FileNames;
                ResetWorkingDirectory();
                UpdatePatchButtonEnableState();
            }
        }

        private void Button_Patch(object sender, RoutedEventArgs e)
        {
            IndexInputDialog inputDialog = new IndexInputDialog();
            inputDialog.Owner = this;

            if (inputDialog.ShowDialog() == true)
            {
                try
                {
                    int gfxIndex = parseGFXIndex(inputDialog.gfxID);
                    GFXFile egf = new GFXFile(egfPath);

                    for (int i = 0; i < bmpPaths.Length; ++i)
                    {
                        Bitmap bitmap = new Bitmap(bmpPaths[i]);
                        egf.Update(gfxIndex + i, bitmap);
                        bitmap.Dispose();
                    }

                    egf.Commit();

                    MessageBox.Show(
                        $"{bmpPaths.Length} BMPs inserted into {selectedEGFLabel.Content}",
                        "Success"
                     );

                    selectedBMPsLabel.Content = String.Empty;
                    bmpPaths = null;
                    patchButton.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message, 
                        "Error", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error
                   );
                }
            }
        }

        private void ResetWorkingDirectory()
        {
            Directory.SetCurrentDirectory(OriginalWorkingDirectory);
        }

        private void UpdatePatchButtonEnableState()
        {
            patchButton.IsEnabled = bmpPaths != null && egfPath != null;
        }

        private static int parseGFXIndex(String input)
        {
            if (!int.TryParse(input, out int gfxIndex) || gfxIndex < 1)
            {
                throw new Exception($"Invalid GFX ID: {input}");
            }
            return gfxIndex;
        }
    }
}
