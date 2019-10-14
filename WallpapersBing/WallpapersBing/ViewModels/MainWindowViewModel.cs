using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
            using var client = new HttpClient();
            string xml = client.GetStringAsync(bingUrl + @"/HPImageArchive.aspx?idx=0&n=10").GetAwaiter().GetResult();
            using System.IO.StringReader stringReader = new System.IO.StringReader(xml);

            XmlSerializer deserializer = new XmlSerializer(typeof(Model.images), new XmlRootAttribute("images"));
            var res = deserializer.Deserialize(stringReader) as Model.images;

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
                    var path = SaveImage(_selectedImage.FullPath).GetAwaiter().GetResult();
                    _selectedImage.FullPath = path;
                }

                SetWallpaper(_selectedImage.FullPath).GetAwaiter().GetResult();                                
            }
        }

        private async Task<string> SaveImage(string url)
        {
            using var client = new HttpClient();
            using var responce = await client.GetAsync(url);            
            var content = await responce.Content.ReadAsStringAsync();

            string fullpath = Path.Combine(pathSaveDirectory, _selectedImage.Name) + ".jpg";

            using var stream = new StreamWriter(fullpath);
            await stream.WriteAsync(content);
            await stream.DisposeAsync();

            return fullpath;
        }



        private async Task<bool> SetWallpaper(string path)
        {
            bool res = false;
            await Task.Run(() => {
                res = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);                
            });
            return res;
        }
    }
}