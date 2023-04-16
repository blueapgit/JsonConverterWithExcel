using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace JsonConverterWithCSV
{
	public partial class Form1 : Form
	{
		public class ColumnData
		{
			public string column;
			public string columnFull;
			public string typeName;
			public string dataTypeName;

			public ColumnData(string column)
			{
				this.column = column;
			}

			public ColumnData(string column, string columnFull, string type)
			{
				this.column = column;
				this.columnFull = columnFull;
				this.typeName = type;
			}
		}

		private Excel.Application app = null;

		public Form1()
		{
			InitializeComponent();
		}

		private void BtnLoad_Click(object sender, EventArgs e)
		{
			if (OpenCsvFileDialog.ShowDialog() != DialogResult.OK)
				return;

			listView.Items.Clear();

			foreach (var fileName in OpenCsvFileDialog.FileNames)
			{
				listView.Items.Add(fileName);
			}
		}

		private void Export_Click(object sender, EventArgs e)
		{
			if (listView.Items.Count == 0)
			{
				ShowPopup("먼저 파일을 불러와주세요");
				return;
			}

			if(app == null)
				app = new Excel.Application();

			FolderBrowserDialog dialog = new FolderBrowserDialog();

			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			string saveDir = dialog.SelectedPath;

			ExportProgressBar.Value = 0;
			ProgressLabel.Text = "(0/" + listView.Items.Count + ")";

			int successCount = 0;

			foreach (ListViewItem item in listView.Items)
			{
				successCount++;

				string path = item.Text;
				string fileName = Path.GetFileNameWithoutExtension(path);
				string savePath = saveDir + "\\" + fileName + ".json";

				ExportProgressBar.Value = (int)(successCount / (float)listView.Items.Count * 100.0f);
				ProgressLabel.Text = savePath + " (" + successCount + "/" + listView.Items.Count + ")";

				JObject jObject = ExcelToJObject(path);

				if (jObject == null)
				{
					ClearProgressBar();
					return;
				}

				using (StreamWriter sw = new StreamWriter(savePath))
				{
					using (JsonTextWriter jw = new JsonTextWriter(sw))
					{
						jw.Formatting = Formatting.Indented;
						jw.IndentChar = ' ';
						jw.Indentation = 4;

						JsonSerializer serializer = new JsonSerializer();
						serializer.Serialize(jw, jObject);
					}
				}

				ExportProgressBar.Value = (int)(successCount / (float)listView.Items.Count * 100.0f);
				ProgressLabel.Text = savePath + " (" + successCount + "/" + listView.Items.Count + ")";
			}

			ExportProgressBar.Value = 100;

			app.Quit();
			GC.Collect();
			while(System.Runtime.InteropServices.Marshal.ReleaseComObject(app) != 0);
			app = null;

			ShowPopup("Export 완료했습니다.", ClearProgressBar);
		}

		private void ClearProgressBar()
		{
			ExportProgressBar.Value = 0;
			ProgressLabel.Text = "";
		}

		private void ShowPopup(string message)
		{
			Popup popup = new Popup();
			popup.SetMessage(message);
			popup.StartPosition = FormStartPosition.CenterParent;
			popup.ShowDialog();
		}

		private void ShowPopup(string message, Action onClosing)
		{
			Popup popup = new Popup();
			popup.SetMessage(message);
			popup.StartPosition = FormStartPosition.CenterParent;
			popup.SetClosingEvent(onClosing);
			popup.ShowDialog();
		}

		private JObject ExcelToJObject(string path)
		{
			Excel.Workbooks workbooks = app.Workbooks;
			Excel.Workbook workbook = workbooks.Open(path);
			Excel.Worksheet sheet = workbook.Sheets[1];
			Excel.Range cells = sheet.UsedRange.Cells;

			int columnLine = 0;

			for (int i = 3; i <= cells.Rows.Count; i++)
			{
				Excel.Range cell = cells[i, 1];
				object value = cell.Value;

				if (value is double)
					break;

				columnLine = i;
			}

			List<List<ColumnData>> columnDataList = CreateColumns(sheet, columnLine);

			JObject jObject = CreateData(columnDataList, sheet, columnLine);

			workbook.Close();

			while (System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook) != 0) ;
			while (System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks) != 0) ;

			return jObject;
		}

		public List<List<ColumnData>> CreateColumns(Excel.Worksheet sheet, int columnLine)
		{
			Excel.Range cells = sheet.UsedRange.Cells;

			List<List<ColumnData>> result = new List<List<ColumnData>>();

			ColumnData[] lastColumnArr = new ColumnData[columnLine * 2];

			List<ColumnData> columnDataList = new List<ColumnData>();

			string column = null;
			ColumnData columnData = null;
			ColumnData lastColumnData = null;

			for (int x = 1; x <= cells.Columns.Count; x++)
			{
				string dataTypeName = cells[1, x].Value?.ToString();
				lastColumnData = null;

				for (int y = 2; y <= columnLine; y++)
				{
					column = cells[y, x].Value?.ToString();

					if (string.IsNullOrWhiteSpace(column))
					{
						columnData = lastColumnArr[y - 2];
					}
					else
					{
						columnData = new ColumnData(column);

						lastColumnArr[y - 2] = columnData;

						for (int i = y - 1; i < lastColumnArr.Length; i++)
						{
							lastColumnArr[i] = null;
						}

						if (lastColumnData != null)
							columnData.columnFull = lastColumnData.columnFull + "/" + columnData.column;
						else
							columnData.columnFull = "@Root/" + sheet.Name + "/" + columnData.column;
					}

					lastColumnData = columnData;
				}

				columnData = null;

				for (int i = lastColumnArr.Length - 1; i >= 0; i--)
				{
					lastColumnData = columnData;
					columnData = lastColumnArr[i];

					if (columnData == null)
						continue;

					if (columnData.typeName != null)
					{
						columnDataList.Add(columnData);
						continue;
					}

					if (columnData.column.IndexOf("//") == 0)
						columnData.typeName = "Null";
					else if (columnData.column.IndexOf("[") != -1)
					{
						columnData.column = columnData.column.Replace("[]", "");
						columnData.typeName = lastColumnData == null ? "JArray/JValue" : "JArray/JObject";
						if(lastColumnData == null)
							columnData.dataTypeName = dataTypeName;
					}
					else if (lastColumnData == null)
					{
						columnData.typeName = "JProperty";
						columnData.dataTypeName = dataTypeName;
					}
					else
						columnData.typeName = "JObject";

					columnDataList.Add(columnData);
				}

				columnDataList.Add(new ColumnData(sheet.Name, "@Root/" + sheet.Name, "JArray/JObject"));
				columnDataList.Add(new ColumnData("@Root", "@Root", "JObject"));

				columnDataList.Reverse();

				result.Add(columnDataList);

				columnDataList = new List<ColumnData>();
			}

			return result;
		}

		public JObject CreateData(List<List<ColumnData>> columnsList, Excel.Worksheet sheet, int columnLine)
		{
			Dictionary<string, JToken> map = new Dictionary<string, JToken>();

			Excel.Range cells = sheet.UsedRange.Cells;

			string data = null;

			for (int y = columnLine + 1; y <= cells.Rows.Count; y++)
			{
				for (int x = 1; x <= cells.Columns.Count; x++)
				{
					data = cells[y, x].Value?.ToString();

					if (string.IsNullOrWhiteSpace(data))
						continue;

					List<ColumnData> columns = columnsList[x - 1];

					JToken parentContainer = null;
					JToken container = null;

					foreach (var column in columns)
					{
						if (column == null)
							continue;

						if (column.typeName == "Null")
							continue;

						if (map.ContainsKey(column.columnFull) == false)
						{
							container = GetOrCreateContainer(parentContainer, column, data);

							if (container == null)
								return null;

							map.Add(column.columnFull, container);
						}
						else
						{
							container = map[column.columnFull];

							if (column.typeName == "JProperty" && container.Parent?.Parent != null && container.Parent.Parent is JArray)
							{
								var keyValue = map.Where(item => item.Value == container.Parent).First();

								var keys = map.Select(item => item.Key).ToArray();

								foreach (var key in keys)
								{
									if (IsParent(map[key], container.Parent) == false)
										continue;

									map.Remove(key);
								}

								parentContainer = new JObject();

								container.Parent.Parent.Add(parentContainer);

								map.Add(keyValue.Key, parentContainer);

								container = GetOrCreateContainer(parentContainer, column, data);

								if (container == null)
									return null;

								map.Add(column.columnFull, container);
							}
							else if(column.typeName == "JArray/JValue")
							{
								JArray parent = container.Parent as JArray;

								container = new JValue(GetData(data, column.dataTypeName));
								parent.Add(container);

								map[column.columnFull] = container;
							}
						}

						parentContainer = container;
					}
				}
			}

			return map["@Root"] as JObject;
		}

		public bool IsParent(JToken target, JContainer parent)
		{
			if (target == null)
				return false;

			if (target == parent)
				return true;

			return IsParent(target.Parent, parent);
		}

		public JToken GetOrCreateContainer(JToken parent, ColumnData columnData, string data)
		{
			if (columnData.typeName == "JObject")
			{
				if (parent != null)
				{
					if (parent is JObject)
					{
						JObject parentObj = parent as JObject;

						if (parentObj.ContainsKey(columnData.column) == false)
						{
							JObject jObject = new JObject();
							parentObj.Add(columnData.column, jObject);

							return jObject;
						}
						else
							return parentObj.GetValue(columnData.column);
					}
					else
					{
						JObject jObject = new JObject();
						parent.AddAfterSelf(jObject);
						return jObject;
					}
				}
				else
					return new JObject();
			}
			else if (columnData.typeName == "JProperty")
			{
				object convertedData = GetData(data, columnData.dataTypeName);

				if (convertedData == null)
					return null;

				JProperty jProperty = new JProperty(columnData.column, convertedData);

				if (parent != null)
				{
					JObject parentObject = parent as JObject;
					parentObject.Add(jProperty);
				}

				return jProperty;
			}
			else if (columnData.typeName == "JArray/JObject")
			{
				if (parent != null)
				{
					if (parent is JObject)
					{
						JObject parentObj = parent as JObject;

						if (parentObj.ContainsKey(columnData.column) == false)
						{
							JArray jArray = new JArray();
							parentObj.Add(columnData.column, jArray);
							JObject jArrayItem = new JObject();
							jArray.Add(jArrayItem);
							return jArrayItem;
						}
						else
						{
							JArray jArray = parentObj.GetValue(columnData.column) as JArray;
							return jArray.Last();
						}
					}
					else
					{
						JArray jArray = new JArray();
						parent.AddAfterSelf(jArray);
						return jArray;
					}
				}
			}
			else if (columnData.typeName == "JArray/JValue")
			{
				if (parent != null)
				{
					if (parent is JObject)
					{
						JObject parentObj = parent as JObject;

						if (parentObj.ContainsKey(columnData.column) == false)
						{
							object convertedData = GetData(data, columnData.dataTypeName);

							JArray jArray = new JArray();
							parentObj.Add(columnData.column, jArray);
							JValue jArrayItem = new JValue(convertedData);
							jArray.Add(jArrayItem);
							return jArrayItem;
						}
						else
						{
							JArray jArray = parentObj.GetValue(columnData.column) as JArray;
							return jArray.Last();
						}
					}
					else
					{
						JArray jArray = new JArray();
						parent.AddAfterSelf(jArray);
						return jArray;
					}
				}
			}

			return null;
		}

		public object GetData(string data, string dataType)
		{
			string type = dataType.ToLower();

			if (type == "int")
			{
				int resultInt = 0;
				if (int.TryParse(data, out resultInt) == false)
				{
					ShowPopup("잘못된 컬럼 자료형이 있습니다. 값:" + data + ", 자료형:" + dataType);
					return null;
				}

				return resultInt;
			}
			else if (type == "long")
			{
				long resultLong = 0;
				if (long.TryParse(data, out resultLong) == false)
				{
					ShowPopup("잘못된 컬럼 자료형이 있습니다. 값:" + data + ", 자료형:" + dataType);
					return null;
				}

				return resultLong;
			}
			else if(type == "float")
			{
				float resultFloat = 0;
				if (float.TryParse(data, out resultFloat) == false)
				{
					ShowPopup("잘못된 컬럼 자료형이 있습니다. 값:" + data + ", 자료형:" + dataType);
					return null;
				}

				return resultFloat;
			}
			else if (type == "double")
			{
				double resultDouble = 0;
				if (double.TryParse(data, out resultDouble) == false)
				{
					ShowPopup("잘못된 컬럼 자료형이 있습니다. 값:" + data + ", 자료형:" + dataType);
					return null;
				}

				return resultDouble;
			}
			else if(type == "bool")
			{
				int number = 0;
				if(int.TryParse(data, out number))
				{
					return number == 0 ? false : true;
				}
				else
				{
					return data.ToLower() == "true" ? true : false;
				}
			}
			else
				return data;
		}

		private void OpenCsvFileDialog_FileOk(object sender, CancelEventArgs e)
		{

		}

		private void RemoveBtn_Click(object sender, EventArgs e)
		{
			while (listView.SelectedItems.Count > 0)
				listView.Items.Remove(listView.SelectedItems[0]);
		}

		private void ExportBrowserDialog_HelpRequest_1(object sender, EventArgs e)
		{

		}
	}
}