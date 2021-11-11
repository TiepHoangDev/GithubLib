using GithubLib.Gists;
using GithubLib.WPF.Models;
using GithubLib.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GithubLib.WPF.Views
{
    /// <summary>
    /// Interaction logic for Gists.xaml
    /// </summary>
    public partial class Gists : Window
    {
        public Gists()
        {
            InitializeComponent();
            this._dc = new GistViewModel();
            this.DataContext = _dc;
        }
        GistViewModel _dc;
        private async void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((ListView)sender).SelectedItem is GistFilesModel f)
            {
                _dc.FileNew = new GistFilesModel
                {
                    Content = await GistHelper.Get(f.Content),
                    Description = f.Description,
                    FileName = f.FileName,
                    Gist_id = f.Gist_id,
                    IsPublic = f.IsPublic,
                };
            }
        }
    }
}
