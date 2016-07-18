using System.Windows;

namespace ElasticSearchPopulator.Gui
{
    using System;

    using Core;
    using Core.Data;

    using Core.DataWriter;

    using ElasticSearchPopulator.Core.Utilities;

    using Newtonsoft.Json;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDataItemGenerator<Citizen> citizenDataItemGenerator;

        private IDataContext context;

        private IDataWriter dataWriter;

        private IDataGenerator<Citizen> citizenDataGenerator;

        public MainWindow()
        {
            
            this.InitializeComponent();

            this.context = new ElasticSearchHttpDataContext();
            this.dataWriter = new ElasticDataWriter();
            this.citizenDataItemGenerator = new CitizenDataItemGenerator(new RandomDataGenerator());
            
        }

        private async void PopulateDataButton_Click(object sender, RoutedEventArgs e)
        {
            this.ProgressBar.Visibility = Visibility.Visible;

            var progress = new Progress<double>(
                percent =>
                    {
                        this.ProgressBar.Value = percent;
                    });

            this.citizenDataGenerator = new CitizenDataGenerator(this.context, this.dataWriter, this.citizenDataItemGenerator);

            this.OutputTextBlock.Text = await this.citizenDataGenerator.Generate(progress, this.TryGetRecordValue(), this.TryGetBatchValue());

            this.ProgressBar.Visibility = Visibility.Hidden;
        }

        private async void DeleteIndex_Click(object sender, RoutedEventArgs e)
        {
            this.OutputTextBlock.Text = await this.context.DeleteIndex("citizens");
        }

        private const string SendToTemplate = "Send to: {0}";

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.SendToTextBox.Text = SendToTemplate.InsertArgs("CSV");

            this.context = new CsvDataContext();
            this.dataWriter = new CsvDataWriter();
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            this.SendToTextBox.Text = SendToTemplate.InsertArgs("ElasticSearch");

            this.context = new ElasticSearchHttpDataContext();
            this.dataWriter = new ElasticDataWriter();
        }

        private void CreateMappingsDataButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateIndexDataButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TestLength_Click(object sender, RoutedEventArgs e)
        {
            ITestLength testLength = new TestLength();

            IDataItemGenerator<Citizen> testCitizenDataItemGenerator = new CitizenDataItemGenerator(new RandomDataGenerator());

            var sizeInBytes = testLength.GetSizeInBytes(this.TryGetRecordValue(), () => testCitizenDataItemGenerator.Generate());

            this.OutputTextBlock.Text = JsonConvert.SerializeObject(new { SizeInBytes = sizeInBytes.ToString("N0") }, Formatting.Indented);
        }

        private int TryGetRecordValue()
        {
            return this.TryGetIntValue(this.RecordTextBox.Text);
        }

        private int TryGetBatchValue()
        {
            return this.TryGetIntValue(this.BatchTextBox.Text);
        }

        private int TryGetIntValue(string text)
        {
            int val;
            if (int.TryParse(text, out val))
            {
                return val;
            }

            return 0;
        }
    }

    public static class TemplateExtensions
    {
        public static string InsertArgs(this string template, params object[] args)
        {
            return string.Format(template, args);
        }
    }
}
