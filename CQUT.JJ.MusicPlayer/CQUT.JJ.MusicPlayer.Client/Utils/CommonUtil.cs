using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class CommonUtil
    {
        /// <summary>
        /// 将ImageUri转为ImageBrush
        /// </summary>
        /// <param name="imageUri"></param>
        /// <returns></returns>
        public static ImageBrush ToImageBrush(this Uri imageUri)
        {
            try
            {
                return new ImageBrush()
                {
                    ImageSource = new BitmapImage(imageUri),
                    Stretch = Stretch.UniformToFill
                };
            }
            catch
            {
                return null;
            }
           
        }

        /// <summary>
        /// 将字符串路径转为图像路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="uriKind"></param>
        /// <returns></returns>
        public static ImageSource ToImageSource(this string path,UriKind uriKind = UriKind.Relative)
        {
            return new BitmapImage(new Uri(path, uriKind));
        }

        /// <summary>
        /// 获取特定对象的特定属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object obj,string propertyName)
        {
            return  (T)obj.GetType().GetProperty(propertyName).GetValue(obj);
        }

        /// <summary>  
        /// 获得指定元素的父元素  
        /// </summary>  
        /// <typeparam name="T">指定页面元素</typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static T GetParentObject<T>(this DependencyObject obj) where T : FrameworkElement
        {
            var parentObj = VisualTreeHelper.GetParent(obj);

            while (parentObj != null)
            {
                if (parentObj is T parent)
                {
                    return parent;
                }

                parentObj = VisualTreeHelper.GetParent(parentObj);
            }

            return null;
        }

        /// <summary>  
        /// 获得指定元素的所有子元素  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static IEnumerable<T> GetAllChildObject<T>(this DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject childObj = null;
            var childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                childObj = VisualTreeHelper.GetChild(obj, i);

                if (childObj is T child)
                {
                    childList.Add(child);
                }
                childList.AddRange(GetAllChildObject<T>(childObj));
            }
            return childList;
        }

        /// <summary>  
        /// 查找子元素  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <param name="name"></param>  
        /// <returns></returns>  
        public static T GetChildObjectByName<T>(this DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject childObj = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                childObj = VisualTreeHelper.GetChild(obj, i);


                if (childObj is T child && (child.Name == name | string.IsNullOrEmpty(name)))
                {
                    return child;
                }
                else
                {
                    grandChild = GetChildObjectByName<T>(childObj, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取第一个特定类型的子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject childObj = VisualTreeHelper.GetChild(obj, i);
                if (childObj is T child)
                    return child;
                else
                {
                    T grandChild = FindVisualChild<T>(childObj);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

    }
}
