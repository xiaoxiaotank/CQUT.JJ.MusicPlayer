using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels.UserHeader
{
    public class UserHeaderViewModel : INotifyPropertyChanged
    {
        private string _nickName;
        private ImageSource _profilePhotoPath;

        public string NickName
        {
            get => _nickName;
            set
            {
                _nickName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NickName)));
            }
        }

        public ImageSource ProfilePhotoPath
        {
            get => _profilePhotoPath;
            set
            {
                _profilePhotoPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProfilePhotoPath)));
            }
        }

        public int Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
