using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace WallpapersBing.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        string bingUrl = @"https://www.bing.com";
        string pathSaveDirectory = Environment.CurrentDirectory;

        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, string pvParam, UInt32 fWinIni);
        const uint SPI_SETDESKWALLPAPER = 0x14;
        const uint SPIF_UPDATEINIFILE = 0x01;
        const uint SPIF_SENDWININICHANGE = 0x02;

        //SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, _selectedImage.FullPath, SPIF_SENDWININICHANGE | SPIF_UPDATEINIFILE);

        public MainWindowViewModel()
        {

            string url = bingUrl + @"/HPImageArchive.aspx?idx=0&n=10";

            Model.WebImageSource imageSource = new Model.WebImageSource(url);
            var res = imageSource.GetImages();

            foreach (var item in res.image)
            {
                Images.Add(new ImageView()
                    {
                    FullPath = bingUrl + item.url,
                    Name = item.urlBase.Split("=", StringSplitOptions.RemoveEmptyEntries)[1],
                    CreationTime = DateTime.Now
                });
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

                SetWallpaper(_selectedImage.FullPath);
            }
        }

        private string SaveImage(string url)
        {
            using var client = new HttpClient();
            var responce = client.GetAsync(url).GetAwaiter().GetResult();
            var content = responce.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            responce.Dispose();
            
            string fullpath = Path.Combine(pathSaveDirectory, _selectedImage.Name) + ".jpg";
            //System.Drawing.ImageConverter converter = new ImageConverter();
            //converter.ConvertFromString(content);

            FileStream stream = new FileStream(fullpath, FileMode.Create, FileAccess.Write);
            stream.Write(content);
            stream.Close();

            return fullpath;
        }



        private bool SetWallpaper(string path)
        {
            //await Task.Run(() => {
            bool res = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            //});
            return res;
        }
    }
}