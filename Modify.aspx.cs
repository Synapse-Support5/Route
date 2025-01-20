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
using static Route.Create;

namespace Route
{
    public partial class Modify : System.Web.UI.Page
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
                if (Session["DistCode"] != null && Session["RouteCode"] != null)
                {
                    string distCode = Session["DistCode"].ToString();
                    string routeCode = Session["RouteCode"].ToString();

                    DataLoad(distCode, routeCode);
                }

                AccessLoad();
                BindWeekdays();
            }
        }

        #region AccessLoad
        public void AccessLoad()
        {
            try
            {
                //Session["name"] = "G116036";
                Session["name"] = Request.ServerVariables["REMOTE_USER"].Substring(6);

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

        #region DataLoad
        public void DataLoad(string distCode, string routeCode)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Modify_Load", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@DistCode", distCode);
                cmd1.Parameters.AddWithValue("@RtCode", routeCode);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                ds1.Clear();
                da.Fill(ds1);

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DistCode.Text = ds1.Tables[0].Rows[0]["DistNm"].ToString();
                }

                if (ds1.Tables[1].Rows.Count > 0)
                {
                    RtCode.Text = ds1.Tables[1].Rows[0]["RouteCode"].ToString();
                }

                if (ds1.Tables[2].Rows.Count > 0)
                {
                    RtName.Text = ds1.Tables[2].Rows[0]["RouteName"].ToString();
                }

                MnfCodeList(ds1.Tables[3]);
                RtTypeList(ds1.Tables[4]);
                RtCoverageList(ds1.Tables[5]);
                StatusList(ds1.Tables[7]);

                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region MnfCodeList
        private void MnfCodeList(DataTable dt)
        {
            MnfId.DataSource = dt;
            MnfId.DataTextField = "MnfCde";
            MnfId.DataValueField = "MnfId";
            MnfId.DataBind();

            MnfId.Items.Insert(0, new ListItem("MNF Code", ""));

            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["Selected"]) == 1)
                {
                    MnfId.Items.FindByValue(row["MnfId"].ToString()).Selected = true;
                }
            }
        }
        #endregion

        #region RtTypeList
        private void RtTypeList(DataTable dt)
        {
            RtType.DataSource = dt;
            RtType.DataTextField = "RouteTypeName";
            RtType.DataValueField = "RouteTypeId";
            RtType.DataBind();

            RtType.Items.Insert(0, new ListItem("Route Type", ""));

            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["Selected"]) == 1)
                {
                    RtType.Items.FindByValue(row["RouteTypeId"].ToString()).Selected = true;
                }
            }
        }
        #endregion

        #region RtCoverageList
        private void RtCoverageList(DataTable dt)
        {
            RtCoverage.DataSource = dt;
            RtCoverage.DataTextField = "CoverageName";
            RtCoverage.DataValueField = "CoverageId";
            RtCoverage.DataBind();

            RtCoverage.Items.Insert(0, new ListItem("Route Coverage", ""));

            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["Selected"]) == 1)
                {
                    RtCoverage.Items.FindByValue(row["CoverageId"].ToString()).Selected = true;
                }
            }
        }
        #endregion

        #region Call Days
        private void BindWeekdays()
        {
            // Define weekdays
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

            // Assuming ds1.Tables[6] contains the comma-separated list of weekdays in the column "Weekdays"
            if (ds1.Tables[6].Rows.Count > 0)
            {
                // Clear any previous selections
                foreach (var weekday in weekdays)
                {
                    weekday.IsSelected = false;
                }

                string weekdaysString = ds1.Tables[6].Rows[0]["Weekday"].ToString();
                if (!string.IsNullOrEmpty(weekdaysString))
                {
                    string[] splitWeekdays = weekdaysString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    // Check checkboxes based on fetched and split values
                    foreach (string dayName in splitWeekdays)
                    {
                        var weekday = weekdays.FirstOrDefault(w => w.DayName.Equals(dayName.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (weekday != null)
                        {
                            weekday.IsSelected = true;
                        }
                    }
                }
            }

            // Bind to GridView
            DayId.DataSource = weekdays;
            DayId.DataBind();

            // Iterate through GridView rows to set checkbox status
            for (int i = 0; i < DayId.Rows.Count; i++)
            {
                GridViewRow row = DayId.Rows[i];

                // Find controls within the row
                HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");

                // Set checkbox status based on IsSelected property of Weekday
                if (chkBox != null)
                {
                    Weekday weekday = weekdays[i]; // Assuming weekdays list matches GridView row order

                    chkBox.Checked = weekday.IsSelected;
                }
            }
        }

        public class Weekday
        {
            public string DayName { get; set; }
            public bool IsSelected { get; set; }
        }
        #endregion

        #region StatusList
        private void StatusList(DataTable dt)
        {
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add(new ListItem("Status", ""));
            DropDownList1.Items.Add(new ListItem("Active", "1"));
            //DropDownList1.Items.Add(new ListItem("Inactive", "0"));

            DropDownList1.DataBind();

            // Set selected item based on fetched data
            if (dt.Rows.Count > 0)
            {
                int status = Convert.ToInt32(dt.Rows[0]["Status"]);
                if (status == 0 || status == 1)
                {
                    DropDownList1.SelectedValue = status.ToString();
                }
            }
        }
        #endregion

        #region Modify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            BtnModify();
        }
        #endregion

        #region BtnModify
        public void BtnModify()
        {
            try
            {
                string distId = DistCode.Text;
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

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Create_Modify", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Action", "UPDATE");
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@Distcode", distId);
                cmd1.Parameters.AddWithValue("@RouteCode", rtCode);
                cmd1.Parameters.AddWithValue("@RouteName", rtName);
                cmd1.Parameters.AddWithValue("@MnfCode", mnfId);
                cmd1.Parameters.AddWithValue("@RtType", rtType);
                cmd1.Parameters.AddWithValue("@RtCoverage", rtCoverage);
                cmd1.Parameters.AddWithValue("@Day", checkedDaysString);
                cmd1.Parameters.AddWithValue("@Status", DropDownList1.SelectedValue);
                cmd1.CommandTimeout = 6000;
                int result = cmd1.ExecuteNonQuery();
                con.Close();

                if (result > 0)
                {
                    Session["ResponseMessage"] = "Data Modified Successfully";
                    Session["ResponseStatus"] = "Success";
                }
                else
                {
                    Session["ResponseMessage"] = "Data Modification Failed";
                    Session["ResponseStatus"] = "Failed";
                }

                //showToast("Data Modified... ", "toast-success");
                Response.Redirect("~/Create");
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
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