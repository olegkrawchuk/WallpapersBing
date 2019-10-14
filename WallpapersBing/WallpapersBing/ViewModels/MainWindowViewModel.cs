using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Base;
using WallpapersBing.Model;

namespace WallpapersBing.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Images.Add(new ImageView()
            {
                FullPath = @"F:\Фото\1 Мая 2010г\SDC12344.JPG",
                Name = "SDC12344.JPG",
                CreationTime = DateTime.Now
            });
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
