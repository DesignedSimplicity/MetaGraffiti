using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

using MetaGraffiti.Base.Modules.Ortho.Info;

namespace MetaGraffiti.App.GpxGraffiti
{
	public partial class MainForm : Form
	{
		private string _path = "";

		public MainForm()
		{
			InitializeComponent();

			txtPath.Text = @"D:\Development\Sources\MetaGraffiti\MetaGraffiti.Base.Tests\Data";
			txtPath.KeyPress += PathKeyPress;
		}

		private void cmdPath_Click(object sender, EventArgs e)
		{
			LoadPath(txtPath.Text.Trim());
		}

		private void PathKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) LoadPath(txtPath.Text.Trim());
		}

		private void listFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listFiles.SelectedItem == null)
				listInfo.Items.Clear();
			else
				ShowInfo(listFiles.SelectedItem.ToString());
		}

		private void ShowInfo(string gpx)
		{
			try
			{
				string uri = Path.Combine(_path, gpx);
				var g = new GpxFileInfo(uri);

				listInfo.BeginUpdate();
				listInfo.Items.Clear();

				listInfo.Items.Add($"Name: {g.Name}");
				listInfo.Items.Add($"Description: {g.Description}");
				listInfo.Items.Add($"Tracks: {g.Tracks.Count}");
				listInfo.Items.Add($"Points: {g.Points.Count()}");

				listInfo.EndUpdate();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading gpx file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadPath(string path)
		{
			if (!Directory.Exists(path))
			{
				MessageBox.Show("Path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			_path = path;
			listFiles.BeginUpdate();
			listFiles.Items.Clear();
			var dir = new DirectoryInfo(path);
			foreach (var file in dir.GetFiles())
			{
				if (file.Extension.ToLowerInvariant() == ".gpx")
				{
					listFiles.Items.Add(file.Name);
				}
			}
			listFiles.EndUpdate();

			if (listFiles.Items.Count == 0)
				MessageBox.Show("Path does not contain gpx files", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private void cmdSaveGpx_Click(object sender, EventArgs e)
		{

		}
	}
}
