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
    class MainWindowViewModel : ViewModelBase, IDisposable
    {
        Model.WebImageSource ImageSource = null;
        string bingUrl = @"https://www.bing.com";
        string pathSaveDirectory = Environment.CurrentDirectory;

        public MainWindowViewModel()
        {
            string url = bingUrl + @"/HPImageArchive.aspx?idx=0&n=10";
            ImageSource = new Model.WebImageSource(url);

            UpdateListImages();
        }

        Model.imagesImage[] GetImages()
        {            
            var res = ImageSource.GetAllImages();
            return res.image;
        }
        void UpdateListImages()
        {
            var res = GetImages();

            if (res.Length > 0)
            {
                Images.Clear();
                foreach (var item in res)
                {
                    string date = item.startdate.ToString();
                    int year = int.Parse(date.Substring(0, 4));
                    int month = int.Parse(date.Substring(4, 2));
                    int day = int.Parse(date.Substring(6, 2));


                    Images.Add(new ImageView()
                    {
                        FullPath = bingUrl + item.url,
                        Name = item.urlBase.Split("=", StringSplitOptions.RemoveEmptyEntries)[1],
                        CreationTime = new DateTime(year, month, day, 0, 0, 0)
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
            var image = ImageSource.DownloadImage(url);
            
            string fullpath = Path.Combine(pathSaveDirectory, _selectedImage.Name) + ".jpg";

            using var stream = new FileStream(fullpath, FileMode.Create, FileAccess.Write);
            stream.Write(image);            

            return fullpath;
        }

        public void Dispose()
        {
            ImageSource.Dispose();
        }
    }
}