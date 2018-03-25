using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.IO;
using System.Linq;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System.Runtime.InteropServices;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.APIs;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class MusicManager : BaseManager<Music>
    {
        public MusicManager(JMDbContext ctx) : base(ctx)
        {
            
        }

        //TODO  增加操作人
        public Music Create(MusicModel model)
        {
            ValidateForCreate(model);

            var music = new Music()
            {
                SingerId = model.SingerId,
                AlbumId = model.AlbumId,
                Name = model.Name,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                FileUrl = model.FileUrl,
                Duration = GetMusicDuration(model.FileUrl)
            };

            return Create(music);
        }


        public Music UpdateBasic(MusicModel model)
        {
            ValidateForUpdateBasic(model,out Music music);

            music.Name = model.Name;
            music.SingerId = model.SingerId;
            music.AlbumId = model.AlbumId;
            music.LastModificationTime = DateTime.Now;

            return music;

        }

        public void Delete(int id)
        {
            ValidForDelete(id, out Music music);
            
            music.IsDeleted = false;
            music.LastModificationTime
                = music.DeletionTime
                = DateTime.Now;
            Save();
        }

        public override Music Find(int id)
        {
            var music = JMDbContext.Music.Include(m => new { m.Singer, m.Album })
                .SingleOrDefault(mu => mu.Id == id && !mu.IsDeleted);
            if (music == null)
                ThrowException("音乐信息不存在！");
            return music;
        }

        private TimeSpan GetMusicDuration(string fileName)
        {
            var durationString = APIClass.GetMusicDurationString(fileName);
            if(int.TryParse(durationString,out int durationInt))
                return TimeSpan.FromMilliseconds(durationInt);
            throw new JMBasicException("音乐文件无效!");
        }


        private void ValidateForCreate(MusicModel model)
        {
            var singer = JMDbContext.Singer.SingleOrDefault(s => s.Id == model.SingerId && !s.IsDeleted && s.IsPublished);
            if (singer == null)
                ThrowException("歌唱家未发布或不存在！");
            var album = JMDbContext.Album.SingleOrDefault(a => a.Id == model.AlbumId && !a.IsDeleted && a.IsPublished);
            if (album == null)
                ThrowException("专辑未发布或不存在");
            if (album.SingerId != singer.Id)
                ThrowException("音乐家与专辑信息不匹配");
            if (string.IsNullOrWhiteSpace(model.Name))
                ThrowException("音乐名不能为空！");
            if (model.Name.Length > 32)
                ThrowException("音乐名不能超过32个字符！");
            if (!File.Exists(model.FileUrl))
                ThrowException("音乐文件不存在");
            if (!model.FileUrl.IsEffectiveMusicFile())
                ThrowException("文件格式无效");
        }

        private void ValidateForUpdateBasic(MusicModel model,out Music music)
        {
            ValidForDelete(model.Id, out music);
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
        }


        private void ValidForDelete(int id, out Music music)
        {
            music = Find(id);
            if (music.IsPublished)
                ThrowException("音乐作品已发布，请撤销后进行该操作");
        }

    }
}
