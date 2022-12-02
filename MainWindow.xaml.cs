using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave_2_ABE
{
    public partial class MainWindow : Window
    {
        string DestFileName = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            MultiCpoy();
        }
        public void SelectBrowse()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "All files (*.*)|*.*";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (fileDialog.ShowDialog() == true)
            {
                foreach (string filename in fileDialog.FileNames)
                {
                    if (EXE.IsChecked == true)
                    {
                        if (System.IO.Path.GetExtension(filename) != ".exe")
                        {
                            lbFiles.Items.Add(filename);
                            dstlbFiles.Items.Add(System.IO.Path.GetFileName(filename));
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        lbFiles.Items.Add(filename);
                        dstlbFiles.Items.Add(System.IO.Path.GetFileName(filename));
                    }


                    // lbFiles.Items.Add(Path.GetFileName(filename));

                    //txtSource.Text = filename; // lbFiles.Items.Add(Path.GetFileName(filename));
                    //DestFileName = System.IO.Path.GetFileName(filename);
                }
            }
        }
        public void MultiCpoy()
        {
            int i = 0;
            foreach (string file in lbFiles.Items)
            {

                var _source = new FileInfo(file);
                var _destination = new FileInfo(txtDestination.Text + "/" + dstlbFiles.Items[i].ToString());
                i++;
                //Check if the file exists, we will delete it
                if (_destination.Exists)
                    _destination.Delete();
                //Create a tast to run copy file
                Task.Run(() =>
                {
                    _source.Copyfile(_destination, x => progressBar.Dispatcher.BeginInvoke(new Action(() => { progressBar.Value = x; lblPercent.Content = x.ToString() + "%"; })));
                }).GetAwaiter().OnCompleted(() => progressBar.Dispatcher.BeginInvoke(new Action(() => { progressBar.Value = 100; lblPercent.Content = "100%"; })));

            }
            lbFiles.Items.Clear();
            dstlbFiles.Items.Clear();
        }


        public void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SelectBrowse();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
