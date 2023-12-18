﻿using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using System.Text;

namespace DemoReadFileTrailerCS
{
    public partial class Form1 : Form
    {

        private readonly string _strPath = @"C:\Hosts\Files\";
        private readonly string _strFileName = DateTime.Now.Date.ToShortDateString() + ".txt";

        private StreamReader _textReader;
        private int _fileLength;

        public Form1()
        {
            InitializeComponent();
            btnStartMonitoring_Click(null, null);
            btnStartMonitoring.Visible = false;
            this.Hide();
        }

        private void btnStartMonitoring_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fileSystemWatcher1.Path))
            {
                fileSystemWatcher1.Changed -= fileSystemWatcher_Changed;
            }

            //load the current file
            listBox1.Items.Add($"Start monitoring {_strFileName} at {DateTime.Now.ToLongTimeString()}");
            FullLoad();

            //initialize the file system watcher and start monitoring
            fileSystemWatcher1.Path = _strPath;
            fileSystemWatcher1.Filter = _strFileName;
            fileSystemWatcher1.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher1.EnableRaisingEvents = true;

            fileSystemWatcher1.Changed += fileSystemWatcher_Changed;
        }

        private void FullLoad()
        {
            try
            {
                string line;
                var fs = new FileStream(Path.Combine(_strPath, _strFileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                _textReader = new StreamReader(fs);

                //while ((line = _textReader.ReadLine()) != null)
                //{
                //    listBox1.Items.Add(line);
                //}

                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                _fileLength = (int)_textReader.BaseStream.Length;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("You need to create your test file first");
            }
        }

        private void CopyToSQL(string lines)
        {
            if (string.IsNullOrEmpty(lines))
            {
                return;
            }
            var dt = new DataTable();

            for (int index = 0; index < 25; index++)
                dt.Columns.Add(new DataColumn());

            foreach (var line in lines.Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                try
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        var cols = line.Split(',');

                        DataRow dr = dt.NewRow();
                        for (int cIndex = 0; cIndex < 25; cIndex++)
                        {
                            dr[cIndex] = cols[cIndex];
                        }

                        dt.Rows.Add(dr);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            //do
            //{
            //    DataRow row = dt.NewRow();

            //    string[] itemArray = line.Split(',');
            //    row.ItemArray = itemArray;
            //    dt.Rows.Add(row);
            //    i = i + 1;
            //    line = sr.ReadLine();
            //} while (!string.IsNullOrEmpty(line));
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                var bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null)
                {
                    DestinationTableName = "dbo.Ticker_Stocks",
                    BatchSize = dt.Rows.Count
                };
                conn.Open();
                bc.WriteToServer(dt);
                conn.Close();
                bc.Close();
            }

           
        }
        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //sometimes the event is triggered with an invalid stream
            if (_textReader.BaseStream.Length == 0) return;

            listBox1.Items.Add(new string('-', 40));
            listBox1.Items.Add($"{e.ChangeType} - Previous length={_fileLength} / Current length={_textReader.BaseStream.Length}");

            if (_textReader.BaseStream.Length > _fileLength)
            {
                listBox1.Items.Add(new string('-', 40));
                listBox1.Items.Add($"Adding new items at {DateTime.Now.ToLongTimeString()}");

                var strEndOfFile = _textReader.ReadToEnd();

                //foreach (string strNewItem in strEndOfFile.Split(new[] { "\r\n" }, StringSplitOptions.None))
                //{
                //    listBox1.Items.Add(strNewItem);
                //}
                CopyToSQL(strEndOfFile);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;

                _fileLength = (int)_textReader.BaseStream.Length;
            }
            else if (_textReader.BaseStream.Length < _fileLength)
            {
                //file is shorter, just reload to start fresh
                listBox1.Items.Add($"Stream is shorter than the previous one. Fully reloading the file at {DateTime.Now.ToLongTimeString()}");
                FullLoad();
            }
        }
    }
}
