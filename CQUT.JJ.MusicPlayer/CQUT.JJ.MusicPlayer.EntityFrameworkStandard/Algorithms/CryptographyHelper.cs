using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Algorithms
{
    public static class CryptographyHelper
    {
        #region MD5加密系统
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string EncryptByMD5(this string plaintext, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            var md5 = MD5.Create();
            var data = md5.ComputeHash(encoding.GetBytes(plaintext));
            var sb = new StringBuilder();
            foreach (var t in data)
                sb.Append(t.ToString("x2"));
            return sb.ToString();
        } 
        #endregion
    }
}
