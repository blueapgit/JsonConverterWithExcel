using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonConverterWithCSV
{
	public partial class Popup : Form
	{
		private Action OnClosing;

		public Popup()
		{
			InitializeComponent();
		}

		public void SetClosingEvent(Action method)
		{
			this.OnClosing = method;
		}

		public void SetMessage(string message)
		{
			label.Text = message;
		}

		private void close_Click(object sender, EventArgs e)
		{
			OnClosing?.Invoke();
			this.Close();
		}
	}
}
