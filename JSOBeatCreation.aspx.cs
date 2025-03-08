using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using OfficeOpenXml;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace Route
{
    public partial class JSOBeatCreation : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
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
                string remoteUser = "G116036";
                //string remoteUser = Request.ServerVariables["REMOTE_USER"];

                if (!string.IsNullOrEmpty(remoteUser))
                {
                    if (remoteUser == Request.ServerVariables["REMOTE_USER"])
                    {
                        Session["name"] = remoteUser.Substring(6);
                    }
                    else
                    {
                        Session["name"] = remoteUser;
                    }

                    if (!string.IsNullOrEmpty(Session["name"]?.ToString()))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        SqlCommand cmd1 = new SqlCommand("SP_Route_SSMType_Beat_Creation", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                        cmd1.Parameters.AddWithValue("@ActionType", "Session");

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

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload_Id.HasFile)
                {
                    string fileExtension = Path.GetExtension(FileUpload_Id.FileName);
                    if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".xlsb")
                    {
                        try
                        {
                            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                            using (var package = new ExcelPackage(FileUpload_Id.FileContent))
                            {
                                var worksheet = package.Workbook.Worksheets[0];
                                var keySheet = package.Workbook.Worksheets["KeySheet"];

                                //string keyValue = keySheet.Cells["A1"].Text.Trim();

                                if (keySheet == null)
                                {
                                    showToast("Invalid file. Please use sample file to upload.", "toast-danger");
                                    return;
                                }

                                string keyValue = keySheet.Cells["A1"]?.Text?.Trim();

                                if (string.IsNullOrEmpty(keyValue) || keyValue != "SYNAPSE")
                                {
                                    showToast("Invalid file. Please use sample file to upload.", "toast-danger");
                                    return;
                                }

                                if (worksheet == null)
                                {
                                    showToast("No worksheet found!", "toast-danger");
                                    return;
                                }
                                                               

                                // Required columns
                                string[] requiredColumns = { "SSMType", "DB CODE", "OLD ROUTE CODE", "OLD ROUTE NAME", "NEW ROUTE CODE", "NEW ROUTE NAME",
                                                 "MnfCode", "RouteType", "RouteCoverage", "Call Days" };

                                if (worksheet.Dimension == null || worksheet.Dimension.End.Row <= 1)
                                {
                                    showToast("The worksheet has no data!", "toast-danger");
                                    return;
                                }

                                // Read header row and check missing columns
                                var headers = new List<string>();
                                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                                {
                                    headers.Add(worksheet.Cells[1, col].Text.Trim());
                                }

                                var missingColumns = requiredColumns.Except(headers).ToList();
                                if (missingColumns.Any())
                                {
                                    showToast($"Missing columns: {string.Join(", ", missingColumns)}", "toast-danger");
                                    return;
                                }

                                dt.Columns.Add("DbCode");
                                dt.Columns.Add("OldRouteCode");
                                dt.Columns.Add("OldRouteName");
                                dt.Columns.Add("NewRouteCode");
                                dt.Columns.Add("NewRouteName");
                                dt.Columns.Add("MnfCode");
                                dt.Columns.Add("RouteType");
                                dt.Columns.Add("RouteCoverage");
                                dt.Columns.Add("CallDays");

                                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                                {
                                    DataRow dr = dt.NewRow();
                                    string ssmType = worksheet.Cells[row, headers.IndexOf("SSMType") + 1].Text.Trim();
                                    string dbcode = worksheet.Cells[row, headers.IndexOf("DB CODE") + 1].Text.Trim();
                                    string oldRouteCode = worksheet.Cells[row, headers.IndexOf("OLD ROUTE CODE") + 1].Text.Trim();
                                    string oldRouteName = worksheet.Cells[row, headers.IndexOf("OLD ROUTE NAME") + 1].Text.Trim();
                                    string newRouteCode = worksheet.Cells[row, headers.IndexOf("NEW ROUTE CODE") + 1].Text.Trim();
                                    string newRouteName = worksheet.Cells[row, headers.IndexOf("NEW ROUTE NAME") + 1].Text.Trim();
                                    string mnf = worksheet.Cells[row, headers.IndexOf("MnfCode") + 1].Text.Trim();
                                    string routeTyp = worksheet.Cells[row, headers.IndexOf("RouteType") + 1].Text.Trim();
                                    string routeCov = worksheet.Cells[row, headers.IndexOf("RouteCoverage") + 1].Text.Trim();
                                    string callDays = worksheet.Cells[row, headers.IndexOf("Call Days") + 1].Text.Trim();
                                                                        
                                    if (!newRouteCode.StartsWith(ssmType + "_") || !newRouteName.StartsWith(ssmType + "_"))
                                    {
                                        showToast("New Route Code and New Route Name must start with " + ssmType + "_", "toast-danger");
                                        return;
                                    }

                                    int mnfCode, routeType, routeCoverage;

                                    if (mnf == "WCCLG")
                                    {
                                        mnfCode = 1;
                                    }
                                    else
                                    {
                                        showToast("Please provide a proper MnfCode", "toast-danger");
                                        return;
                                    }

                                    if (routeTyp == "Van Route")
                                    {
                                        routeType = 1;
                                    }
                                    else if (routeTyp == "Non Van Route")
                                    {
                                        routeType = 2;
                                    }
                                    else
                                    {
                                        showToast("Please provide a proper RouteType", "toast-danger");
                                        return;
                                    }

                                    if (routeCov == "Fortnightly")
                                    {
                                        routeCoverage = 1;
                                    }
                                    else if (routeCov == "Monthly")
                                    {
                                        routeCoverage = 2;
                                    }
                                    else if (routeCov == "Weekly")
                                    {
                                        routeCoverage = 3;
                                    }
                                    else
                                    {
                                        showToast("Please provide a proper RouteCoverage", "toast-danger");
                                        return;
                                    }

                                    dr["DbCode"] = dbcode;
                                    dr["OldRouteCode"] = oldRouteCode;
                                    dr["OldRouteName"] = oldRouteName;
                                    dr["NewRouteCode"] = newRouteCode;
                                    dr["NewRouteName"] = newRouteName;
                                    dr["MnfCode"] = mnfCode;
                                    dr["RouteType"] = routeType;
                                    dr["RouteCoverage"] = routeCoverage;
                                    dr["CallDays"] = callDays;

                                    dt.Rows.Add(dr);
                                }

                                //resdt = dt;
                                resdt = UploadExcel(dt);

                                if(resdt.Rows.Count > 0)
                                {
                                    ResultModalGrid.DataSource = resdt;
                                    ResultModalGrid.DataBind();

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ClickButton", "$('#Button1').click();", true);

                                    showToast("Some of the records uploaded", "toast-success");
                                }
                                else
                                {
                                    showToast("Something went wrong. Please try again.", "toast-danger");
                                }
                                

                            }
                        }
                        catch (Exception ex)
                        {
                            showToast("Error reading file: " + ex.Message, "toast-danger");
                        }
                    }
                    else
                    {
                        showToast("Please select a valid Excel file", "toast-danger");
                    }
                }
                else
                {
                    showToast("Please upload a file", "toast-danger");
                }
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }

        public DataTable UploadExcel(DataTable dt)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                foreach (DataRow row in dt.Rows)
                {
                    using (SqlCommand cmd1 = new SqlCommand("SP_Route_SSMType_Beat_Creation_Newlogic", con))
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                        cmd1.Parameters.AddWithValue("@DistCode", row["DbCode"].ToString());
                        cmd1.Parameters.AddWithValue("@OldRouteCode", row["OldRouteCode"].ToString());
                        cmd1.Parameters.AddWithValue("@OldRouteNm", row["OldRouteName"].ToString());
                        cmd1.Parameters.AddWithValue("@NewRouteCode", row["NewRouteCode"].ToString());
                        cmd1.Parameters.AddWithValue("@NewRouteNm", row["NewRouteName"].ToString());
                        cmd1.Parameters.AddWithValue("@MnfCode", Convert.ToInt32(row["MnfCode"]));
                        cmd1.Parameters.AddWithValue("@RouteType", Convert.ToInt32(row["RouteType"]));
                        cmd1.Parameters.AddWithValue("@RouteCoverage", Convert.ToInt32(row["RouteCoverage"]));
                        cmd1.Parameters.AddWithValue("@CallDays", row["CallDays"].ToString());

                        cmd1.CommandTimeout = 6000;

                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(resdt);
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                showToast("Error inserting data: " + ex.Message, "toast-danger");
            }
            return resdt;
        }


        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }
        #endregion


    }
}