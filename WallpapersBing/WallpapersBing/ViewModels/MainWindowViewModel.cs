using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Base;

namespace WallpapersBing.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        string bingUrl = @"https://www.bing.com";
        string pathSaveDirectory = Path.Combine(Environment.CurrentDirectory, "Wallpapers");

        public MainWindowViewModel()
        {
            string url = bingUrl + @"/HPImageArchive.aspx?idx=0&n=10";
            UpdateListImages(url);
        }

        Model.imagesImage[] GetImages(string url)
        {
            Model.WebImageSource imageSource = new Model.WebImageSource(url);
            var res = imageSource.GetImages();
            return res.image;
        }
        void UpdateListImages(string url)
        {
            var res = GetImages(url);
            if (res.Length > 0)
            {
                Images.Clear();
                foreach (var item in res)
                {
                    Images.Add(new ImageView()
                    {
                        FullPath = bingUrl + item.url,
                        Name = item.urlBase.Split("=", StringSplitOptions.RemoveEmptyEntries)[1],
                        CreationTime = DateTime.Now
                    });
                }
            }            
        }


        public ObservableCollection<ImageView> Images { get; } = new ObservableCollection<ImageView>();


        ImageView _selectedImage = null;
        public ImageView SelectedImage 
        {
            get => _selectedImage;
            set
            { 
                SetProperty(ref _selectedImage, value, nameof(SelectedImage));

                if (!_selectedImage.FullPath.StartsWith(pathSaveDirectory))
                {
                    var path = SaveImage(_selectedImage.FullPath);
                    _selectedImage.FullPath = path;
                }

                Helpers.WallpaperSetter.SetWallpaper(_selectedImage.FullPath);
            }
        }

        private string SaveImage(string url)
        {
            using var client = new HttpClient();
            var responce = client.GetAsync(url).GetAwaiter().GetResult();
            var content = responce.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            responce.Dispose();
            
            string fullpath = Path.Combine(pathSaveDirectory, _selectedImage.Name) + ".jpg";

            FileStream stream = new FileStream(fullpath, FileMode.Create, FileAccess.Write);
            stream.Write(content);
            stream.Close();

            return fullpath;
        }
    }
}