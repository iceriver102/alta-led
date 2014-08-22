using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Alta_LED.Data.Init
{

    public static class StaticFunction
    {
        public static void ScaleElement(this UIElement E, Size scale)
        {
            E.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform s = new ScaleTransform(scale.Width, scale.Height);
            E.RenderTransform = s;
        }
        public static void ScaleLayout(this Border E, Double scaleX, Double scaleY)
        {
            E.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform s = new ScaleTransform(scaleX, scaleY);
            E.LayoutTransform = s;
        }
        public static bool isNumber(this String str)
        {
            if (str == "" || str == String.Empty)
                return false;
            Regex regex = new Regex("^-?\\d*(\\.\\d+)?$");
            return regex.IsMatch(str);
        }
        public static bool isNumberGreaterZero(this String str)
        {
            if (str == "" || str == String.Empty)
                return false;
            Regex regex = new Regex("^?\\d*(\\.\\d+)?$");
            return regex.IsMatch(str);
        }
        public static bool IsKeyADigit(this System.Windows.Forms.Keys key)
        {
            return (key >= System.Windows.Forms.Keys.D0 && key <= System.Windows.Forms.Keys.D9) || (key >= System.Windows.Forms.Keys.NumPad0 && key <= System.Windows.Forms.Keys.NumPad9);
        }
        public static void WriteFile(String data, String FileName)
        {
            StreamWriter wr = new StreamWriter(FileName);
            wr.WriteLine(data);           
            wr.Close();
        }
        static Random rand = new Random();
        public static Point getPosition(this UIElement e)
        {
            return new Point() { X = Canvas.GetLeft(e), Y = Canvas.GetTop(e) };
        }
        public static void setPosition(this UIElement layout,double x, double y)
        {
            Canvas.SetTop(layout, y);
            Canvas.SetLeft(layout, x);
        }
        public static void setPosition(this UIElement layout, Point pos)
        {
            Canvas.SetLeft(layout, pos.X);
            Canvas.SetTop(layout, pos.Y);
        }
        public static void setPosition(this UIElement layout, Vector v)
        {
            Canvas.SetLeft(layout, v.X);
            Canvas.SetTop(layout, v.Y);
        }
        public static void aniMoveTo(this UIElement target, double newX, double newY, double timeDuration, IEasingFunction iEasingFunction, Action CompleteFunction=null)
        {
            var top = Canvas.GetTop(target);
            if (top.ToString().Contains("NaN")) { top = 0; }
            var left = Canvas.GetLeft(target);
            if (left.ToString().Contains("NaN")) { left = 0; }
            TranslateTransform trans = new TranslateTransform();
            target.RenderTransform = trans;
            var tmpNewX = newX - left;
            var tmpNewY = newY - top;
            if(iEasingFunction == null) iEasingFunction = new PowerEase  { EasingMode = EasingMode.EaseInOut };
            DoubleAnimation anim1 = new DoubleAnimation(0, newY - top, TimeSpan.FromSeconds(timeDuration)) { EasingFunction = iEasingFunction };
            DoubleAnimation anim2 = new DoubleAnimation(0, newX - left, TimeSpan.FromSeconds(timeDuration)) { EasingFunction = iEasingFunction };
            anim1.FillBehavior = FillBehavior.HoldEnd;
            anim2.FillBehavior = FillBehavior.HoldEnd;
            anim1.Completed += (s, a) =>
            {
                target.SetValue(Canvas.TopProperty, newY);
                trans.BeginAnimation(TranslateTransform.YProperty, null);
            };
            anim2.Completed += (s, a) =>
            {
                target.SetValue(Canvas.LeftProperty, newX);
                trans.BeginAnimation(TranslateTransform.XProperty, null);
                if (CompleteFunction != null)
                {
                    CompleteFunction();
                }
            };
            trans.BeginAnimation(TranslateTransform.YProperty, anim1);
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }

        public static void aniChangeOpacity(this UIElement target, double to, double timeDuration)
        {
            DoubleAnimation anim = new DoubleAnimation((double)target.Opacity, to, new Duration(TimeSpan.FromSeconds(timeDuration)));
            anim.Completed += (s, a) =>
            {
                target.Opacity = to;
                target.BeginAnimation(UIElement.OpacityProperty, null);
            };
            target.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        public static void aniScaleImage(this Image img, double renderTranformOriginX, double renderTranformOriginY, double scaleToX, double scaleToY, double timeDuration, IEasingFunction iEasingFunction)
        {
            var width = img.Width;
            var height = img.Height;

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            img.RenderTransformOrigin = new Point(renderTranformOriginX, renderTranformOriginY);
            img.RenderTransform = scale;

            if (iEasingFunction == null) iEasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };
            DoubleAnimation anim1 = new DoubleAnimation(1, scaleToX, TimeSpan.FromSeconds(timeDuration)) { EasingFunction = iEasingFunction };
            DoubleAnimation anim2 = new DoubleAnimation(1, scaleToY, TimeSpan.FromSeconds(timeDuration)) { EasingFunction = iEasingFunction };
            anim1.FillBehavior = FillBehavior.HoldEnd;
            anim2.FillBehavior = FillBehavior.HoldEnd;
            anim1.Completed += (s, a) =>
            {
                img.Width = width * scaleToX;
                scale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            };
            anim2.Completed += (s, a) =>
            {
                img.Height = height * scaleToY;
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim1);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim2);
        }

        public static void aniScaleCanvas(this Canvas canvas, double renderTranformOriginX, double renderTranformOriginY, double scaleToX, double scaleToY, double timeDuration, IEasingFunction iEasingFunction, bool save = false)
        {
            var width = canvas.Width;
            var height = canvas.Height;

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            canvas.RenderTransformOrigin = new Point(0.5, 0.5);
            canvas.RenderTransform = scale;

            if (iEasingFunction == null) iEasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };
            DoubleAnimation anim1 = new DoubleAnimation(1, scaleToX, TimeSpan.FromSeconds(timeDuration)) { EasingFunction = iEasingFunction };
            DoubleAnimation anim2 = new DoubleAnimation(1, scaleToY, TimeSpan.FromSeconds(timeDuration)) { EasingFunction = iEasingFunction };

            anim1.Completed += (s, a) =>
            {
                scale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                if (save)
                    canvas.Width = width * scaleToX;
            };
            anim2.Completed += (s, a) =>
            {
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                if (save)
                    canvas.Height = height * scaleToY;
            };
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim1);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim2);
        }

        public static void aniScaleUserControl(this UserControl userControl, double renderTranformOriginX, double renderTranformOriginY, double scaleToX, double scaleToY, double timeDuration, IEasingFunction iEasingFunction, bool save = false)
        {
            var width = userControl.Width;
            var height = userControl.Height;

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            userControl.RenderTransformOrigin = new Point(0.5, 0.5);
            userControl.RenderTransform = scale;

            if (iEasingFunction == null) iEasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };
            DoubleAnimation anim1 = new DoubleAnimation(1, scaleToX, TimeSpan.FromSeconds(timeDuration)) { EasingFunction = iEasingFunction };
            DoubleAnimation anim2 = new DoubleAnimation(1, scaleToY, TimeSpan.FromSeconds(timeDuration)) { EasingFunction = iEasingFunction };

            anim1.Completed += (s, a) =>
            {
                if (save)
                    userControl.Width = width * scaleToX;
                scale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            };
            anim2.Completed += (s, a) =>
            {
                if (save)
                    userControl.Height = height * scaleToY;
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            };
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim1);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim2);
        }

        //Phương thức đếm số file trong folder
        public static int countFilesInDirectory(string folderName, bool typeCount, string rootPath = @"Resources\Images\")
        {
            try
            {
                int result = 0;
                string path = rootPath + folderName;
                if (typeCount)
                {
                    result = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly).Length; //File thuộc folder hiện hành
                }
                else
                {
                    result = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Length; // File thuộc tất cả các folder bao gồm cả folder con
                }
                return result == 0 ? 1 : result;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        //Phương thức lấy Uri cho video
        public static Uri getVideoSource(string folderName, bool randomVideo = true, int position = 1, string rootPath = @"Resources\Videos\", bool typeCount = true)
        {
            try
            {
                rootPath = rootPath == "" ? @"Resources\Videos\" : rootPath;

                int count = countFilesInDirectory(folderName, typeCount, rootPath);
                int pos = 1;
                if (randomVideo)
                {
                    pos = rand.Next(1, count);
                }
                else
                {
                    pos = position;
                }
                string path = "";
                path = rootPath + folderName + @"\" + folderName + pos + ".avi";
                if(File.Exists(path))
                    return new Uri(path, UriKind.Relative);
                path = rootPath + folderName + @"\" + folderName + pos + ".mp4";
                if (File.Exists(path))
                    return new Uri(path, UriKind.Relative);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Phương thức lấy ImageSource
        public static ImageSource getImageSource(string folderName, bool randomImage = true, int position = 1, string rootPath = @"Resources\Images\", bool typeCount = true)
        {
            try
            {
                rootPath = rootPath == "" ? @"Resources\Images\" : rootPath;
                int count = countFilesInDirectory(folderName, typeCount, rootPath);
                int pos = 1;
                if (randomImage)
                {
                    pos = rand.Next(1, count);
                }
                else
                {
                    pos = position;
                }
                ImageSourceConverter s = new ImageSourceConverter();
                string path = "";
                path = rootPath + folderName + @"\" + folderName + pos + ".jpg";
                if (File.Exists(path))
                    return (ImageSource)s.ConvertFromString(path);
                path = rootPath + folderName + @"\" + folderName + pos + ".png";
                if (File.Exists(path))
                    return (ImageSource)s.ConvertFromString(path);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static Uri convertStringToUri(string folderName, bool randomImage = true, int position = 1, string rootPath = @"Resources\Images\", bool typeCount = true)
        {
            try
            {
                rootPath = rootPath == "" ? @"Resources\Images\" : rootPath;

                int count = countFilesInDirectory(folderName, typeCount, rootPath);
                int pos = 1;
                if (randomImage)
                {
                    pos = rand.Next(1, count);
                }
                else
                {
                    pos = position;
                }
                
                string path = "";
                path = rootPath + folderName + @"\" + folderName + pos + ".jpg";
                if (File.Exists(path))
                    return new Uri(path, UriKind.Relative);
                path = rootPath + folderName + @"\" + folderName + pos + ".png";
                if (File.Exists(path))
                    return new Uri(path, UriKind.Relative);

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ImageSource getImageSourceDirectly(string url)
        {
            try
            {
                ImageSourceConverter s = new ImageSourceConverter();
                ImageSource result = (ImageSource)s.ConvertFromString(url);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Phương thức convert ImageSource thành ImageBrush
        public static ImageBrush convertImageToImageBrush(ImageSource imageSource)
        {
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = imageSource;
            return ib;
        }

        //Phương thức lấy uriSound
        public static Uri getUriSound(string folderName, bool typeCount, int typeSound, string rootPath = @"Resources\Sounds\")
        {
            try
            {
                int count = countFilesInDirectory(folderName, true, rootPath);
                int pos = rand.Next(1, count);
                if (typeSound != 0)
                {
                    pos = typeSound;
                }
                string path = rootPath + folderName + @"\" + folderName + pos + ".mp3";
                if (File.Exists(path))
                    return new Uri(path, UriKind.Relative);
                path = rootPath + folderName + @"\" + folderName + pos + ".wav";
                if (File.Exists(path))
                    return new Uri(path, UriKind.Relative);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Create an attached property named `GroupID`
        public static class UIElementExtensions
        {
            public static Int32 GetGroupID(DependencyObject obj)
            {
                return (Int32)obj.GetValue(GroupIDProperty);
            }

            public static void SetGroupID(DependencyObject obj, Int32 value)
            {
                obj.SetValue(GroupIDProperty, value);
            }

            // Using a DependencyProperty as the backing store for GroupID.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty GroupIDProperty =
                DependencyProperty.RegisterAttached("GroupID", typeof(Int32), typeof(UIElementExtensions), new UIPropertyMetadata(null));
        }

        //Open Window in second monitor
        public static void MaximizeToSecondaryMonitor(Window window)
        {
            var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null)
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                window.Left = workingArea.Left;
                window.Top = workingArea.Top;
                window.Width = workingArea.Width;
                window.Height = workingArea.Height;
                if (window.IsLoaded)
                    window.WindowState = WindowState.Maximized;
            }
            else
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                window.Left = 0;
                window.Top = 0;
                window.Width = 1366;
                window.Height = 768;
                if (window.IsLoaded)
                    window.WindowState = WindowState.Maximized;
            }
        }

        public static void MaximizeToOtherMonitor(Window window, int idScreen)
        {
            var otherScreen = System.Windows.Forms.Screen.AllScreens[idScreen];

            if (otherScreen != null)
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = otherScreen.WorkingArea;
                window.Left = workingArea.Width * idScreen;
                window.Top = 0;
                window.Width = workingArea.Width;
                window.Height = workingArea.Height;
                //if (window.IsLoaded)
                //    window.WindowState = WindowState.Maximized;
            }
            else
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                window.Left = 0;
                window.Top = 0;
                window.Width = 1366;
                window.Height = 768;
                if (window.IsLoaded)
                    window.WindowState = WindowState.Maximized;
            }
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        //Phương thức chuyển từ chuỗi có dấu thành chuỗi không dấu
        public static string ConvertToUnSign(string text)
        {
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            text = text.Replace(" ", " ");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Trim();
        }

        //Phương thức lấy size của textblock
        public static Size MeasureString(TextBlock tblock, string candidate)
        {
            var formattedText = new FormattedText(
                candidate,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(tblock.FontFamily, tblock.FontStyle, tblock.FontWeight, tblock.FontStretch), tblock.FontSize, Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        #region Input Output

        public static bool checkExistFile(string fullPath)
        {
            return File.Exists(fullPath);
        }

        public static bool writeFile(string fullPath, string[] content)
        {
            try
            {
                int iLastIndex = fullPath.LastIndexOf('\\');
                string sFileName = fullPath.Substring(iLastIndex + 1, (fullPath.Length - iLastIndex - 1));
                string path = fullPath.Substring(0, iLastIndex + 1);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                BinaryWriter w = new BinaryWriter(fs);

                foreach (string tmp in content)
                {
                    w.Write(tmp);
                }

                w.Close();
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string[] readFile(string fullPath)
        {
            try
            {
                int iLastIndex = fullPath.LastIndexOf('\\');
                string sFileName = fullPath.Substring(iLastIndex + 1, (fullPath.Length - iLastIndex - 1));
                string path = fullPath.Substring(0, iLastIndex + 1);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);

                int pos = 0;
                int length = (int)r.BaseStream.Length / 2;
                string[] result = new string[length];
                while (r.PeekChar() != -1)
                {
                    string v = r.ReadString();
                    result[pos] = v;
                    pos++;
                }

                r.Close();
                fs.Close();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Input Output

        #region Convert Image Byte Array



        #endregion Convert Image Byte Array

        #region Play Image PNG

        static string rootPath = @"Resources\Images\";

        public static void playImagePNG(this Image img, string folder, int duration = 100, bool repeat = false, bool holdLastFrame = false)
        {
            int indexPlayImage = 0;
            img.Source = getImageSource(folder, false, 0);
            //folder = rootPath + folder;
            int countFiles = countFilesInDirectory(folder, true);
            mtPlayImage(img, indexPlayImage, folder, countFiles, duration, repeat, holdLastFrame);
        }
        
        //Default: Duration = 100
        public static void myThreadPlayImage(this Image img, Thread threadPlayImage, int indexPlayImage, string folder, int countFiles, int duration, bool repeat, bool holdLastFrame)
        {
            try
            {
                while (true)
                {
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new Action<Image, Thread, int, string, int, bool, bool>(handleThreadPlayImage), img, threadPlayImage, indexPlayImage, folder, countFiles, repeat, holdLastFrame);
                    }
                    Thread.Sleep(duration);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void handleThreadPlayImage(this Image img, Thread threadPlayImage, int indexPlayImage, string folder, int countFiles, bool repeat, bool holdLastFrame)
        {
            try
            {
                if (img.Source != null)
                    indexPlayImage = getIndexImageFromUrl(img.Source.ToString(), folder) + 1;
                string path = rootPath + folder + @"\" + folder + indexPlayImage + ".png";
                ImageSourceConverter s;
                ImageSource imgSource;
                if (indexPlayImage >= countFiles)
                {
                    if (!repeat)
                    {
                        threadPlayImage.Abort();
                        if (holdLastFrame)
                        {
                            path = rootPath + folder + @"\" + folder + indexPlayImage + ".png";
                            s = new ImageSourceConverter();
                            imgSource = (ImageSource)s.ConvertFromString(path);
                            img.Source = imgSource;
                        }
                        else
                        {
                            img.Source = null;
                        }
                        return;
                    }
                    indexPlayImage = 0;
                }
                path = rootPath + folder + @"\" + folder + indexPlayImage + ".png";
                s = new ImageSourceConverter();
                imgSource = (ImageSource)s.ConvertFromString(path);
                img.Source = imgSource;
            }
            catch (Exception)
            {
            }
        }

        public static void mtPlayImage(this Image img, int indexPlayImage, string folder, int countFiles, int duration, bool repeat, bool holdLastFrame)
        {
            try
            {
                Thread threadPlayImage = null;
                threadPlayImage = new Thread(() => myThreadPlayImage(img, threadPlayImage, indexPlayImage, folder, countFiles, duration, repeat, holdLastFrame));
                threadPlayImage.SetApartmentState(ApartmentState.STA);
                threadPlayImage.Start();
            }
            catch (Exception)
            {
            }
        }

        private static int getIndexImageFromUrl(string url, string folder)
        {
            int iLastIndex = url.LastIndexOf(folder);
            string indexFiles = url.Substring(iLastIndex + folder.Length, (url.Length - iLastIndex - folder.Length - 4));
            return int.Parse(indexFiles);
        }



        #endregion Play Image PNG
        //Function convert number string to string with split character
        public static string convertMoneyString(string convertString, string lastString = "VND", string charSplit = ".")
        {
            string result = " " + lastString;
            int index = 1;
            int len = convertString.Length, saveLen = convertString.Length;
            while (len > 3)
            {
                result = charSplit + convertString.Substring(saveLen - 3 * index, 3) + result;
                index++;
                len -= 3;
            }
            result = convertString.Substring(0, len) + result;
            return result;
        }
        
    }
}
