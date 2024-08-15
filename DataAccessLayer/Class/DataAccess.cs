
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DomainLayer.Models;
using DomainLayer.ViewModels;
//using Sap.Data.Hana;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using xcel = ClosedXML;
using Excel = Microsoft.Office.Interop.Excel;
namespace DataAccessLayer.Class
{
    public class DataAccess
    {
        //Saving of added udf fields in created table
        //public static void AddHeaderUdf(int Check, List<string[]> TableFields, string TableName, int UserId, int MapId, int UpdateId)
        //{
        //    foreach (var item in TableFields)
        //    {
        //        Header header = new Header();
        //        header.MapId = Check == 1 ? MapId : Convert.ToInt32(UpdateId);
        //        header.TableName = TableName;
        //        header.SAPHeaderField = item[0].ToString();
        //        header.AddonHeaderField = item[1].ToString();
        //        header.DataType = item[2].ToString();
        //        header.Length = item[3].ToString();
        //        header.IsRequired = false;
        //        header.CreateDate = DateTime.Now;
        //        header.CreateUserID = UserId;
        //        //Header.Add(header);
        //    }
        //}

        //public static void AddRowUdf(int Check, List<string[]> TableFields, string TableName, int UserId, int MapId, int UpdateId)
        //{
        //    foreach (var item in TableFields)
        //    {
        //        Row row = new Row();
        //        row.MapId = Check == 1 ? MapId : Convert.ToInt32(UpdateId);
        //        row.TableName = TableName;
        //        row.SAPRowField = item[0].ToString();
        //        row.AddonRowField = item[1].ToString();
        //        row.DataType = item[2].ToString();
        //        row.Length = item[3].ToString();
        //        row.IsRequired = false;
        //        row.CreateDate = DateTime.Now;
        //        row.CreateUserID = UserId;
        //        //Row.Add(row);
        //    }
        //}

        public static DataTable Select(string ConString, string query)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            con.Open();
                            da.Fill(dt);
                            con.Close();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
                string err = ex.Message;
                DataTable dt = new DataTable();
                return dt;
            }
        }



        public static DataTable Select(string type, string Server, string User, string Password, string Database, string query)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    if (type.Contains("HANA"))
                    {
                        //using (HanaConnection con = new HanaConnection(QueryAccess.HANA_conString(Server, User, Password, Database)))
                        //{
                        //    using (HanaCommand cmd = new HanaCommand(query, con))
                        //    {
                        //        HanaDataAdapter da = new HanaDataAdapter(cmd);
                        //        con.Open();
                        //        da.Fill(dt);
                        //        con.Close();
                        return dt;
                        //    }
                        //}
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(QueryAccess.MSSQL_conString(Server, User, Password, Database)))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                con.Open();
                                da.Fill(dt);
                                con.Close();
                                return dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
                string err = ex.Message;
                return null;
            }
        }

        public static DataTable SelectHana(string ConString, string query)
        {
            try
            {  //2023/07/28 - I commented this because Error in posting in PCII
                using (DataTable dt = new DataTable())
                {
                    //    using (HanaConnection con = new HanaConnection(ConString))
                    //    {
                    //        using (HanaCommand cmd = new HanaCommand(query, con))
                    //        {
                    //            HanaDataAdapter da = new HanaDataAdapter(cmd);
                    //            con.Open();
                    //            da.Fill(dt);
                    //            con.Close();
                    return dt;
                    //        }
                    //    }
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
                return null;
            }
        }

        public static Boolean Execute(string ConString, string query)
        {
            Boolean _bool = false;
            try
            {

                using (SqlConnection con = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = con.CreateCommand();
                    con.Open();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    _bool = true;
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return _bool;
        }

        public static Boolean ExecuteHana(string ConString, string query)
        {
            Boolean _bool = false;
            try
            {

                //using (HanaConnection con = new HanaConnection(ConString))
                //{
                //    HanaCommand cmd = new HanaCommand();
                //    cmd = con.CreateCommand();
                //    con.Open();
                //    cmd.CommandText = query;
                //    cmd.ExecuteNonQuery();
                //    _bool = true;
                //}
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return _bool;
        }

        public static string Execute(string ConString, string query, string DocEntry)
        {
            string ret = "Error";
            try
            {

                using (SqlConnection con = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = con.CreateCommand();
                    con.Open();
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    ret = "";
                }
            }
            catch (Exception ex)
            {
                ret = $@"Error posting. Key: {DocEntry} " + ex.Message;
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return ret;
        }

        public static string Execute(string type, string Server, string User, string Password, string Database, string query, string DocEntry)
        {
            string ret = "Error";
            try
            {
                if (type.Contains("HANA"))
                {
                    //using (HanaConnection con = new HanaConnection(QueryAccess.HANA_conString(Server, User, Password, Database)))
                    //{
                    //    HanaCommand cmd = new HanaCommand();
                    //    cmd.CommandTimeout = 0;
                    //    cmd = con.CreateCommand();
                    //    con.Open();
                    //    cmd.CommandText = query;
                    //    cmd.ExecuteNonQuery();
                    //    ret = "";
                    //}
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(QueryAccess.MSSQL_conString(Server, User, Password, Database)))
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandTimeout = 0;
                        cmd = con.CreateCommand();
                        con.Open();
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        ret = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ret = $@"Error posting. Key: {DocEntry} " + ex.Message;
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return ret;
        }

        public static string ExecuteHana(string ConString, string query, string DocEntry)
        {
            string ret = "Error";
            try
            {

                //using (HanaConnection con = new HanaConnection(ConString))
                //{
                //    HanaCommand cmd = new HanaCommand();
                //    cmd = con.CreateCommand();
                //    con.Open();
                //    cmd.CommandText = query;
                //    cmd.ExecuteNonQuery();
                //    ret = "";
                //}
            }
            catch (Exception ex)
            {
                ret = $@"Error posting. Key: {DocEntry} " + ex.Message;
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return ret;
        }

        public static void SaveToFTP(MemoryStream stream, string FilePath, string user, string pass)
        {
            WebRequest request = WebRequest.Create(FilePath);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(user, pass);
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {

            }

            request.Abort();
            request = WebRequest.Create(FilePath);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(user, pass);

            using (Stream ftpStream = request.GetRequestStream())
            {
                stream.WriteTo(ftpStream);

                ftpStream.Close();
            }
        }

        public static List<string> GetFileListFTP(string FilePath, string user, string pass)
        {
            try
            {
                WebRequest request = WebRequest.Create(FilePath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(user, pass);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Stream ReadFromFTP(string FilePath, string user, string pass)
        {
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential(user, pass);

            byte[] newFileData = request.DownloadData(FilePath);
            MemoryStream stream = new MemoryStream(newFileData);

            return stream;

        }

        public string ConcatDecimal(string value)
        {
            if (value == "" || value == string.Empty || value == null)
            {
                return "0";
            }
            else if (value.Contains('.'))
            {
                var number = value.Split('.');
                var r = new Regex($@"^\d*[1-9]\d*$");
                if (r.IsMatch(number[1]))
                {
                    return string.Format(new NumberFormatInfo() { NumberDecimalDigits = 2 },
                        "{0:F}", new decimal(Convert.ToDouble(value))).ToString();
                }
                else
                {
                    return number[0];
                }
            }
            else
            {
                return value;
            }


        }

        public static DataSet GetExcelSheet(string filePath)
        {
            DataSet ds = new DataSet();
            using (xcel.Excel.XLWorkbook workBook = new xcel.Excel.XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                var wscnt = workBook.Worksheets.Count;
                foreach (var workSheet in workBook.Worksheets)
                {
                    //Create a new DataTable.
                    DataTable dt = new DataTable();
                    dt.TableName = workSheet.Name.Replace(" ", "");
                    ds.Tables.Add(dt);
                }
                ds.AcceptChanges();
            }
            return ds;
        }
        public static DataSet GetExcelColumn(string filePath, ExcelMapperViewModel.ExcelMapperModel model)
        {
            DataSet ds = new DataSet();
            using (xcel.Excel.XLWorkbook workBook = new xcel.Excel.XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                var wscnt = workBook.Worksheets.Count;
                foreach (var workSheet in workBook.Worksheets.Where(sel => sel.Name.Replace(" ", "") == model.Worksheet))//do this if you have many sheets???
                                                                                                                         //foreach (var workSheet in workBook.Worksheets)///I do this because there are only 1 sheets in the excel

                {
                    //Create a new DataTable.
                    DataTable dt = new DataTable();
                    dt.TableName = workSheet.Name.Replace(" ", "");
                    bool isHeader = false; //closeXCEL First Reading
                    int fcel = 0;
                    int lcel = 0;

                    var range = workSheet.RangeUsed();
                    var table = range.AsTable();
                    var fcell = table.Cells(c => c.Address.ToString() == model.HeaderRow).FirstOrDefault();

                    foreach (xcel.Excel.IXLRow row in workSheet.Rows($@"{fcell.Address.RowNumber}"))
                    {
                        fcel = row.Cells().First().Address.ColumnNumber;
                        lcel = row.Cells().Last().Address.ColumnNumber;
                        foreach (xcel.Excel.IXLCell cell in row.Cells())
                        {
                            //Validate if Row Name is the beginning from User Set
                            var celAddr = $@"{cell.Address.ColumnLetter}{cell.Address.RowNumber}";
                            if (model.HeaderRow == celAddr || isHeader == true)
                            {
                                dt.Columns.Add(cell.Value.ToString());
                                isHeader = true;
                                if (fcel == 0)
                                    fcel = cell.Address.ColumnNumber;

                            }
                        }

                        if (dt.Columns.Count > 0)
                        {
                            var drow = dt.NewRow();
                            dt.Rows.Add(drow);
                        }
                    }
                    ds.Tables.Add(dt);
                }
                ds.AcceptChanges();
            }
            return ds;
        }

        public static DataSet GetExcelData(string filePath, List<ExcelMapperViewModel.ExcelMapperModel> _model)
        {
            DataSet ds = new DataSet();
            using (xcel.Excel.XLWorkbook workBook = new xcel.Excel.XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                var wscnt = workBook.Worksheets.Count;
                foreach (var model in _model)
                {
                    foreach (var workSheet in workBook.Worksheets.Where(sel => sel.Name.Replace(" ", "") == model.Worksheet))
                    {
                        //Create a new DataTable.
                        DataTable dt = new DataTable();
                        dt.TableName = workSheet.Name.Replace(" ", "");
                        bool isHeader = false; //closeXCEL First Reading
                        int fcel = 0;
                        int lcel = 0;
                        int colStart = 0;

                        var range = workSheet.RangeUsed();
                        var table = range.AsTable();
                        bool _rowStarted = false;

                        foreach (xcel.Excel.IXLRow row in workSheet.Rows())
                        {
                            //try
                            //{

                            //fcel = row.Cells().First().Address.ColumnNumber;
                            //}
                            //catch (Exception EX)
                            //{

                            //    throw;
                            //}
                            fcel = row.Cells().First().Address.ColumnNumber;
                            lcel = row.Cells().Last().Address.ColumnNumber;
                            if (row.Cells(c => c.Address.ToString() == model.HeaderRow).Any())
                            {
                                foreach (xcel.Excel.IXLCell cell in row.Cells())
                                {
                                    //Validate if Row Name is the beginning from User Set
                                    var celAddr = $@"{cell.Address.ColumnLetter}{cell.Address.RowNumber}";
                                    if (model.HeaderRow == celAddr || isHeader == true)
                                    {
                                        if (colStart == 0)
                                            colStart = cell.Address.ColumnNumber; //Set the Start of Column

                                        dt.Columns.Add(cell.Value.ToString());
                                        isHeader = true;
                                        if (fcel == 0)
                                            fcel = cell.Address.ColumnNumber;

                                    }
                                }
                            }

                            if (row.Cells(c => c.Address.ToString() == model.DataRowStart).Any())
                                _rowStarted = true;

                            if (_rowStarted)
                            {
                                if (dt.Columns.Count > 0)
                                {
                                    int rCnt = 0;
                                    var drow = dt.NewRow();
                                    foreach (xcel.Excel.IXLCell cell in row.Cells())
                                    {
                                        if (cell.Address.ColumnNumber >= colStart)
                                        {
                                            if (cell.Address.ColumnNumber <= dt.Columns.Count)
                                            {
                                                drow[rCnt] = cell.Value.ToString();
                                                rCnt++;
                                            }
                                            else
                                                break;
                                        }
                                    }
                                    dt.Rows.Add(drow);
                                }
                            }

                        }
                        ds.Tables.Add(dt);
                    } //End Foreach xCel-Sheets
                    ds.AcceptChanges();
                } //End Foreach model                
            }
            return ds;
        }
        public static Boolean OPSGetExcelData(string filePath, ExcelMapperViewModel.ExcelMapperModel model, int StartRw, string PrimaryKey, string TableName, string con, string Database, List<OPSFieldSets> ColumnNames, string Dest, out string errorMessage, int MapId, string EntityName, string ErrorPath)
        {
            Boolean _bool = false;
            errorMessage = "";
            try
            {
                DataTable dt = new DataTable();
                int rowcounter = 0;
                bool rowCounterTotal = true;
                int startRowNumber = 1;
                bool _rowStarted = false;
                bool _finalRow = true;
                while (_finalRow)
                {
                    bool start = true;
                    using (xcel.Excel.XLWorkbook workBook = new xcel.Excel.XLWorkbook(filePath))
                    {
                        if (start == false)
                        {

                            break;
                        }
                        //Read the first Sheet from Excel file.
                        var wscnt = workBook.Worksheets.Count;
                        //foreach (var model in _model)
                        //{

                        foreach (var workSheet in workBook.Worksheets.Where(sel => sel.Name.Replace(" ", "") == model.Worksheet))
                        //foreach (var workSheet in workBook.Worksheets)

                        {
                            if (start == false)
                            {
                                break;
                            }
                            using (DataSet ds = new DataSet())
                            {
                                //using (DataTable dt = new DataTable())
                                //{

                                //dt.Rows.Clear();
                                //Create a new DataTable.

                                dt.TableName = workSheet.Name.Replace(" ", "");
                                bool isHeader = false; //closeXCEL First Reading
                                int fcel = 0;
                                int lcel = 0;
                                int lrow = 0;
                                int colStart = 0;

                                var range = workSheet.RangeUsed();
                                var table = range.AsTable();
                                lrow = workSheet.LastRowUsed().RowNumber();

                                //if (rowcounter == lrow - 1)
                                //{
                                //    _finalRow = false;
                                //    break;
                                //}



                                var divident = lrow / 1000; ///10000 / 1000 = 10 loops


                                //var starting_row = i * 1000;

                                for (int rowIndex = startRowNumber; rowIndex <= workSheet.LastRowUsed().RowNumber(); rowIndex++)
                                {
                                    if (start == false)
                                    {
                                        break;
                                    }
                                    xcel.Excel.IXLRow row = workSheet.Row(rowIndex);

                                    fcel = row.Cells().First().Address.ColumnNumber;
                                    lcel = row.Cells().Last().Address.ColumnNumber;

                                    if (row.Cells(c => c.Address.ToString() == model.HeaderRow).Any())
                                    {
                                        foreach (xcel.Excel.IXLCell cell in row.Cells())
                                        {
                                            //Validate if Row Name is the beginning from User Set
                                            var celAddr = $@"{cell.Address.ColumnLetter}{cell.Address.RowNumber}";
                                            if (model.HeaderRow == celAddr || isHeader == true)
                                            {
                                                if (colStart == 0)
                                                    colStart = cell.Address.ColumnNumber; //Set the Start of Column

                                                dt.Columns.Add(cell.Value.ToString());
                                                isHeader = true;
                                                if (fcel == 0)
                                                    fcel = cell.Address.ColumnNumber;

                                            }
                                        }
                                    }

                                    if (row.Cells(c => c.Address.ToString() == model.DataRowStart).Any() || _rowStarted)
                                        _rowStarted = true;

                                    if (_rowStarted)
                                    {
                                        if (dt.Columns.Count > 0)
                                        {
                                            int rCnt = 0;
                                            var drow = dt.NewRow();
                                            foreach (xcel.Excel.IXLCell cell in row.CellsUsed(xcel.Excel.XLCellsUsedOptions.AllFormats))
                                            {

                                                if (cell.Address.ColumnNumber >= colStart)
                                                {
                                                    if (cell.Address.ColumnNumber >= colStart && cell.Address.ColumnNumber <= dt.Columns.Count)
                                                    {
                                                        string newCellValue = cell.Value?.ToString() ?? "";
                                                        drow[cell.Address.ColumnNumber - 1] = newCellValue;
                                                    }
                                                    else
                                                        break;
                                                }
                                            }
                                            dt.Rows.Add(drow);
                                            rowcounter++;

                                            if (rowcounter == lrow - 1 || dt.Rows.Count >= 1000)
                                            {

                                                startRowNumber = rowcounter + 2;
                                                ds.Tables.Add(dt);
                                                ds.AcceptChanges();



                                                var columnData = ds.Tables.Cast<DataTable>()
                                                   .SelectMany(xtable => xtable.Rows.Cast<DataRow>()
                                                                .Select(xrow => xtable.Columns.Cast<DataColumn>()

                                                                                              .ToDictionary(column => column.ColumnName,
                                                                                                            column => xrow[column])))
                                                     .ToList();



                                                var columnList = ds.Tables.Cast<DataTable>()
                                                  .Select(xtable => xtable.Columns.Cast<DataColumn>().ToList())
                                                  .ToList();

                                                dt.Rows.Clear();
                                                //dt = new DataTable();
                                                ds.Clear();


                                                QueryAccess.OPSCreateTable(PrimaryKey, TableName, con, Database, columnList, MapId);
                                                QueryAccess.OPSInsertData(TableName, con, Database, columnList, columnData, PrimaryKey, Dest, EntityName, MapId, ErrorPath);

                                                ds.Tables.Clear();
                                                if (rowcounter == lrow - 1)
                                                {
                                                    _finalRow = false;
                                                    start = false;
                                                    break;
                                                }


                                                //ds = new DataSet();
                                                //dt.TableName = workSheet.Name.Replace(" ", "");


                                                //foreach (var column in columnList)
                                                //{
                                                //    foreach (var xlist in column)
                                                //    {
                                                //        dt.Columns.Add(xlist.ToString());
                                                //    }
                                                //}


                                                start = false;


                                            }


                                        }
                                    }

                                    //break;

                                }
                                //break;  

                                //}//end of Data Table
                            }//end of Dataset

                        }//end of foreach worksheet


                    }//end of using worksheet

                }//end of using while loop

                return _bool = true;
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;

                return _bool;
            }

        }


        //sir Denzo Version
        //public static Boolean OPSGetExcelData(string filePath, ExcelMapperViewModel.ExcelMapperModel model, int StartRw, string PrimaryKey, string TableName, string con, string Database, List<OPSFieldSets> ColumnNames, string Dest, out string errorMessage, int MapId)
        //{
        //    Boolean _bool = false;
        //    errorMessage = "";
        //    try
        //    {
        //        //DataSet ds = new DataSet();
        //        using (xcel.Excel.XLWorkbook workBook = new xcel.Excel.XLWorkbook(filePath))
        //        {

        //            //Read the first Sheet from Excel file.
        //            var wscnt = workBook.Worksheets.Count;
        //            //foreach (var model in _model)
        //            //{

        //            foreach (var workSheet in workBook.Worksheets.Where(sel => sel.Name.Replace(" ", "") == model.Worksheet))
        //            //foreach (var workSheet in workBook.Worksheets)

        //            {
        //                ////Create a new DataTable.
        //                DataTable dt = new DataTable();
        //                dt.TableName = workSheet.Name.Replace(" ", "");
        //                bool isHeader = false; //closeXCEL First Reading
        //                int fcel = 0;
        //                int lcel = 0;
        //                int lrow = 0;
        //                int colStart = 0;

        //                var range = workSheet.RangeUsed();
        //                var table = range.AsTable();
        //                lrow = workSheet.LastRowUsed().RowNumber();
        //                int rowcounter = 0;
        //                int rowcontinu = 0;
        //                bool _rowStarted = false;

        //                double divrest = (double)lrow / 1000.0; ///10000 / 1000 = 10 loops
        //                var _divident = (divrest - Math.Truncate(divrest) > 0 ? divrest + 1 : divrest);
        //                int divident = (int)Math.Abs(double.Parse(_divident.ToString()));
        //                var lopcount = 0;

        //                //var starting_row = i * 1000;

        //                foreach (xcel.Excel.IXLRow row in workSheet.Rows())
        //                {

        //                    fcel = row.Cells().First().Address.ColumnNumber;
        //                    lcel = row.Cells().Last().Address.ColumnNumber;

        //                    if (row.Cells(c => c.Address.ToString() == model.HeaderRow).Any())
        //                    {
        //                        foreach (xcel.Excel.IXLCell cell in row.Cells())
        //                        {
        //                            //Validate if Row Name is the beginning from User Set
        //                            var celAddr = $@"{cell.Address.ColumnLetter}{cell.Address.RowNumber}";
        //                            if (model.HeaderRow == celAddr || isHeader == true)
        //                            {
        //                                if (colStart == 0)
        //                                    colStart = cell.Address.ColumnNumber; //Set the Start of Column

        //                                dt.Columns.Add(cell.Value.ToString());
        //                                isHeader = true;
        //                                if (fcel == 0)
        //                                    fcel = cell.Address.ColumnNumber;

        //                            }
        //                        }
        //                    }

        //                    if (row.Cells(c => c.Address.ToString() == model.DataRowStart).Any() || _rowStarted)
        //                        _rowStarted = true;

        //                    if (_rowStarted)
        //                    {
        //                        if (dt.Columns.Count > 0)
        //                        {
        //                            using (DataSet ds = new DataSet())
        //                            {
        //                                int rCnt = 0;
        //                                var drow = dt.NewRow();
        //                                foreach (xcel.Excel.IXLCell cell in row.CellsUsed(xcel.Excel.XLCellsUsedOptions.AllFormats))
        //                                {

        //                                    if (cell.Address.ColumnNumber >= colStart)
        //                                    {
        //                                        if (cell.Address.ColumnNumber >= colStart && cell.Address.ColumnNumber <= dt.Columns.Count)
        //                                        {
        //                                            string newCellValue = cell.Value?.ToString() ?? "";
        //                                            drow[cell.Address.ColumnNumber - colStart] = newCellValue;
        //                                        }
        //                                        else
        //                                            break;
        //                                    }
        //                                }
        //                                dt.Rows.Add(drow);

        //                                rowcounter++;
        //                                rowcontinu++;

        //                                if (rowcounter >= 1000 || ((lopcount == divident) && (rowcontinu == lrow - 1))) /// if (rowcounter == lrow - 1 || dt.Rows.Count >= 1000)
        //                                {
        //                                    lopcount++;
        //                                    rowcounter = 0; ///reset

        //                                    // DataTable dt = new DataTable();

        //                                    ds.Tables.Add(dt);
        //                                    ds.AcceptChanges();

        //                                    var columnData = ds.Tables.Cast<DataTable>()
        //                                       .SelectMany(xtable => xtable.Rows.Cast<DataRow>()
        //                                                    .Select(xrow => xtable.Columns.Cast<DataColumn>()

        //                                                                                  .ToDictionary(column => column.ColumnName,
        //                                                                                                column => xrow[column])))
        //                                         .ToList();



        //                                    var columnList = ds.Tables.Cast<DataTable>()
        //                                      .Select(xtable => xtable.Columns.Cast<DataColumn>().ToList())
        //                                      .ToList();

        //                                    //dt.clear();
        //                                    //dt = new datatable();

        //                                    ds.Clear();
        //                                    dt.Rows.Clear();
        //                                    dt.AcceptChanges();

        //                                    QueryAccess.OPSCreateTable(PrimaryKey, TableName, con, Database, columnList, MapId);
        //                                    QueryAccess.OPSInsertData(TableName, con, Database, columnList, columnData, PrimaryKey, Dest);

        //                                    //if (rowcounter == lrow - 1)
        //                                    //{
        //                                    //    break;
        //                                    //}


        //                                    //ds = new DataSet();

        //                                    //dt.TableName = workSheet.Name.Replace(" ", "");
        //                                    //foreach (var column in columnList)
        //                                    //{
        //                                    //    foreach (var xlist in column)
        //                                    //    {
        //                                    //        dt.Columns.Add(xlist.ToString());
        //                                    //    }
        //                                    //}



        //                                }//End 1000 row


        //                            }//End using
        //                        }//End Column Count
        //                    }//end rowStarted

        //                }//End ForEachRow

        //            }


        //        }
        //        return _bool = true;

        //    }
        //    catch (Exception ex)
        //    {

        //        errorMessage = ex.Message;

        //        return _bool;
        //    }

        //}



      

        public static string OPSCreateSuccessExcel(Dictionary<string, List<string>> SuccessListValue, string DBlocalcon, ExcelMapperViewModel.ExcelMapperModel listmode, int MapId, string LocalPath, out string errorMessage)
        {

			
            try
            {

				using (var workbook = new XLWorkbook())
				{
                  
                    string tableName = $"{MapId}_{listmode.DestinationTable}";
                    string checkTableExistQuery = $@"IF OBJECT_ID('dbo.{tableName}', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0";
                    var worksheet = workbook.Worksheets.Add(listmode.Worksheet); // Create a single worksheet for all data

                    using (var connection = new SqlConnection(DBlocalcon))
                    {
                        connection.Open();
                        using (var command = new SqlCommand(checkTableExistQuery, connection))
                        {
                            int tableExists = (int)command.ExecuteScalar();
                            if (tableExists == 1)
                            {

                                // Adding column headers
                                DataColumnCollection columns = null;
                                int rowIndex = 1;

                                foreach (var Success in SuccessListValue)
                                {
                                    //var MapId = Success[0];
                                    var _PrimaryKeyValue = Success.Key;
                                    var _PrimaryKey = Success.Value[0];

                                    string FetchDataHeader = $@"SELECT * from dbo.[{MapId}_{listmode.DestinationTable}] WHERE {_PrimaryKey} = '{_PrimaryKeyValue}'";


                                    // Fetch data from the database and store it in a DataTable
                                    DataTable dtResult = new DataTable();

                                    //using (var connection = new SqlConnection(DBlocalcon))
                                   // {
                                        using (var adapter = new SqlDataAdapter(FetchDataHeader, connection))
                                        {
                                            adapter.Fill(dtResult);
                                        }
                                    //}

                                    if (dtResult.Rows.Count > 0)
                                    {
                                        // Adding column headers only once, based on the first result
                                        if (columns == null)
                                        {
                                            columns = dtResult.Columns;

                                            for (int colIndex = 1; colIndex <= columns.Count - 2; colIndex++)
                                            {
                                                worksheet.Cell(rowIndex, colIndex).Value = columns[colIndex - 1].ColumnName;
                                            }

                                            worksheet.Cell(rowIndex, worksheet.LastColumnUsed().ColumnNumber() + 1).Value = "Status";

                                            rowIndex++;
                                        }

                                        // Adding data rows
                                        foreach (DataRow row in dtResult.Rows)
                                        {
                                            int columnsCounter = columns.Count - 2;
                                            for (int colIndex = 1; colIndex <= columnsCounter; colIndex++)
                                            {
                                                //worksheet.Cell(rowIndex, colIndex).Value = row[columns[colIndex - 1].ColumnName].ToString();

                                                var cellValue = row[columns[colIndex - 1].ColumnName].ToString();
                                                if (double.TryParse(cellValue, out double numericValue))
                                                {
                                                    // If the cell value can be parsed as a number, set the cell format to treat it as text.
                                                    worksheet.Cell(rowIndex, colIndex).SetValue(cellValue).Style.NumberFormat.Format = "@";
                                                }
                                                else
                                                {
                                                    // If the cell value is not a numeric value, simply set it as is.
                                                    worksheet.Cell(rowIndex, colIndex).Value = cellValue;
                                                }
                                            }

                                            worksheet.Cell(rowIndex, columnsCounter + 1).Value = "Uploaded Successfully";

                                            rowIndex++;
                                        }


                                    }
                                }

                                string DonePath = $"{LocalPath}\\Done\\{DateTime.Now.ToString("MM")}{DateTime.Now.ToString("yyyy")}\\{DateTime.Now.ToString("yyyyMMdd")}";
                                if (!Directory.Exists(DonePath))
                                {
                                    Directory.CreateDirectory(DonePath);
                                }

                                string filePath = $"{DonePath}\\{listmode.DestinationTable}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                                workbook.SaveAs(filePath);
                             
                                
                               

                                errorMessage = "";
                                return filePath;

                            }
                            else
                            {
                                // Handle the case where the table does not exist
                               return errorMessage = $"Table {tableName} Does not Exist";
                            }
                        }
                    }//using new sql

                 
				}//end of using workbook
			}//end of try
            catch (Exception ex)
            {
				errorMessage = ex.Message;

                return errorMessage;
			}

        }

  

        public static string OPSCreateExcelData(string ErrorPath, Dictionary<string, List<string>> ErrrorValueList, string DBlocalcon, ExcelMapperViewModel.ExcelMapperModel listmode, int MapId, out string errorMessage)
        {

            try
            {
				using (var workbook = new XLWorkbook())
				{
					var worksheet = workbook.Worksheets.Add(listmode.Worksheet); // Create a single worksheet for all data


                 
                    // Adding column headers
                    DataColumnCollection columns = null;
					int rowIndex = 1;

					foreach (var Error in ErrrorValueList)
					{
						var _PrimaryKeyValue = Error.Key;
						var _PrimaryKey = Error.Value[0];
						var _ErrorMessage = Error.Value[1];

						string FetchDataHeader = $@"SELECT * from dbo.[{MapId}_{listmode.DestinationTable}] WHERE {_PrimaryKey} = '{_PrimaryKeyValue}'";

						// Fetch data from the database and store it in a DataTable
						DataTable dtResult = new DataTable();
						using (var connection = new SqlConnection(DBlocalcon))
						{
							using (var adapter = new SqlDataAdapter(FetchDataHeader, connection))
							{
								adapter.Fill(dtResult);
							}
						}

						if (dtResult.Rows.Count > 0)
						{
							// Adding column headers only once, based on the first result
							if (columns == null)
							{
								columns = dtResult.Columns;

								for (int colIndex = 1; colIndex <= columns.Count - 2; colIndex++)
								{
									worksheet.Cell(rowIndex, colIndex).Value = columns[colIndex - 1].ColumnName;
								}

								worksheet.Cell(rowIndex, worksheet.LastColumnUsed().ColumnNumber() + 1).Value = "Error Message";

								rowIndex++;
							}

							// Adding data rows
							foreach (DataRow row in dtResult.Rows)
							{
								int columnsCounter = columns.Count - 2;
								for (int colIndex = 1; colIndex <= columnsCounter; colIndex++)
								{
									//worksheet.Cell(rowIndex, colIndex).Value = row[columns[colIndex - 1].ColumnName].ToString();

									var cellValue = row[columns[colIndex - 1].ColumnName].ToString();
									if (double.TryParse(cellValue, out double numericValue))
									{
										// If the cell value can be parsed as a number, set the cell format to treat it as text.
										worksheet.Cell(rowIndex, colIndex).SetValue(cellValue).Style.NumberFormat.Format = "@";
									}
									else
									{
										// If the cell value is not a numeric value, simply set it as is.
										worksheet.Cell(rowIndex, colIndex).Value = cellValue;
									}
								}

								worksheet.Cell(rowIndex, columnsCounter + 1).Value = _ErrorMessage;


								rowIndex++;
							}


						}
					}

                    string CreateErrorPath = $"{ErrorPath}\\{DateTime.Now.ToString("MM")}{DateTime.Now.ToString("yyyy")}\\{DateTime.Now.ToString("yyyyMMdd")}";

                    if (!Directory.Exists(CreateErrorPath))
                    {
                        // Create the directory if it does not exist
                        Directory.CreateDirectory(CreateErrorPath);
                    }

                    string filePath = $"{CreateErrorPath}\\{listmode.DestinationTable}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

					workbook.SaveAs(filePath);
					errorMessage = "";

					return filePath; // You can return the file path or any other information you may need.

				}

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;

				return errorMessage;
			}


			
        }

        public static string OPSCreateExcelDuplicationChecker(string ErrorPath, Dictionary<string, List<string>> ErrrorValueList, string DBlocalcon, int MapId,string TableName)
        {


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1"); // Create a single worksheet for all data

                // Adding column headers
                DataColumnCollection columns = null;
                int rowIndex = 1;

                foreach (var Error in ErrrorValueList)
                {
                    var _PrimaryKeyValue = Error.Key;
                    var _PrimaryKey = Error.Value[0];
                    var _ErrorMessage = Error.Value[1];

                    //string FetchDataHeader = $@"SELECT * from dbo.[{MapId}_Header] WHERE {_PrimaryKey} = '{_PrimaryKeyValue}'";
                    string FetchDataHeader = $@"SELECT * from dbo.[{TableName}] WHERE {_PrimaryKey} = '{_PrimaryKeyValue}'";

                    // Fetch data from the database and store it in a DataTable
                    DataTable dtResult = new DataTable();
                    using (var connection = new SqlConnection(DBlocalcon))
                    {
                        using (var adapter = new SqlDataAdapter(FetchDataHeader, connection))
                        {
                            adapter.Fill(dtResult);
                        }
                    }

                    if (dtResult.Rows.Count > 0)
                    {
                        // Adding column headers only once, based on the first result
                        if (columns == null)
                        {
                            columns = dtResult.Columns;





                            for (int colIndex = 1; colIndex <= columns.Count - 2; colIndex++)
                            {
                                worksheet.Cell(rowIndex, colIndex).Value = columns[colIndex - 1].ColumnName;
                            }

                            worksheet.Cell(rowIndex, worksheet.LastColumnUsed().ColumnNumber() + 1).Value = "Error Message";

                            rowIndex++;
                        }

                        // Adding data rows
                        foreach (DataRow row in dtResult.Rows)
                        {
                            int columnsCounter = columns.Count - 2;
                            for (int colIndex = 1; colIndex <= columnsCounter; colIndex++)
                            {
                                //worksheet.Cell(rowIndex, colIndex).Value = row[columns[colIndex - 1].ColumnName].ToString();

                                var cellValue = row[columns[colIndex - 1].ColumnName].ToString();
                                if (double.TryParse(cellValue, out double numericValue))
                                {
                                    // If the cell value can be parsed as a number, set the cell format to treat it as text.
                                    worksheet.Cell(rowIndex, colIndex).SetValue(cellValue).Style.NumberFormat.Format = "@";
                                }
                                else
                                {
                                    // If the cell value is not a numeric value, simply set it as is.
                                    worksheet.Cell(rowIndex, colIndex).Value = cellValue;
                                }
                            }

                            worksheet.Cell(rowIndex, columnsCounter + 1).Value = _ErrorMessage;


                            rowIndex++;
                        }


                    }
                }

                string _Tablename = TableName.Substring(TableName.IndexOf('_')+1);

                string CreateDuplicateErrPath = $"{ErrorPath}\\{DateTime.Now.ToString("MM")}{DateTime.Now.ToString("yyyy")}\\{DateTime.Now.ToString("yyyyMMdd")}";

                if (!Directory.Exists(CreateDuplicateErrPath))
                {
                    // Create the directory if it does not exist
                    Directory.CreateDirectory(CreateDuplicateErrPath);
                }

                string filePath = $"{CreateDuplicateErrPath}\\{_Tablename}_Duplicate_Entry_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                workbook.SaveAs(filePath);
                return filePath; // You can return the file path or any other information you may need.
            }
        }

        public static DataSet ReadExcelData(string filePath, ExcelMapperViewModel map)
        {
            DataSet ds = new DataSet();
            using (xcel.Excel.XLWorkbook workBook = new xcel.Excel.XLWorkbook(filePath))
            {
                if (map != null)
                {
                    //Read the first Sheet from Excel file.
                    var wscnt = workBook.Worksheets.Count;
                    foreach (var workSheet in workBook.Worksheets)
                    {
                        //Create a new DataTable.
                        DataTable dt = new DataTable();
                        dt.TableName = workSheet.Name;
                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (xcel.Excel.IXLRow row in workSheet.Rows())
                        {
                            //Use the first row to add columns to DataTable.
                            if (firstRow)
                            {
                                foreach (xcel.Excel.IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                if (dt.Columns.Count > 0)
                                {
                                    int i = 0;
                                    if (workSheet.FirstColumnUsed(true) != null)
                                    {
                                        var fcel = workSheet.FirstColumnUsed().FirstCellUsed().Address.ColumnNumber;
                                        var lcel = workSheet.LastColumnUsed().FirstCellUsed().Address.ColumnNumber;
                                        //foreach (xcel.IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                        bool IsEmptyRow = true;
                                        var drow = dt.NewRow();
                                        foreach (xcel.Excel.IXLCell cell in row.Cells())
                                        {
                                            drow[i] = cell.Value.ToString();
                                            //dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                            IsEmptyRow = false;
                                            i++;
                                        }
                                        if (IsEmptyRow == false)
                                            dt.Rows.Add(drow); //Add rows to DataTable
                                    }

                                }
                            }
                        }
                        ds.Tables.Add(dt);
                    }
                }
            }
            return ds;
        }

        public static bool IsColumnExist(DataTable dt, string column)
        {
            try
            {
                var d = dt.AsEnumerable().Select(sel => (string)sel[$@"{column}"]).Any();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #region XCEL_REFERENCE
        //Open the Excel file using ClosedXML.
        //Keep in mind the Excel file cannot be open when trying to read it
        //using (xcel.Excel.XLWorkbook workBook = new xcel.Excel.XLWorkbook(filePath))
        //{
        //    var wscnt = workBook.Worksheets.Count;
        //    foreach (var workSheet in workBook.Worksheets)
        //    {
        //        if (workSheet.FirstColumnUsed(true) != null)
        //        {
        //            var fcel = workSheet.FirstColumnUsed().FirstCellUsed().Address;
        //            var lcel = workSheet.LastColumnUsed().FirstCellUsed().Address;
        //            var ccl = workSheet.Cell(1, 1).DataType == xcel.Excel.XLDataType.Text ? workSheet.Cell(1, 1).Value.ToString() : "";
        //            if (!string.IsNullOrEmpty(ccl))
        //            {
        //                workSheet.Range(fcel, lcel).InsertRowsAbove(1);
        //            }
        //        }                              
        //    }
        //    workBook.Save();
        //}
        #endregion

        public static string GetConnectionString(string server, string database, string username, string password)
        {
            return $"Data Source={server};Initial Catalog={database};Persist Security Info=True;User ID={username};Password={password}";
        }
    }
}