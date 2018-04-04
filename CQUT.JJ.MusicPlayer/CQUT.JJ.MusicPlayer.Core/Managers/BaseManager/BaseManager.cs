using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class BaseManager<T> : IBaseManager<T> where T : class
    {
        private readonly JMDbContext _ctx;
        private readonly DbSet<T> _dbSet;

        public JMDbContext JMDbContext => _ctx;

        public BaseManager(JMDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<T>();
        }

        public virtual T Create(T obj)
        {
            obj = _dbSet.Add(obj).Entity;
            Save();
            return obj;
        }

        public virtual T Find(int id)
        {
            var obj = _dbSet.Find(id);
            if (obj == null)
                ThrowException("找不到对象！");
            return obj;
        }

        public virtual IQueryable<T> FindAll()
        {
            return _dbSet;
        }

        public int Remove(int id)
        {
            T obj = null;
            try
            {
                obj = Find(id);                
            }
            catch
            {
                ThrowException("对象不存在或已被移除!");
            }

            return Remove(obj);
        }

        public int Remove(T obj)
        {
            _dbSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return _ctx.SaveChanges();
        }

        public void ThrowException(string message,HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            throw new JMBasicException(message, httpStatusCode);
        }

        
    }
}
