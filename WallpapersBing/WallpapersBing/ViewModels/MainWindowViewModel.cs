using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Base;

namespace WallpapersBing.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Images.Add(new ImageView()
            {
                FullPath = @"https://www.bing.com/th?id=OHR.AlbertaThanksgiving_ROW3027926486_1920x1080.jpg&rf=LaDigue_1920x1080.jpg&pid=hp",
                Name = "SDC12344.JPG",
                CreationTime = DateTime.Now
            });

            Images.Add(new ImageView()
            {
                FullPath = @"F:\Фото\1 Мая 2010г\SDC12345.JPG",
                Name = "SDC12345.JPG",
                CreationTime = DateTime.Now.AddDays(-1)
            }) ;

            using var client = new HttpClient();
            string xml = client.GetStringAsync(@"https://www.bing.com/HPImageArchive.aspx?idx=0&n=10").GetAwaiter().GetResult();
            Model.XmlDeserializer<Model.Image> deserializer = new Model.XmlDeserializer<Model.Image>(xml, "images");
            var res = deserializer.Deserialize();
            var list = res as List<Model.Image>;

        }

        public ObservableCollection<ImageView> Images { get; } = new ObservableCollection<ImageView>();


        ImageView _selectedImage = null;
        public ImageView SelectedImage 
        {
            get => _selectedImage;
            set => SetProperty(ref _selectedImage, value, nameof(SelectedImage));
        }
    }
}
