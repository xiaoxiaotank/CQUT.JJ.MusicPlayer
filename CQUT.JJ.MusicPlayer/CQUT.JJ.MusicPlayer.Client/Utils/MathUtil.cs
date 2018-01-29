using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class MathUtil
    {
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static double GetMin(params double[] number)
        {
            var min = number[0];
            for (int i = 1; i < number.Length; i++)
            {
                if (min > number[i])
                    min = number[i];
            }
            return min;
        }
    }
}
