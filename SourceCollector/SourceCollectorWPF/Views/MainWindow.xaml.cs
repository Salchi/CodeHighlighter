using SourceCollectorWPF.BusinessLogic;
using SourceCollectorWPF.Models;
using SourceCollectorWPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Forms;

namespace SourceCollectorWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
