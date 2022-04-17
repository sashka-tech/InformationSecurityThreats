using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExcelDataReader;

namespace Lab2
{
    class Parser
    {
        private string fileName = string.Empty;
        public IList<Threat> GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                using (Stream stream = File.Open("thrlist.xlsx", FileMode.Open))
                {
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                ds = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = (x) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                }
            }
            catch (FileFormatException)
            {
                MessageBox.Show("Your file seems to be corrupted or has wrong format");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Missing file or disk fails");
            }
            catch (IOException)
            {
                MessageBox.Show("Refresh button needs to rest... Please try again");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("OS denies acces to file \n" + " Try again and...\n  May the Force be with you");
            }
            catch (Exception)
            {
                MessageBox.Show("Error! Please try again...");
            }
            Threat thrt;
            List<Threat> threatList = new List<Threat>();
            foreach (DataTable table in ds.Tables)
                for(int i = 1; i < table.Rows.Count; i++)
                {
                    thrt = new Threat();
                    thrt.Id = Convert.ToInt32(table.Rows[i][0]);
                    thrt.Name = Convert.ToString(table.Rows[i][1]);
                    thrt.Description = Convert.ToString(table.Rows[i][2]);
                    thrt.Source = Convert.ToString(table.Rows[i][3]);
                    thrt.Target = Convert.ToString(table.Rows[i][4]);
                    thrt.ConfidentialityBreach = Convert.ToString(table.Rows[i][5]) == "1" ? true : false;
                    thrt.IntegrityViolation = Convert.ToString(table.Rows[i][6]) == "1" ? true : false;
                    thrt.AccessViolation = Convert.ToString(table.Rows[i][7]) == "1" ? true : false;
                    threatList.Add(thrt);
                }
            return threatList;
        }
    }
}
