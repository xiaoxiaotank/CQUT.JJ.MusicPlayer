using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels.Common
{
    public class SingerViewModel : INotifyPropertyChanged
    {
        private DateTime _birthday;

        private int _fansCount;

        public int Id { get; set; }

        public string ProfilePhotoPath { get; set; }

        public string Name { get; set; }

        public string ForeignerName { get; set; }

        public string Nationality { get; set; }

        /// <summary>
        /// 代表作
        /// </summary>
        public string MagnumOpusName { get; set; }

        public string Birthday
        {
            get => _birthday.ToString("yyyy-MM-dd");
            set
            {
                _birthday = DateTime.Parse(value);
            }
        }

        public int FansCount
        {
            get => _fansCount;
            set
            {
                _fansCount = value;
                FansCountCn = $"{value}人";
            }
        }

        /// <summary>
        /// 中文标识
        /// </summary>
        public string FansCountCn { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
