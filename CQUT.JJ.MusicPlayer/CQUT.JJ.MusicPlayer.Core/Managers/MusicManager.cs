using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.IO;
using System.Linq;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System.Runtime.InteropServices;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class MusicManager : BaseManager<Music>
    {
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public MusicManager(JMDbContext ctx) : base(ctx)
        {
            
        }

        public Music Create(MusicModel model)
        {
            ValidateForCreate(model);

            var music = new Music()
            {
                SingerId = model.SingerId,
                AlbumId = model.AlbumId,
                Name = model.Name,
                CreationTime = model.CreationTime,
                IsDeleted = false,
                Duration = GetMusicDuration(model.FileUrl)
            };

            return Create(music);
        }

        private TimeSpan GetMusicDuration(string fileName)
        {
            var durLength = string.Empty.PadLeft(128, ' ');
            mciSendString(string.Format($"open {fileName}"), durLength, durLength.Length, 0);
            if(int.TryParse(durLength.Trim(),out int second))
                return TimeSpan.FromSeconds(second);
            throw new JMBasicException("音乐文件无效");
        }

        public Music Update(MusicModel model)
        {
            ValidateForUpdate(model);

            var music = JMDbContext.Music.SingleOrDefault(m => m.Id == model.Id);
            return music;

        }

        public void Delete(int id)
        {
            var music = JMDbContext.Music.SingleOrDefault(m => m.Id == id && !m.IsDeleted);
            if (music != null)
            {
                music.IsDeleted = false;
                music.LastModificationTime
                    = music.DeletionTime
                    = DateTime.Now;
                Save();
            }
        }

    
        private void ValidateForCreate(MusicModel model)
        {
            var singer = JMDbContext.Singer.SingleOrDefault(s => s.Id == model.SingerId && !s.IsDeleted);
            if (singer == null)
                ThrowException("歌唱家不存在！");
            var album = JMDbContext.Album.SingleOrDefault(a => a.Id == model.AlbumId && !a.IsDeleted);
            if (album == null)
                ThrowException("专辑不存在");
            if (string.IsNullOrWhiteSpace(model.Name))
                ThrowException("音乐名不能为空！");
            if (model.Name.Length > 32)
                ThrowException("音乐名不能超过32个字符！");
            if (!File.Exists(model.FileUrl))
                ThrowException("音乐文件不存在");
            if (!GlobalConstants.Music_File_Suffix.Contains(Path.GetExtension(model.FileUrl)))
                ThrowException("文件格式无效");
        }

        private void ValidateForUpdate(MusicModel model)
        {
            throw new NotImplementedException();
        }

    }
}
