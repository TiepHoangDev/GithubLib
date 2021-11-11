using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace GithubLib.WPF.Models
{
    public class GistFilesModel : BaseModel
    {
        string filename;
        string content;
        string description;
        string gist_id;
        bool isPublic;

        public string FileName { get => filename; set => _notifyPropertyChanged(ref filename, value); }
        public string Content { get => content; set => _notifyPropertyChanged(ref content, value); }
        public string Description { get => description; set => _notifyPropertyChanged(ref description, value); }
        public bool IsPublic { get => isPublic; set => _notifyPropertyChanged(ref isPublic, value); }
        public string Gist_id { get => gist_id; set => _notifyPropertyChanged(ref gist_id, value); }
    }

    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void _notifyPropertyChanged<T>(ref T t, T value, [CallerMemberName] string propertyName = "")
        {
            if (t?.Equals(value) != true)
            {
                t = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Cmd<T> : ICommand where T : class
    {
        private Action<T> _action;

        public event EventHandler CanExecuteChanged;

        public Cmd(Action<T> _action)
        {
            this._action = _action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter as T);
        }
    }
}
