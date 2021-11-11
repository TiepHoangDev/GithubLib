using GithubLib.Gists;
using GithubLib.WPF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GithubLib.WPF.ViewModels
{
    public class GistViewModel : BaseModel
    {
        private Cmd<object> refresh;
        private Cmd<object> createNew;
        /// <summary>
        /// A https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token
        /// </summary>
        private string _token = "ghp_YrRu56vRHygcaBg11DRUhOhj8pOOw34g8ee";
        private ICollection<GistFilesModel> gistFiles;

        public GistFilesModel FileNew
        {
            get => fileNew;
            set
            {
                _notifyPropertyChanged(ref fileNew, value);
            }
        }
        public ICollection<GistFilesModel> GistFiles
        {
            get => gistFiles;
            set => _notifyPropertyChanged(ref gistFiles, value);
        }

        public string Token
        {
            get => _token;
            set => _notifyPropertyChanged(ref _token, value);
        }

        public Cmd<object> Refresh
        {
            get
            {
                if (refresh == null)
                {
                    refresh = new Cmd<object>(async _ =>
                    {
                        try
                        {
                            var text = await new GistHelper(this._token).GetByUser("tiephoangdev");
                            GistFiles = JsonConvert.DeserializeObject<List<object>>(text)?.Select(q =>
                            {
                                var a = JsonConvert.DeserializeAnonymousType(q + "", new
                                {
                                    html_url = "",
                                    id = "",
                                    description = "",
                                    @public = false,
                                    files = new object()
                                });
                                var files = JObject.FromObject(a.files);
                                var data = JsonConvert.DeserializeAnonymousType(files.First.First + "", new
                                {
                                    filename = "",
                                    raw_url = "",
                                });
                                return new GistFilesModel
                                {
                                    FileName = data.filename,
                                    Content = data.raw_url,
                                    Description = a.description,
                                    Gist_id = a.id,
                                    IsPublic = a.@public
                                };
                            })?.ToList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    });
                }
                return refresh;
            }
        }

        Cmd<Object> _delete;
        public Cmd<object> CreateNew
        {
            get
            {
                if (createNew == null)
                {
                    createNew = new Cmd<object>(async o =>
                    {
                        try
                        {
                            await new GistHelper(this._token).CreateNew(FileNew.FileName, FileNew.Content);
                            MessageBox.Show("success");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    });
                }
                return createNew;
            }
        }

        public Cmd<object> Save
        {
            get
            {
                if (_Save == null)
                {
                    _Save = new Cmd<object>(async o =>
                    {
                        try
                        {
                            await new GistHelper(this._token).Update(FileNew.Gist_id, FileNew.FileName, FileNew.Content);
                            MessageBox.Show("success");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    });
                }
                return _Save;
            }
        }

        public Cmd<object> Delete
        {
            get
            {
                if (_delete == null)
                {
                    _delete = new Cmd<object>(async o =>
                    {
                        try
                        {
                            await new GistHelper(this._token).Delete(FileNew.Gist_id);
                            MessageBox.Show("success");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    });
                }
                return _delete;
            }
        }

        Cmd<object> _Save;
        private GistFilesModel fileNew = new GistFilesModel();
    }
}
