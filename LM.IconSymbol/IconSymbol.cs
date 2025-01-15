using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection;

namespace LM.Icon
{
    /// <summary>
    /// To use the IconSymbol,you can set the following properies
    /// Foreground set the foreground color of Icon
    /// Background set the background color of Icon
    /// BorderThickness set the icon strokethickness
    /// Icon set the icon type
    /// When Adding new Icons,please add IconData also the IconType,make them the same name
    /// </summary>
    public class IconSymbol : ContentControl
    {
        private Path _path;
        public IconSymbol()
        {
            _path = new Path();
            _path.Stretch = System.Windows.Media.Stretch.Uniform;
            _path.Width = this.Width;
            _path.Height = this.Height;
            _path.Stroke = Foreground;
            _path.Fill = Foreground;

            _path.StrokeThickness = BorderThickness.Left;
            Border border = new Border();
            border.Child = _path;
            this.Content = border;
            Binding strokeBinding = new Binding();
            strokeBinding.Source = this;
            strokeBinding.Path = new PropertyPath("Foreground");
            BindingOperations.SetBinding(_path, Path.StrokeProperty, strokeBinding);
            BindingOperations.SetBinding(_path, Path.FillProperty, strokeBinding);

            Binding backBinding = new Binding();
            backBinding.Source = this;
            backBinding.Path = new PropertyPath("Background");
            BindingOperations.SetBinding(border, Border.BackgroundProperty, backBinding);
            
            Binding strokeThicknessBinding=new Binding();
            strokeThicknessBinding.Source = this;
            strokeThicknessBinding.Path = new PropertyPath("BorderThickness.Left");
            BindingOperations.SetBinding(_path,Path.StrokeThicknessProperty, strokeThicknessBinding);
            
            this.Background = Brushes.Transparent;

            this.PreviewMouseLeftButtonUp += IconSymbol_PreviewMouseLeftButtonUp;
        }

        private void IconSymbol_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsChecked = !IsChecked;
        }

        public IconType Icon
        {
            get { return (IconType)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IconType), typeof(IconSymbol), new PropertyMetadata(default(IconType), (d, e) => {
                var control = d as IconSymbol;
                if (control != null && e.NewValue != null)
                {
                    var filed = typeof(IconPathData).GetField(e.NewValue.ToString(),
                        BindingFlags.GetField|BindingFlags.IgnoreCase|BindingFlags.Public|BindingFlags.Static);
                    if (filed != null)
                    {
                        var pathStrData = filed.GetValue(null);
                        if (pathStrData != null)
                        {
                            var pathData = PathGeometry.Parse(pathStrData.ToString());
                            if (pathData != null)
                            {
                                control._path.Data = pathData;
                            }
                        }
                    }
                }
            }));



        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(IconSymbol), new PropertyMetadata(false));


    }
}
