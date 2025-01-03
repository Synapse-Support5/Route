using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using OfficeOpenXml;

namespace Route
{
    public partial class NewGeo : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        public DataSet ds1 = new DataSet();
        bool anyCheckboxSelected = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AccessLoad();
            }
        }

        #region AccessLoad
        public void AccessLoad()
        {
            try
            {
                Session["name"] = "G116036";
                //Session["name"] = Request.ServerVariables["REMOTE_USER"].Substring(6);

                if (Session["name"].ToString() != "")
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd1 = new SqlCommand("SP_Route_NewGeo", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "Session");
                    cmd1.Parameters.AddWithValue("@DistCode", "");
                    cmd1.Parameters.AddWithValue("@RouteCode", "");
                    cmd1.Parameters.AddWithValue("@RtrCode", "");
                    cmd1.Parameters.AddWithValue("@RtrNm", "");
                    cmd1.Parameters.AddWithValue("@Cls1Desc", "");
                    cmd1.Parameters.AddWithValue("@Cls2Desc", "");
                    cmd1.Parameters.AddWithValue("@Cls3Desc", "");
                    cmd1.Parameters.AddWithValue("@RtrAdd1", "");
                    cmd1.Parameters.AddWithValue("@RtrAdd2", "");
                    cmd1.Parameters.AddWithValue("@RtrAdd3", "");
                    cmd1.Parameters.AddWithValue("@RtrPhone", "");
                    cmd1.Parameters.AddWithValue("@RtrTaxType", "");
                    cmd1.Parameters.AddWithValue("@RtrGSTNO", "");
                    cmd1.Parameters.AddWithValue("@RouteName", "");
                    cmd1.Parameters.AddWithValue("@MnfCode", "");
                    cmd1.Parameters.AddWithValue("@RtType", "");
                    cmd1.Parameters.AddWithValue("@RtCoverage", "");

                    cmd1.CommandTimeout = 6000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(resdt);

                    if (resdt.Rows.Count > 0)
                    {
                        //lblUserName.Text = "User Name > " + resdt.Rows[0][0].ToString() + ": User ID > " + Session["name"].ToString();
                        lblUserName.Text = "Welcome : " + resdt.Rows[0][0].ToString();
                        hdnBusinessType.Value = resdt.Rows[0][2].ToString();
                        hdnRole.Value = resdt.Rows[0][3].ToString();

                    }
                    else
                    {
                        Response.Redirect("AccessDeniedPage.aspx");
                    }
                    con.Close();
                }
                else
                {
                    Response.Redirect("AccessDeniedPage.aspx");
                }
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region BtnEnter_Click
        protected void BtnEnter_Click(object sender, EventArgs e)
        {
            Enter_Click();
        }

        public void Enter_Click()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_NewGeo", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "DistExistCheck");
                cmd1.Parameters.AddWithValue("@DistCode", DistCodeTxt.Text);
                cmd1.Parameters.AddWithValue("@RouteCode", "");
                cmd1.Parameters.AddWithValue("@RtrCode", "");
                cmd1.Parameters.AddWithValue("@RtrNm", "");
                cmd1.Parameters.AddWithValue("@Cls1Desc", "");
                cmd1.Parameters.AddWithValue("@Cls2Desc", "");
                cmd1.Parameters.AddWithValue("@Cls3Desc", "");
                cmd1.Parameters.AddWithValue("@RtrAdd1", "");
                cmd1.Parameters.AddWithValue("@RtrAdd2", "");
                cmd1.Parameters.AddWithValue("@RtrAdd3", "");
                cmd1.Parameters.AddWithValue("@RtrPhone", "");
                cmd1.Parameters.AddWithValue("@RtrTaxType", "");
                cmd1.Parameters.AddWithValue("@RtrGSTNO", "");
                cmd1.Parameters.AddWithValue("@RouteName", "");
                cmd1.Parameters.AddWithValue("@MnfCode", "");
                cmd1.Parameters.AddWithValue("@RtType", "");
                cmd1.Parameters.AddWithValue("@RtCoverage", "");

                SqlDataReader reader = cmd1.ExecuteReader();
                string response = string.Empty;

                if (reader.Read())
                {
                    response = reader["Response"].ToString();
                }
                reader.Close();

                if (response == "Ready To Proceed")
                {
                    btnDivSingle.Visible = false;
                    btnDivSplit.Visible = true;
                    btnDivSplit2.Visible = true;
                }
                else
                {
                    btnDivSingle.Visible = true;
                    btnDivSplit.Visible = false;
                    btnDivSplit2.Visible = false;

                    showToast(response, "toast-danger");
                    return;
                }

                con.Close();

            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Submit_Click
        protected void Submit_Click(object sender, EventArgs e)
        {
            BtnSubmit();
        }

        public void BtnSubmit()
        {
            try
            {
                if (FileUpload_Id.HasFile)
                {
                    string fileExtension = Path.GetExtension(FileUpload_Id.FileName);
                    if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".xlsb")
                    {
                        DataTable dtExcel = GetDataFromExcel(FileUpload_Id.FileContent);

                        if (dtExcel != null && dtExcel.Rows.Count > 0)
                        {
                            con.Open();
                            foreach (DataRow row in dtExcel.Rows)
                            {
                                string cls1Desc = row["OUTLET CLASSIFICATION 1"].ToString();

                                // Get existing data for comparison
                                using (SqlCommand cmd = new SqlCommand("SP_Route_NewGeo", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@ActionType", "GetCls1Desc");
                                    cmd.Parameters.AddWithValue("@DistCode", "");
                                    cmd.Parameters.AddWithValue("@RouteCode", "");
                                    cmd.Parameters.AddWithValue("@RtrCode", "");
                                    cmd.Parameters.AddWithValue("@RtrNm", "");
                                    cmd.Parameters.AddWithValue("@Cls1Desc", cls1Desc);
                                    cmd.Parameters.AddWithValue("@Cls2Desc", "");
                                    cmd.Parameters.AddWithValue("@Cls3Desc", "");
                                    cmd.Parameters.AddWithValue("@RtrAdd1", "");
                                    cmd.Parameters.AddWithValue("@RtrAdd2", "");
                                    cmd.Parameters.AddWithValue("@RtrAdd3", "");
                                    cmd.Parameters.AddWithValue("@RtrPhone", "");
                                    cmd.Parameters.AddWithValue("@RtrTaxType", "");
                                    cmd.Parameters.AddWithValue("@RtrGSTNO", "");
                                    cmd.Parameters.AddWithValue("@RouteName", "");
                                    cmd.Parameters.AddWithValue("@MnfCode", "");
                                    cmd.Parameters.AddWithValue("@RtType", "");
                                    cmd.Parameters.AddWithValue("@RtCoverage", "");

                                    var existingData = cmd.ExecuteScalar()?.ToString() ?? string.Empty;

                                    if (existingData == string.Empty)
                                    {
                                        existingData = "Not Exists";
                                    }
                                    else
                                    {
                                        existingData = cmd.ExecuteScalar()?.ToString() ?? string.Empty;
                                    }

                                    row["ExistingData"] = existingData;

                                    // Set MatchStatus based on comparison
                                    row["MatchStatus"] = cls1Desc.Equals(existingData, StringComparison.OrdinalIgnoreCase) ? 0 : 1;
                                }
                            }

                            con.Close();

                            // Bind the DataTable to the GridView here
                            GridView1.DataSource = dtExcel;
                            GridView1.DataBind();

                            // Store the DataTable in session to use later on continue
                            Session["UploadedData"] = dtExcel;

                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoteAlert", "showNoteAlert();", true);
                        }
                        else
                        {
                            showToast("No data found in the Excel file", "toast-danger");
                        }
                    }
                    else
                    {
                        showToast("Please select a valid Excel file", "toast-danger");
                    }
                }
                else
                {
                    showToast("Please select a file", "toast-danger");
                }
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
            finally
            {
                // Hide the loader at the end
                //ScriptManager.RegisterStartupScript(this, GetType(), "hideLoader", "hideLoader();", true);
            }
        }
        #endregion

        #region GetDataFromExcel
        private DataTable GetDataFromExcel(Stream fileStream)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            DataTable dt = new DataTable(); 

            try
            {
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    // Check if the sheets exist
                    ExcelWorksheet retailerSheet = package.Workbook.Worksheets["Retailer"];
                    ExcelWorksheet routeSheet = package.Workbook.Worksheets["Route"];
                    ExcelWorksheet distributorInfoSheet = package.Workbook.Worksheets["Distributor Info"];
                    //ExcelWorksheet restrictedSheet = package.Workbook.Worksheets["Restricted"];

                    var sheet = package.Workbook.Worksheets["Restricted"];
                    if (sheet != null)
                    {
                        string identifier = sheet.Cells["A1"].Text;

                        if (identifier == "SampleFile")
                        {
                            showToast("You cannot upload the sample file", "toast-danger");
                            return null;
                        }
                    }

                    // Check if Distributor Info sheet exists
                    string distributorSAPCode = string.Empty;
                    if (distributorInfoSheet != null)
                    {
                        // Find the cell containing "Distributor SAP Code"
                        for (int row = 1; row <= distributorInfoSheet.Dimension.End.Row; row++)
                        {
                            for (int col = 1; col <= distributorInfoSheet.Dimension.End.Column; col++)
                            {
                                if (distributorInfoSheet.Cells[row, col].Text == "Distributor SAP Code")
                                {
                                    // Get the Distcode from the adjacent cell
                                    distributorSAPCode = distributorInfoSheet.Cells[row, col + 2].Text;
                                    break;
                                }
                            }

                            if (!string.IsNullOrEmpty(distributorSAPCode))
                                break;
                        }
                    }

                    if (retailerSheet == null || routeSheet == null)
                    {
                        showToast("The Excel file must contain Retailer and Route sheets", "toast-danger");
                        return null;
                    }

                    // Check if the sheets are empty
                    if (retailerSheet.Dimension == null || routeSheet.Dimension == null)
                    {
                        showToast("Both Retailer and Route sheets must have data and a header row", "toast-danger");
                        return null;
                    }

                    // Validate the header rows for both sheets
                    if (!IsHeaderValid(retailerSheet) || !IsHeaderValid(routeSheet))
                    {
                        showToast("Both Retailer and Route sheets must have a non-empty header row", "toast-danger");
                        return null;
                    }

                    // Validate if the header rows have the required columns
                    Dictionary<string, int> retailerColumns = GetHeaderIndices(retailerSheet);
                    Dictionary<string, int> routeColumns = GetHeaderIndices(routeSheet);

                    if (!retailerColumns.ContainsKey("ROUTE CODE") || !retailerColumns.ContainsKey("RETAILER NAME"))
                    {
                        showToast("The Retailer sheet must contain ROUTE CODE and RETAILER NAME columns", "toast-danger");
                        return null;
                    }

                    if (!routeColumns.ContainsKey("ROUTE CODE"))
                    {
                        showToast("The Route sheet must contain RouteCode column", "toast-danger");
                        return null;
                    }

                    // Add columns to the DataTable
                    foreach (var columnName in retailerColumns.Keys)
                    {
                        dt.Columns.Add(columnName);
                    }
                    dt.Columns.Add("ExistingData", typeof(string));
                    dt.Columns.Add("MatchStatus", typeof(int)); // 0 if same, 1 if different
                    dt.Columns.Add("ROUTE NAME", typeof(string)); // New column for ROUTENAME
                    dt.Columns.Add("SSM CODE", typeof(string)); 
                    dt.Columns.Add("SSM NAME", typeof(string)); 
                    dt.Columns.Add("COMPANYCODE", typeof(string));
                    dt.Columns.Add("LOCALUPCOUNTRY", typeof(string));
                    dt.Columns.Add("FREQUENCY", typeof(string)); 
                    dt.Columns.Add("VANNONVAN", typeof(string));
                    dt.Columns.Add("Distributor SAP Code", typeof(string));

                    // Validate if the sheets have data after the header
                    if (retailerSheet.Dimension.Rows < 2 && routeSheet.Dimension.Rows < 2)
                    {
                        showToast("Both Retailer and Route sheets must have data rows", "toast-danger");
                        return null;
                    }

                    // Read data from the 'Retailer' sheet
                    var retailerData = ReadSheetData(retailerSheet, retailerColumns);

                    // Read data from the 'Route' sheet
                    var routeData = ReadSheetData(routeSheet, routeColumns);

                    // Validate that Route data contains RouteCodes matching Retailer sheet
                    var retailerRouteCodes = retailerData.Rows.Cast<DataRow>().Select(row => row["ROUTE CODE"].ToString()).Distinct();
                    var routeRouteCodes = routeData.Rows.Cast<DataRow>().Select(row => row["ROUTE CODE"].ToString()).Distinct();

                    if (!retailerRouteCodes.All(code => routeRouteCodes.Contains(code)))
                    {
                        showToast("The Route sheet must contain all ROUTE CODE values from the Retailer sheet", "toast-danger");
                        return null;
                    }

                    // Merge Route data into the DataTable
                    foreach (DataRow retailerRow in retailerData.Rows)
                    {
                        var routeCode = retailerRow["ROUTE CODE"].ToString();
                        var matchingRouteRow = routeData.AsEnumerable().FirstOrDefault(r => r["ROUTE CODE"].ToString() == routeCode);

                        var newRow = dt.NewRow();
                        foreach (var columnName in retailerColumns.Keys)
                        {
                            newRow[columnName] = retailerRow[columnName];
                        }

                        // Add route-specific data
                        newRow["ROUTE NAME"] = matchingRouteRow?["ROUTE NAME"] ?? DBNull.Value;
                        newRow["SSM CODE"] = matchingRouteRow?["SSM CODE"] ?? DBNull.Value;
                        newRow["SSM NAME"] = matchingRouteRow?["SSM NAME"] ?? DBNull.Value;
                        newRow["COMPANYCODE"] = matchingRouteRow?["COMPANYCODE"] ?? DBNull.Value;
                        newRow["LOCALUPCOUNTRY"] = matchingRouteRow?["LOCALUPCOUNTRY"] ?? DBNull.Value;
                        newRow["FREQUENCY"] = matchingRouteRow?["FREQUENCY"] ?? DBNull.Value;
                        newRow["VANNONVAN"] = matchingRouteRow?["VANNONVAN"] ?? DBNull.Value;

                        newRow["Distributor SAP Code"] = distributorSAPCode;

                        dt.Rows.Add(newRow);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                showToast("An error occurred while reading the Excel file: " + ex.Message, "toast-danger");
                return null;
            }
        }

        // Helper method to validate the header row
        private bool IsHeaderValid(ExcelWorksheet sheet)
        {
            if (sheet.Dimension == null || sheet.Dimension.Rows < 1)
                return false;

            for (int col = 1; col <= sheet.Dimension.Columns; col++)
            {
                var cellValue = sheet.Cells[1, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(cellValue))
                {
                    return true; // Found at least one non-empty header cell
                }
            }
            return false; // All header cells are empty or null
        }

        // Helper method to read headers and column indices
        private Dictionary<string, int> GetHeaderIndices(ExcelWorksheet sheet)
        {
            var headerIndices = new Dictionary<string, int>();

            for (int col = 1; col <= sheet.Dimension.Columns; col++)
            {
                string header = sheet.Cells[1, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(header) && !headerIndices.ContainsKey(header))
                {
                    headerIndices[header] = col;
                }
            }

            return headerIndices;
        }

        // Helper method to read data from a sheet into a DataTable
        private DataTable ReadSheetData(ExcelWorksheet sheet, Dictionary<string, int> columnIndices)
        {
            DataTable table = new DataTable();

            foreach (var column in columnIndices.Keys)
            {
                table.Columns.Add(column);
            }

            for (int row = 2; row <= sheet.Dimension.Rows; row++)
            {
                if (sheet.Cells[row, 2].Value == null) continue;

                var newRow = table.NewRow();
                foreach (var column in columnIndices)
                {
                    newRow[column.Key] = sheet.Cells[row, column.Value].Value;
                }
                table.Rows.Add(newRow);
            }

            return table;
        }

        #endregion

        #region GridView1_RowDataBound
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the MatchStatus value for the current row
                int matchStatus = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "MatchStatus"));

                // If MatchStatus is 0, set the text color of the "ExistingData" cell to red
                if (matchStatus == 1)
                {
                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        #endregion

        #region btnContinue_Click
        protected void btnContinue_Click(object sender, EventArgs e)
        {
            DataTable dtExcel = Session["UploadedData"] as DataTable;
            if (dtExcel == null)
            {
                showToast("No data available to process.", "toast-danger");
                return;
            }

            string distributorSapCodeFromExcel = dtExcel.Rows[0]["Distributor SAP Code"].ToString();

            if (distributorSapCodeFromExcel != DistCodeTxt.Text)
            {
                showToast("The entered DistCode does not match the Distributor SAP Code from the uploaded Excel file", "toast-danger");
                return;
            }

            foreach (DataRow row in dtExcel.Rows)
            {
                if (row["URCODE"].ToString() != "")
                {
                    showToast("URCODE field should be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["RETAILERCODE"]?.ToString()))
                {
                    showToast("RETAILERCODE field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["RETAILER NAME"]?.ToString()))
                {
                    showToast("RETAILER NAME field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["ROUTE CODE"]?.ToString()))
                {
                    showToast("ROUTE CODE field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["CLASSIFICATION CODE"]?.ToString()))
                {
                    showToast("CLASSIFICATION CODE field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["OUTLET CLASSIFICATION 2"]?.ToString()))
                {
                    showToast("OUTLET CLASSIFICATION 2 field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["OUTLET CLASSIFICATION 3"]?.ToString()))
                {
                    showToast("OUTLET CLASSIFICATION 3 field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["TAXTYPE"]?.ToString()))
                {
                    showToast("TAXTYPE field should not be empty", "toast-danger");
                    return;
                }

                //if (string.IsNullOrWhiteSpace(row["RETAILER GSTTIN"]?.ToString()))
                //{
                //    showToast("RETAILER GSTTIN field should not be empty", "toast-danger");
                //    return;
                //}

                if (string.IsNullOrWhiteSpace(row["ROUTE NAME"]?.ToString()))
                {
                    showToast("ROUTE NAME field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["SSM CODE"]?.ToString()))
                {
                    showToast("SSM CODE field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["SSM NAME"]?.ToString()))
                {
                    showToast("SSM NAME field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["COMPANYCODE"]?.ToString()))
                {
                    showToast("COMPANYCODE field should not be empty", "toast-danger");
                    return;
                }

                //if (string.IsNullOrWhiteSpace(row["LOCALUPCOUNTRY"]?.ToString()))
                //{
                //    showToast("LOCALUPCOUNTRY field should not be empty", "toast-danger");
                //    return;
                //}

                if (string.IsNullOrWhiteSpace(row["FREQUENCY"]?.ToString()))
                {
                    showToast("FREQUENCY field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["VANNONVAN"]?.ToString()))
                {
                    showToast("VANNONVAN field should not be empty", "toast-danger");
                    return;
                }

                if (string.IsNullOrWhiteSpace(row["ExistingData"]?.ToString()) || row["ExistingData"].ToString() == "Not Exists")
                {
                    string Classification1 = row["OUTLET CLASSIFICATION 1"].ToString();

                    showToast("Classification 1 : " + Classification1 + " contains invalid data please correct it and try again", "toast-danger");
                    return;
                }
            }

            try
            {
                con.Open();

                foreach (DataRow row in dtExcel.Rows)
                {
                    row["OUTLET CLASSIFICATION 1"] = row["ExistingData"]; // Replace with ExistingData

                    //using (SqlCommand cmd = new SqlCommand("SP_Route_NewGeo", con))
                    //{
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    //    cmd.Parameters.AddWithValue("@ActionType", "Submit_Click");
                    //    cmd.Parameters.AddWithValue("@DistCode", DistCodeTxt.Text);
                    //    cmd.Parameters.AddWithValue("@RouteCode", row["MARKET CODE"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtrCode", row["RETAILERCODE"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtrNm", row["RETAILER NAME"].ToString());//
                    //    cmd.Parameters.AddWithValue("@Cls1Desc", row["OUTLET CLASSIFICATION 1"].ToString());//
                    //    cmd.Parameters.AddWithValue("@Cls2Desc", row["OUTLET CLASSIFICATION 2"].ToString());//
                    //    cmd.Parameters.AddWithValue("@Cls3Desc", row["OUTLET CLASSIFICATION 3"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtrAdd1", row["RETAILER ADD1"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtrAdd2", row["RETAILER ADD2"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtrAdd3", row["RETAILER ADD3"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtrPhone", row["PHONEOFF"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtrTaxType", row["TAXTYPE"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtrGSTNO", row["RETAILER GSTTIN"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RouteName", row["ROUTE NAME"].ToString());//
                    //    cmd.Parameters.AddWithValue("@MnfCode", row["COMPANYCODE"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtType", row["VANNONVAN"].ToString());//
                    //    cmd.Parameters.AddWithValue("@RtCoverage", row["FREQUENCY"].ToString());//

                    //    cmd.ExecuteNonQuery();
                    //}
                }

                using (SqlCommand cmd = new SqlCommand("SP_Route_NewGeo_NewLogic", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd.Parameters.AddWithValue("@DistCode", DistCodeTxt.Text);
                    cmd.Parameters.AddWithValue("@NewGeoetails", dtExcel);
                    cmd.CommandTimeout = 600000;
                    cmd.ExecuteNonQuery();
                }

                showToast("Data processed successfully!", "toast-success");

                ClearForm();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }
        #endregion

        #region ClearForm
        public void ClearForm()
        {
            DistCodeTxt.Text = string.Empty;

            btnDivSingle.Visible = true;
            btnDivSplit.Visible = false;
            btnDivSplit2.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();

        }
        #endregion

    }
}