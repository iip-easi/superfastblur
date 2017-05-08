using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Image = System.Drawing.Image;

namespace Ivan
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _image = Application.GetResourceStream(new Uri((string)"pack://application:,,,/2017年5月8日 104418.png"))?
                        .Stream;
        }

        private Stream _image;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            GaussianBlurImage();
        }

        private void GaussianBlurImage()
        {
            Image image = Image.FromStream(_image);
            GaussianBlur blur = new GaussianBlur(image as Bitmap);
            using (MemoryStream memory = new MemoryStream())
            {
                blur.Process(5).Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                Timage.Source = bitmapImage;
            }
            //http://stackoverflow.com/a/1069509/6116637
        }

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            //选取文件
            OpenFileDialog open = new OpenFileDialog();//定义打开文本框实体
            open.Title = "打开文件";//对话框标题
            open.Filter = "文件（.jpg）|*.*";
            if (open.ShowDialog() == true)
            {
                _image.Close();
                _image = File.OpenRead(open.FileName); 
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = _image;
                img.EndInit();
                Simage.Source = img;
                GaussianBlurImage();
            }

        }
    }
}
