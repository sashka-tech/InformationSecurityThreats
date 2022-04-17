using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lab2
{
	class Paging
	{
        public int PageIndex { get; set; }

        DataTable PagedList = new DataTable();

        public DataTable Next(IList<Threat> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable Previous(IList<Threat> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if(PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable First(IList<Threat> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable Last(IList<Threat> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

		public DataTable SetPaging(IList<Threat> ListToPage, int RecordsPerPage)
		{
			int PageGroup = PageIndex * RecordsPerPage;

			IList<Threat> PagedList = new List<Threat>();

			PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();

			DataTable FinalPaging = PagedTable(PagedList);

			return FinalPaging;
		}

		private DataTable PagedTable<T>(IList<T> SourceList)
		{
			Type columnType = typeof(T);
			DataTable TableToReturn = new DataTable();
			foreach (var Column in columnType.GetProperties())
			{
                if (Column.Name == "Id")
                {
                    TableToReturn.Columns.Add("ID", Column.PropertyType);
                }
                if (Column.Name == "Name")
                {
                    TableToReturn.Columns.Add("Наименование УБИ", Column.PropertyType);
                }
                if (Column.Name == "Description")
                {
                    TableToReturn.Columns.Add("Описание", Column.PropertyType);
                }
                if (Column.Name == "Source")
                {
                    TableToReturn.Columns.Add("Источник угрозы", Column.PropertyType);
                }
                if (Column.Name == "Target")
                {
                    TableToReturn.Columns.Add("Объект воздействия", Column.PropertyType);
                }
                if (Column.Name == "ConfidentialityBreach")
                {
                    TableToReturn.Columns.Add("Нарушение конфиденциальности", Column.PropertyType);
                }
                if (Column.Name == "IntegrityViolation")
                {
                    TableToReturn.Columns.Add("Нарушение целостности", Column.PropertyType);
                }
                if (Column.Name == "AccessViolation")
                {
                    TableToReturn.Columns.Add("Нарушение доступности", Column.PropertyType);
                }
			}

            foreach (object item in SourceList)
            {
                int i = 0;
                DataRow ReturnTableRow = TableToReturn.NewRow();
                foreach (var Column in columnType.GetProperties())
                {
                    ReturnTableRow[i++] = Column.GetValue(item);
                }
                TableToReturn.Rows.Add(ReturnTableRow);
            }
            return TableToReturn;
		}
	}
}
