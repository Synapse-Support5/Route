using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using OfficeOpenXml;
using System.IO;

namespace Route
{
    public partial class Map : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        DataSet ds1 = new DataSet();
        bool anyCheckboxSelected = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AccessLoad();
                DistErpLoad();
            }
        }

        #region AccessLoad
        public void AccessLoad()
        {
            try
            {
                //Session["name"] = "G116036";
                ////Session["name"] = Request.ServerVariables["REMOTE_USER"].Substring(6);

                //if (Session["name"].ToString() != "")
                //{
                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                //SqlCommand cmd1 = new SqlCommand("SP_Route_Map_Dropdowns", con);
                //cmd1.CommandType = CommandType.StoredProcedure;
                //cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                //cmd1.Parameters.AddWithValue("@ActionType", "Session");
                //cmd1.Parameters.AddWithValue("@DistCode", "");

                //cmd1.CommandTimeout = 6000;
                //SqlDataAdapter da = new SqlDataAdapter(cmd1);
                //da.Fill(resdt);

                //if (resdt.Rows.Count > 0)
                //{
                //    //lblUserName.Text = "User Name > " + resdt.Rows[0][0].ToString() + ": User ID > " + Session["name"].ToString();
                //    lblUserName.Text = "Welcome : " + resdt.Rows[0][0].ToString();
                //    hdnBusinessType.Value = resdt.Rows[0][2].ToString();
                //    hdnRole.Value = resdt.Rows[0][3].ToString();
                //}
                //else
                //{
                //    Response.Redirect("AccessDeniedPage.aspx");
                //}
                //con.Close();
                //}
                //else
                //{
                //    Response.Redirect("AccessDeniedPage.aspx");
                //}

                //string remoteUser = "G116036";
                string remoteUser = Request.ServerVariables["REMOTE_USER"];

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
                        SqlCommand cmd1 = new SqlCommand("SP_Route_Map_Dropdowns", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                        cmd1.Parameters.AddWithValue("@ActionType", "Session");
                        cmd1.Parameters.AddWithValue("@DistCode", "");

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

        #region SelectedIndexChanged
        protected void DistDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            RouteLoadGridView.DataSource = null;
            RouteLoadGridView.DataBind();

            RouteLoad();
            DistCodeLoadGrid();

            DistSearch.Value = DistDrp.SelectedItem.ToString();
        }

        protected void RouteDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistCodeLoadGridView.DataSource = null;
            DistCodeLoadGridView.DataBind();

            RtrLoad();
            RouteLoadGrid();

            RouteSearch.Value = RouteDrp.SelectedItem.ToString();
        }
        #endregion

        #region DistErpLoad
        public void DistErpLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Map_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "DistLoad");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                DistDrp.DataSource = resdt;
                DistDrp.DataTextField = resdt.Columns["DistNm"].ToString();
                DistDrp.DataValueField = resdt.Columns["DistCode"].ToString();
                DistDrp.DataBind();
                DistDrp.Items.Insert(0, new ListItem("Dist. Code", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region DistCodeLoadGridView
        public void DistCodeLoadGrid()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Map_LoadGrid", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "DistCode");
                cmd1.Parameters.AddWithValue("@Distcode", DistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@RouteId", "");
                cmd1.CommandTimeout = 6000;
                cmd1.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                DistCodeLoadGridView.DataSource = resdt;
                DistCodeLoadGridView.DataBind();
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region RouteLoadGridView
        public void RouteLoadGrid()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Map_LoadGrid", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ActionType", "Route");
                cmd1.Parameters.AddWithValue("@Distcode", DistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@RouteId", RouteDrp.SelectedValue);
                cmd1.CommandTimeout = 6000;
                cmd1.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                RouteLoadGridView.DataSource = resdt;
                RouteLoadGridView.DataBind();
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region RtrLoad
        public void RtrLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Map_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RtrLoad");
                cmd1.Parameters.AddWithValue("@DistCode", DistDrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                if (resdt.Rows.Count > 0)
                {
                    RtrId.DataSource = resdt;
                    RtrId.DataBind();
                }
                else
                {
                    RtrId.DataSource = null;
                    RtrId.DataBind();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region MappedRtrFontColor
        protected void RtrId_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int selectedValue = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Selected"));

                if (selectedValue == 1)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        #endregion

        #region RouteLoad
        public void RouteLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Map_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RouteLoad");
                cmd1.Parameters.AddWithValue("@DistCode", DistDrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                RouteDrp.DataSource = resdt;
                RouteDrp.DataTextField = resdt.Columns["RouteName"].ToString();
                RouteDrp.DataValueField = resdt.Columns["RouteId"].ToString();
                RouteDrp.DataBind();
                RouteDrp.Items.Insert(0, new ListItem("Route", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region EnterBtn_Click
        protected void EnterBtn_Click(object sender, EventArgs e)
        {
            EnterBtnRtrModal();
        }

        public void EnterBtnRtrModal()
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
                            // Get RtrId column from the Excel data
                            var excelIds = dtExcel.AsEnumerable().Select(row => row["RtrCode"].ToString()).ToHashSet();

                            // Loop through GridView rows
                            foreach (GridViewRow row in RtrId.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    // Retrieve the RtrCode value from the hidden BoundField
                                    string gridRtrCode = RtrId.DataKeys[row.RowIndex]["RtrCode"].ToString();

                                    // Find the checkbox in the row
                                    HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");

                                    // Check the checkbox if the RtrId matches
                                    if (chkBox != null && excelIds.Contains(gridRtrCode))
                                    {
                                        chkBox.Checked = true;
                                    }
                                }
                            }
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

        private DataTable GetDataFromExcel(Stream fileStream)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            DataTable dt = new DataTable();

            try
            {
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    // Get the first worksheet in the Excel file
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Add generic columns to the DataTable
                    for (int col = 1; col <= colCount; col++)
                    {
                        string columnName = "RtrCode";
                        //if (string.IsNullOrEmpty(columnName))
                        //{
                        //    columnName = $"RtrCode";
                        //}
                        dt.Columns.Add(columnName);
                    }

                    // Add rows to the DataTable
                    for (int row = 1; row <= rowCount; row++) // Read all rows, starting from the first row
                    {
                        DataRow dataRow = dt.NewRow();
                        for (int col = 1; col <= colCount; col++)
                        {
                            dataRow[col - 1] = worksheet.Cells[row, col].Text.Trim();
                        }
                        dt.Rows.Add(dataRow);
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
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BtnSubmit();
        }
        #endregion

        #region RouteMappingsGridView_RowCommand for L1L2
        protected void RouteMappingsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UnMap")
            {
                int unMapRouteId = Convert.ToInt32(e.CommandArgument);
                Session["unMapRouteId"] = unMapRouteId;
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoteAlert2", "showNoteAlert2();", true);

                RouteMappingsGridView.DataSource = null;
                RouteMappingsGridView.DataBind();
            }
        }
        #endregion

        #region BtnSubmit
        public void BtnSubmit()
        {
            try
            {
                string distCode = DistDrp.SelectedValue;
                string route = RouteDrp.SelectedValue;
                List<int> checkedRtr = new List<int>();

                foreach (GridViewRow row in RtrId.Rows)
                {
                    HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                    if (chkBox != null && chkBox.Checked)
                    {
                        int rtrId = Convert.ToInt32(RtrId.DataKeys[row.RowIndex].Value);
                        checkedRtr.Add(rtrId);
                        anyCheckboxSelected = true;
                    }
                }

                if (distCode == "")
                {
                    showToast("Please select Dist. Code", "toast-danger");
                    return;
                }
                else if (!anyCheckboxSelected)
                {
                    showToast("Please select any Retailer", "toast-danger");
                    return;
                }
                else if (route == "")
                {
                    showToast("Please select Route", "toast-danger");
                    return;
                }

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                foreach (int rtrId in checkedRtr)
                {
                    SqlCommand cmd = new SqlCommand("SP_Route_Map_MapBtn", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User", Session["name"].ToString());
                    cmd.Parameters.AddWithValue("@RtrId", rtrId);
                    cmd.Parameters.AddWithValue("@RouteId", route);
                    cmd.Parameters.AddWithValue("@Distcode", distCode);
                    //cmd.Parameters.AddWithValue("@InActiveL1L2", "");

                    // Check if there's an unMapRouteId in session
                    if (Session["unMapRouteId"] != null)
                    {
                        int unMapRouteId = Convert.ToInt32(Session["unMapRouteId"]);
                        cmd.Parameters.AddWithValue("@InActiveL1L2", unMapRouteId);
                        Session.Remove("unMapRouteId"); // Remove session after use
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@InActiveL1L2", ""); // or whatever default value
                    }
                    cmd.CommandTimeout = 6000;
                    //cmd.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    ds1.Clear();
                    da.Fill(ds1);

                    if (ds1.Tables.Count > 1)
                    {
                        string responseMessage = ds1.Tables[1].Rows[0]["Response"].ToString();
                        if (responseMessage.StartsWith("Maximum routes reached"))
                        {
                            DataTable dt = new DataTable();
                            using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                            {
                                da2.Fill(dt);
                            }

                            if (dt.Rows.Count > 0)
                            {
                                RouteMappingsGridView.DataSource = dt;
                                RouteMappingsGridView.DataBind();

                                ScriptManager.RegisterStartupScript(this, GetType(), "showNoteAlert", "showNoteAlert();", true);
                                Session["pausedRtrId"] = rtrId;
                                return;
                            }
                        }
                    }
                }

                showToast("Route Mapped Successfully", "toast-success");

                con.Close();

                ClearForm();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region ClearForm
        public void ClearForm()
        {
            DistDrp.ClearSelection();
            DistSearch.Value = string.Empty;
            RouteSearch.Value = string.Empty;

            foreach (GridViewRow row in RtrId.Rows)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                if (chk != null)
                {
                    chk.Checked = false;
                }
            }
            RouteDrp.ClearSelection();
            ScriptManager.RegisterStartupScript(this, GetType(), "hideNoteAlert", "hideNoteAlert();", true);

            RouteLoadGridView.DataSource = null;
            RouteLoadGridView.DataBind();
        }
        #endregion

        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }
        #endregion


    }
}