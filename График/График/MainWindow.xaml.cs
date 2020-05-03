using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using System.Globalization;

namespace График
{
    public partial class MainWindow : Window
    {
        //
        // Поля
        //
       
        // Список для хранения данных
        List<double> dataList = new List<double>();
        // Контейнер слоев рисунков
        DrawingGroup drawingGroup = new DrawingGroup();

        public MainWindow()
        {
            InitializeComponent();

            
            Execute(); // Заполнение слоев

            // Отображение на экране
            image1.Source = new DrawingImage(drawingGroup);
        }

     

        // Послойное формирование рисунка в Z-последовательности
        void Execute()
        {
            BackgroundFun();    // Фон
            GridFun();          // Мелкая сетка
            GraficF();           // Строим график
            MarkerFun();        // Надписи
        }

        // Фон
        private void BackgroundFun()
        {
            // Создаем объект для описания геометрической фигуры
            GeometryDrawing geometryDrawing = new GeometryDrawing();

            // Описываем и сохраняем геометрию квадрата
            RectangleGeometry rectGeometry = new RectangleGeometry();
            rectGeometry.Rect = new Rect(0.1, -20, 30, 30);
            geometryDrawing.Geometry = rectGeometry;

            // Настраиваем перо и кисть
            geometryDrawing.Pen = new Pen(Brushes.Red, 0.05);// Перо рамки
            geometryDrawing.Brush = Brushes.Beige;// Кисть закраски

            // Добавляем готовый слой в контейнер отображения
            drawingGroup.Children.Add(geometryDrawing);
        }

        // Горизонтальная сетка
        private void GridFun()
        {
            // Создаем коллекцию для описания геометрических фигур
            GeometryGroup geometryGroup = new GeometryGroup();

            // Создаем и добавляем в коллекцию десять параллельных линий 
            for (double i = -5; i < 10; i++)
            {
                LineGeometry line = new LineGeometry(new Point(0.1,-i * 2),
                    new Point(30.1, -i * 2));
                geometryGroup.Children.Add(line);
            }

            // Сохраняем описание геометрии
            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = geometryGroup;

            // Настраиваем перо
            geometryDrawing.Pen = new Pen(Brushes.Gray, 0.03);
            double[] dashes = { 1, 1, 1, 1, 1 };// Образец штриха
            geometryDrawing.Pen.DashStyle = new DashStyle(dashes, -.1);

            // Настраиваем кисть 
            geometryDrawing.Brush = Brushes.Beige;

            // Добавляем готовый слой в контейнер отображения
            drawingGroup.Children.Add(geometryDrawing);
        }

        // Строим график
        private void GraficF()

        {
            int c = 0;
            string lin;

            StreamReader file = new StreamReader(@"C:\Users\1225908\source\repos\Project7\Project7\bin\Debug\test123.txt");
            while ((lin = file.ReadLine()) != null)
            {
                dataList.Add(double.Parse(lin));
                c++;
            }


            double[] arr = dataList.ToArray();
            // Строим описание синусоиды
            GeometryGroup geometryGroup = new GeometryGroup();
            for (int i = 0; i <= (arr.Length - 3); i+=2)
            {
                LineGeometry line = new LineGeometry(
                    new Point(arr[i+2],
                        -arr[i+3]),
                    new Point(arr[i ],
                        -arr[i + 1]));
                geometryGroup.Children.Add(line);
            }

            // Сохраняем описание геометрии
            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = geometryGroup;

            // Настраиваем перо
            geometryDrawing.Pen = new Pen(Brushes.Blue, 0.05);

            // Добавляем готовый слой в контейнер отображения
            drawingGroup.Children.Add(geometryDrawing);
        }
         
        // Надписи
        private void MarkerFun()
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            for (double i = -10; i <= 5; i++)
            {
                FormattedText formattedText = new FormattedText(
                String.Format("{0,7:F}",   -i * 2),
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                1,
                Brushes.Black);

                formattedText.SetFontWeight(FontWeights.Bold);

                Geometry geometry = formattedText.BuildGeometry(new Point(-4, i *2));
                geometryGroup.Children.Add(geometry);
            }

            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = geometryGroup;

            geometryDrawing.Brush = Brushes.LightGray;
            geometryDrawing.Pen = new Pen(Brushes.Gray, 0.03);

            drawingGroup.Children.Add(geometryDrawing);
        }
    }
}
