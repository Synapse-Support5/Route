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
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using Antlr.Runtime.Misc;

namespace Route
{
    public partial class Create : System.Web.UI.Page
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
                if (Session["ResponseMessage"] != null)
                {
                    string responseMessage = Session["ResponseMessage"].ToString();
                    string responseStatus = Session["ResponseStatus"].ToString();

                    if (responseStatus == "Success")
                    {
                        showToast(responseMessage, "toast-success");
                    }
                    else
                    {
                        showToast(responseMessage, "toast-danger");
                    }
                }

                AccessLoad();
                DistErpLoad();
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
                    SqlCommand cmd1 = new SqlCommand("SP_Route_Create_Dropdowns", con);
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
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region SelectedIndexChanged
        protected void DistDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string distCode = DistDrp.SelectedValue;

            if (!string.IsNullOrEmpty(distCode))
            {
                BindGridView(distCode);
            }

            MnfIdLoad();
        }

        protected void MnfIdDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            RtTypeLoad();
        }

        protected void RtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RtCoverageLoad();
        }

        protected void RtCoverage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWeekdays();
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
                SqlCommand cmd1 = new SqlCommand("SP_Route_Create_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "DistLoad");
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

        #region GridView for Existing DistCode
        private void BindGridView(string distCode)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd1 = new SqlCommand("SP_Route_Create_LoadGrid", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@DistCode", distCode);
                cmd1.CommandTimeout = 6000;
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                if (resdt.Rows.Count > 0)
                {
                    RouteGridView.DataSource = resdt;
                    RouteGridView.DataBind();
                }
                else
                {
                    RouteGridView.DataSource = null;
                    RouteGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }

        }
        #endregion

        #region MnfIdLoad
        public void MnfIdLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Create_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "MnfId");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                MnfId.DataSource = resdt;
                MnfId.DataTextField = resdt.Columns["MnfCde"].ToString();
                MnfId.DataValueField = resdt.Columns["MnfId"].ToString();
                MnfId.DataBind();
                MnfId.Items.Insert(0, new ListItem("MNF Code", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }

        }
        #endregion

        #region RtTypeLoad
        public void RtTypeLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Create_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RtType");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                RtType.DataSource = resdt;
                RtType.DataTextField = resdt.Columns["RouteTypeName"].ToString();
                RtType.DataValueField = resdt.Columns["RouteTypeId"].ToString();
                RtType.DataBind();
                RtType.Items.Insert(0, new ListItem("Route Type", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }

        }
        #endregion

        #region RtCoverage
        public void RtCoverageLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Create_Dropdowns", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RtCoverage");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                RtCoverage.DataSource = resdt;
                RtCoverage.DataTextField = resdt.Columns["CoverageName"].ToString();
                RtCoverage.DataValueField = resdt.Columns["CoverageId"].ToString();
                RtCoverage.DataBind();
                RtCoverage.Items.Insert(0, new ListItem("Route Coverage", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }

        }
        #endregion

        #region Call Days
        private void BindWeekdays()
        {
            var weekdays = new List<Weekday>
            {
                new Weekday { DayName = "Sunday" },
                new Weekday { DayName = "Monday" },
                new Weekday { DayName = "Tuesday" },
                new Weekday { DayName = "Wednesday" },
                new Weekday { DayName = "Thursday" },
                new Weekday { DayName = "Friday" },
                new Weekday { DayName = "Saturday" }
            };

            DayId.DataSource = weekdays;
            DayId.DataBind();
        }

        public class Weekday
        {
            public string DayName { get; set; }
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BtnSubmit();
        }
        #endregion

        #region BtnSubmit
        public void BtnSubmit()
        {
            try
            {
                string distId = DistDrp.SelectedValue;
                string rtCode = RtCode.Text;
                string rtName = RtName.Text;
                string mnfId = MnfId.SelectedValue;
                string rtType = RtType.SelectedValue;
                string rtCoverage = RtCoverage.SelectedValue;
                List<string> checkedDays = new List<string>();

                GridViewRow headerRow = DayId.HeaderRow;
                if (headerRow != null)
                {
                    HtmlInputCheckBox selectAllCheckBox = (HtmlInputCheckBox)headerRow.FindControl("selectAllCheckBox");
                    if (selectAllCheckBox != null && selectAllCheckBox.Checked)
                    {
                        // If the "Select All" checkbox is checked, consider all rows as selected
                        foreach (GridViewRow row in DayId.Rows)
                        {
                            Label dayLabel = (Label)row.FindControl("DayLabel");
                            if (dayLabel != null)
                            {
                                checkedDays.Add(dayLabel.Text);
                            }
                        }
                        anyCheckboxSelected = true;
                    }
                    else
                    {
                        // If the "Select All" checkbox is not checked, check individual row checkboxes
                        foreach (GridViewRow row in DayId.Rows)
                        {
                            HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                            if (chkBox != null && chkBox.Checked)
                            {
                                anyCheckboxSelected = true;

                                Label dayLabel = (Label)row.FindControl("DayLabel");
                                if (dayLabel != null)
                                {
                                    checkedDays.Add(dayLabel.Text);
                                }
                            }
                        }
                    }
                }

                string checkedDaysString = string.Join(", ", checkedDays);
                string pattern = @"^[a-zA-Z0-9 _]+$";

                if (distId == "")
                {
                    showToast("Please select Dist. Code", "toast-danger");
                    return;
                }
                else if (rtCode == "")
                {
                    showToast("Please select Route Code", "toast-danger");
                    return;
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(rtCode, pattern))
                {
                    showToast("Route Code can only contain alphanumeric characters, spaces, and underscores", "toast-danger");
                    return;
                }
                else if (rtName == "")
                {
                    showToast("Please select Route Name", "toast-danger");
                    return;
                }
                else if (mnfId == "")
                {
                    showToast("Please select MNF Code", "toast-danger");
                    return;
                }
                else if (rtType == "")
                {
                    showToast("Please select Route Type", "toast-danger");
                    return;
                }
                else if (rtCoverage == "")
                {
                    showToast("Please select Route Coverage", "toast-danger");
                    return;
                }
                //else if (!anyCheckboxSelected)
                //{
                //    showToast("Please select any day", "toast-danger");
                //    return;
                //}

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd1 = new SqlCommand("SP_Route_Create_Modify", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Action", "RTEXISTS");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@Distcode", distId);
                cmd1.Parameters.AddWithValue("@RouteCode", rtCode);
                cmd1.Parameters.AddWithValue("@RouteName", rtName);
                cmd1.Parameters.AddWithValue("@MnfCode", mnfId);
                cmd1.Parameters.AddWithValue("@RtType", rtType);
                cmd1.Parameters.AddWithValue("@RtCoverage", rtCoverage);
                cmd1.Parameters.AddWithValue("@Day", checkedDaysString);
                cmd1.Parameters.AddWithValue("@Status", 1);
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                foreach (DataRow row in resdt.Rows)
                {
                    if (Convert.ToInt32(row["Value"]) == 1)
                    {
                        showToast("Distcode " + distId + " is already having route " + rtCode, "toast-danger");
                        return;
                    }
                    else
                    {
                        SqlCommand cmd2 = new SqlCommand("SP_Route_Create_Modify", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@Action", "INSERT");
                        cmd2.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                        cmd2.Parameters.AddWithValue("@Distcode", distId);
                        cmd2.Parameters.AddWithValue("@RouteCode", rtCode);
                        cmd2.Parameters.AddWithValue("@RouteName", rtName);
                        cmd2.Parameters.AddWithValue("@MnfCode", mnfId);
                        cmd2.Parameters.AddWithValue("@RtType", rtType);
                        cmd2.Parameters.AddWithValue("@RtCoverage", rtCoverage);
                        cmd2.Parameters.AddWithValue("@Day", checkedDaysString);
                        cmd2.Parameters.AddWithValue("@Status", 1);
                        cmd2.ExecuteNonQuery();

                        showToast("Route Created Successfully", "toast-success");

                        cmd2.CommandTimeout = 6000;

                        con.Close();

                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region EditBtn_Click
        protected void EditBtn_Click(object sender, EventArgs e)
        {
            // Get the reference to the clicked button
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            // Extract the data from the GridView row
            string distCode = row.Cells[0].Text;
            string routeCode = row.Cells[1].Text;
            //string routeName = row.Cells[2].Text;
            //string mnfCde = row.Cells[3].Text;
            //string routeTypeName = row.Cells[4].Text;
            //string coverageName = row.Cells[5].Text;
            //string weekDay = row.Cells[6].Text;
            //string status = row.Cells[7].Text;

            // Store data in session variables
            Session["DistCode"] = distCode;
            Session["RouteCode"] = routeCode;

            Response.Redirect("~/Modify");
        }
        #endregion

        #region ClearForm
        private void ClearForm()
        {
            DistDrp.ClearSelection();
            MnfId.ClearSelection();
            RtCode.Text = string.Empty;
            RtName.Text = string.Empty;
            RtType.ClearSelection();
            RtCoverage.ClearSelection();

            foreach (GridViewRow row in DayId.Rows)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                if (chk != null)
                {
                    chk.Checked = false;
                }
            }

            GridViewRow headerRow = DayId.HeaderRow;
            if (headerRow != null)
            {
                HtmlInputCheckBox allchk = (HtmlInputCheckBox)headerRow.FindControl("selectAllCheckBox");
                if (allchk != null)
                {
                    allchk.Checked = false;
                }
            }

            RouteGridView.DataSource = null;
            RouteGridView.DataBind();
        }
        #endregion

        #region SetActiveNavLink
        //private void SetActiveNavLink()
        //{
        //    // Get the current page name
        //    string currentPage = Request.Url.AbsolutePath;

        //    // Determine which link to set as active
        //    if (currentPage.EndsWith("Home.aspx", StringComparison.OrdinalIgnoreCase))
        //    {
        //        HomeLink.Attributes.Add("class", "nav-link btn-outline-secondary");
        //    }
        //    else if (currentPage.EndsWith("Create", StringComparison.OrdinalIgnoreCase))
        //    {
        //        CreateLink.Attributes.Add("class", "btn-outline-secondary");
        //    }
        //    else if (currentPage.EndsWith("Modify.aspx", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ModifyLink.Attributes.Add("class", "nav-link btn-outline-secondary");
        //    }
        //}
        #endregion

        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }
        #endregion
    }
}