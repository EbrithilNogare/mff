using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Requester;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

		public MainWindow()
        {
            InitializeComponent();

			var headerdata = new List<HeaderDataGridValue>();
			dataGridHeader.ItemsSource = headerdata;
		}

        private async void SendButton_Click(object sender, RoutedEventArgs e)
		{

			string url = urlTextBox.Text;
			string method = methodComboBox.Text;
			string header = "";// headerTextBlock.Text; //TODO

			Requester.Requester cm = new Requester.Requester();
			RequestResponse response = await cm.Send(method, url, header);

			previewHTML.NavigateToString(response.content);
			var output = formatedRichTextBox;
			output.Document.Blocks.Clear();
			Beautifier beautifier = new Beautifier();
			beautifier.BeatyRichTextBox(response.content, output);
			rawTextBlock.Text = response.content;

			FullTextBlock.Text =
				response.statusCode.Key + " (" + response.statusCode.Value + ")\n\n" +
				response.header + "\n" +
				response.timing + "ms\n\n" +
				"------------------Response------------------" + "\n" +
				response.content;

		}
	}

	class HeaderDataGridValue 
	{
		public bool active { get; set; }
		public string key { get; set; }
		public string value { get; set; }
	}
}
