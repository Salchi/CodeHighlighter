using CodeHighlighter.BusinessLogic;
using CodeHighlighter.Models;
using CodeHighlighter.ViewModels;
using System;
using System.Windows;
using System.Windows.Forms;

namespace CodeHighlighter.Views
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
