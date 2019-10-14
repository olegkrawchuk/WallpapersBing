using System;
using System.Collections.Generic;
using System.Text;

namespace WallpapersBing.Model
{
    class ImageView : Base.ViewModelBase
    {

        String _fullPath;
        public string FullPath
        {
            get => _fullPath;
            set => SetProperty(ref _fullPath, value, nameof(FullPath));
        }

        String _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        DateTime _creationTime;
        public DateTime CreationTime
        {
            get => _creationTime;
            set => SetProperty(ref _creationTime, value, nameof(CreationTime));
        }
    }
}
