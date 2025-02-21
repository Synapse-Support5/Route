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

namespace Route
{
    public partial class SOBeatCreation : System.Web.UI.Page
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
                ZoneLoad();
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
                        SqlCommand cmd1 = new SqlCommand("SP_Route_SOBeatCreation", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                        cmd1.Parameters.AddWithValue("@ActionType", "Session");
                        cmd1.Parameters.AddWithValue("@Zone", "");
                        cmd1.Parameters.AddWithValue("@SOCode", "");

                        cmd1.CommandTimeout = 6000;
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(resdt);

                        if (resdt.Rows.Count > 0)
                        {
                            //lblUserName.Text = "User Name > " + resdt.Rows[0][0].ToString() + ": User ID > " + Session["name"].ToString();
                            lblUserName.Text = "Welcome : " + resdt.Rows[0][0].ToString();
                            hdnBusinessType.Value = resdt.Rows[0][2].ToString();
                            hdnRole.Value = resdt.Rows[0][3].ToString();

                            //DistLoad();
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

        #region ZoneLoad
        public void ZoneLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_SOBeatCreation", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "ZoneLoad");
                cmd1.Parameters.AddWithValue("@Zone", "");
                cmd1.Parameters.AddWithValue("@SOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                ZoneDrp.DataSource = resdt;
                ZoneDrp.DataTextField = resdt.Columns["Zone"].ToString();
                ZoneDrp.DataValueField = resdt.Columns["Zone"].ToString();
                ZoneDrp.DataBind();
                ZoneDrp.Items.Insert(0, new ListItem("Zone", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region SOLoad
        public void SOLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_SOBeatCreation", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "SOLoad");
                cmd1.Parameters.AddWithValue("@Zone", ZoneDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@SOCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                SODrp.DataSource = resdt;
                SODrp.DataTextField = resdt.Columns["SOName"].ToString();
                SODrp.DataValueField = resdt.Columns["SOCode"].ToString();
                SODrp.DataBind();
                SODrp.Items.Insert(0, new ListItem("SO Code", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region DistRtrLoadGrid
        public void DistRtrLoadGrid()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd1 = new SqlCommand("SP_Route_SOBeatCreation", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "DistRtrLoadGrid");
                cmd1.Parameters.AddWithValue("@Zone", ZoneDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@SOCode", SODrp.SelectedValue);
                cmd1.CommandTimeout = 6000;
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                if (resdt.Rows.Count > 0)
                {
                    DistRtrLoadGrid2.DataSource = resdt;
                    DistRtrLoadGrid2.DataBind();
                }
                else
                {
                    DistRtrLoadGrid2.DataSource = null;
                    DistRtrLoadGrid2.DataBind();
                }
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }

        }
        #endregion

        #region SelectedIndexChanged

        protected void ZoneDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            SOLoad();
        }

        protected void SODrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistRtrLoadGrid();
        }
        #endregion

        #region Submit_Click
        protected void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Dists = new DataTable();
                Dists.Columns.Add("DistCode", typeof(string));

                HashSet<string> uniqueDistCodes = new HashSet<string>();

                // Iterate through GridView rows
                foreach (GridViewRow row in DistRtrLoadGrid2.Rows)
                {
                    // Find the checkbox control
                    HtmlInputCheckBox chk = (HtmlInputCheckBox)row.FindControl("CheckBox1");

                    if (chk != null && chk.Checked)
                    {
                        // Get the DistCode from the row
                        string distCode = row.Cells[0].Text.Trim();

                        // Add to HashSet to remove duplicates
                        if (!string.IsNullOrEmpty(distCode))
                        {
                            uniqueDistCodes.Add(distCode);
                        }
                    }
                }

                // Convert HashSet to DataTable
                foreach (string code in uniqueDistCodes)
                {
                    Dists.Rows.Add(code);
                }

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd1 = new SqlCommand("SP_Route_SOBeatCreation_NewLogic", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@SOBeatDistcodeTable", Dists);
                cmd1.Parameters.AddWithValue("@SOCode", SODrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ZoneName", ZoneDrp.SelectedValue);
                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);

                //DataTable dtSo = new DataTable();
                DataSet dsSo = new DataSet();
                //da.Fill(dtSo);

                da.Fill(dsSo);

                if (dsSo.Tables[1].Rows.Count > 0)
                {
                    // Example: Binding DataTable to a GridView for display
                    ResultModalGrid.DataSource = dsSo.Tables[1];
                    ResultModalGrid.DataBind();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ClickButton", "$('#Button1').click();", true);

                    showToast("Some of the records uploaded", "toast-success");
                }
                else
                {
                    showToast("All records uploaded", "toast-success");
                }

                con.Close();

                ClearForm();
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

        #region ClearForm
        public void ClearForm()
        {
            ZoneDrp.ClearSelection();
            SODrp.ClearSelection();

            DistRtrLoadGrid2.DataSource = null;
            DistRtrLoadGrid2.DataBind();

            foreach (GridViewRow row in DistRtrLoadGrid2.Rows)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                if (chk != null)
                {
                    chk.Checked = false;
                }
            }
        }
        #endregion
    }
}