using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
using Newtonsoft.Json;

using Requester;

namespace GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<Requester.DataGridValue> paramsData = new List<Requester.DataGridValue>();
		List<Requester.DataGridValue> headerData = new List<Requester.DataGridValue>();

		public MainWindow()
		{
			InitializeComponent();

			dataGridHeader.ItemsSource = headerData;
			dataGridParams.ItemsSource = paramsData;

			var template = new TemplateLoader();
			ApplySavedData(template.New());
		}

		// events
		private async void SendButton_Click(object sender, RoutedEventArgs e)
		{
			await SendRequestFromGUI();
		}
		private void newButton_Click(object sender, RoutedEventArgs e)
		{
			var template = new TemplateLoader();
			ApplySavedData(template.New());
		}
		private void loadButton_Click(object sender, RoutedEventArgs e)
		{
			LoadTemplate();
		}
		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			SaveTemplate();
		}
		private async void urlTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				await SendRequestFromGUI();
				e.Handled = true;
			}
		}
		private async void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				await SendRequestFromGUI();
				e.Handled = true;
			}

			if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				SaveTemplate();
				e.Handled = true;
			}

			if (e.Key == Key.O && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				LoadTemplate();
				e.Handled = true;
			}
		}

		// methods
		private async Task SendRequestFromGUI()
		{
			infoTextBlock.Text = "Loading ...";

			rawTextBlock.Document.Blocks.Clear();
			FullTextBlock.Document.Blocks.Clear();
			previewHTML.Navigate((Uri)null);

			try
			{
				Requester.Requester cm = new Requester.Requester();
				string url = urlTextBox.Text;
				string method = methodComboBox.Text;
				Dictionary<string, string> header = new Dictionary<string, string>();

				foreach (Requester.DataGridValue item in dataGridHeader.Items.SourceCollection)
				{
					if (item.active && item.key != null)
						header.Add(item.key, item.value);
				}

				string body = new TextRange(
					bodyTextBlock.Document.ContentStart,
					bodyTextBlock.Document.ContentEnd
				).Text;

				Dictionary<string, string> dataGridParamsInDictionary = new Dictionary<string, string>();
				foreach (Requester.DataGridValue item in dataGridParams.Items.SourceCollection)
				{
					if (item.active)
						dataGridParamsInDictionary.Add(item.key, item.value);
				}			
				url = cm.ApplyParamsToUrl(url, dataGridParamsInDictionary);


				ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
				RequestResponse response = await cm.Send(method, url, body, header);

				if (response.content != null && response.content != "")
				{
					previewHTML.NavigateToString(response.content);
				}

				rawTextBlock.Document.Blocks.Add(new Paragraph(new Run(response.content)));

				// make slowest part in parallel
				formatedRichTextBox.Dispatcher.Invoke(new Action(() =>
				{
					var output = formatedRichTextBox;
					output.Document.Blocks.Clear();
					Beautifier beautifier = new Beautifier();
					beautifier.BeautyRichTextBox(response.content, output);
				}));

				FullTextBlock.Document.Blocks.Add(new Paragraph(new Run(response.statusCode.Key + " (" + response.statusCode.Value + ")")));
				FullTextBlock.Document.Blocks.Add(new Paragraph(new Run(response.header)));
				FullTextBlock.Document.Blocks.Add(new Paragraph(new Run(response.timing + "ms")));
				FullTextBlock.Document.Blocks.Add(new Paragraph(new Run(response.content)));

				StatusCodeTextBox.Text = response.statusCode.Key + " (" + response.statusCode.Value + ")";
				string[] statusCodeColors = new string[]{
					"#bdbdbd", // 1xx
					"#2e7d32", // 2xx
					"#1565c0", // 3xx
					"#c62828", // 4xx
					"#6a1b9a", // 5xx
				};
				StatusCodeTextBox.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(statusCodeColors[(int)(response.statusCode.Key / 100 - 1)]));

				infoTextBlock.Text = "Loading complete";
			}
			catch (Exception e)
			{
				if (e.InnerException != null)
				{
					StatusCodeTextBox.Text = "404 (Page Not Found)";
					StatusCodeTextBox.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#c62828"));
					infoTextBlock.Text = e.InnerException.Message;
				}
				else
				{
					infoTextBlock.Text = e.Message;
				}
			}
		}
		private void ApplySavedData(Requester.savedDataFormat data)
		{
			urlTextBox.Text = data.url;

			methodComboBox.Text = data.method.ToUpper();

			TextRange tr = new TextRange(bodyTextBlock.Document.ContentEnd, bodyTextBlock.Document.ContentEnd);
			tr.Text = data.content;

			headerData.Clear();
			foreach (var item in data.header)
			{
				headerData.Add(item);
			}
			dataGridHeader.Items.Refresh();

			paramsData.Clear();
			foreach (var item in data.parameters)
			{
				paramsData.Add(item);
			}
			dataGridParams.Items.Refresh();
		}
		private Requester.savedDataFormat GetSavedData()
		{
			Requester.savedDataFormat toReturn = new Requester.savedDataFormat();
			toReturn.url = urlTextBox.Text;
			toReturn.method = methodComboBox.Text;
			toReturn.content = new TextRange(
				bodyTextBlock.Document.ContentStart,
				bodyTextBlock.Document.ContentEnd
			).Text;

			toReturn.header = new List<Requester.DataGridValue>();
			foreach (var item in headerData)
			{
				toReturn.header.Add(item);
			}

			toReturn.parameters = new List<Requester.DataGridValue>();
			foreach (var item in paramsData)
			{
				toReturn.parameters.Add(item);
			}

			return toReturn;
		}
		private void LoadTemplate()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Template file (*.json)|*.json";
			openFileDialog.RestoreDirectory = true;
			openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

			if (openFileDialog.ShowDialog() == true)
			{
				infoTextBlock.Text = "Loading template";
				var template = new TemplateLoader();
				ApplySavedData(template.Load(openFileDialog.FileName));
				infoTextBlock.Text = "Template loaded from " + openFileDialog.FileName;
			}
		}
		private void SaveTemplate()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Template file (*.json)|*.json";
			saveFileDialog.Title = "Save Template File";
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			saveFileDialog.ShowDialog();
			if (saveFileDialog.FileName != "")
			{
				infoTextBlock.Text = "Saving template";
				var template = new TemplateLoader();
				template.Save(saveFileDialog.FileName, GetSavedData());
				infoTextBlock.Text = "Template saved to" + saveFileDialog.FileName;
			}
		}
	}	
}
