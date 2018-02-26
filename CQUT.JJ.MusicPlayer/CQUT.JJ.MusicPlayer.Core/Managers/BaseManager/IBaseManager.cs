using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public interface IBaseManager<T> where T : class
    {
        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id">数据id</param>
        /// <returns>数据</returns>
        T Find(int id);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns>数据</returns>
        IQueryable<T> FindAll();

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="obj">需要创建的对象</param>
        /// <returns>创建后的对象</returns>
        T Create(T obj);

        /// <summary>
        /// 根据id移除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns>影响行数</returns>
        int Remove(int id);

        /// <summary>
        /// 直接移除对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        int Remove(T obj);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        int Save();

        /// <summary>
        /// 抛异常
        /// </summary>
        /// <param name="message"></param>
        void ThrowException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest);
    }
}
