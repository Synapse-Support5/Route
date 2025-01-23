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
using System.Web.UI.HtmlControls;

namespace Route
{
    public partial class BeatReailgnment : System.Web.UI.Page
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
                        SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                        cmd1.Parameters.AddWithValue("@ActionType", "Session");
                        cmd1.Parameters.AddWithValue("@DistCode", "");
                        cmd1.Parameters.AddWithValue("@FromRouteId", "");
                        cmd1.Parameters.AddWithValue("@ToRouteId", "");
                        cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                        cmd1.Parameters.AddWithValue("@FromUrCode", "");
                        cmd1.Parameters.AddWithValue("@Search", "");
                        cmd1.Parameters.AddWithValue("@Status", "");

                        cmd1.CommandTimeout = 6000;
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(resdt);

                        if (resdt.Rows.Count > 0)
                        {
                            //lblUserName.Text = "User Name > " + resdt.Rows[0][0].ToString() + ": User ID > " + Session["name"].ToString();
                            lblUserName.Text = "Welcome : " + resdt.Rows[0][0].ToString();
                            hdnBusinessType.Value = resdt.Rows[0][2].ToString();
                            hdnRole.Value = resdt.Rows[0][3].ToString();

                            DistLoad();
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
            FromRouteDrp.ClearSelection();
            ToRouteDrp.ClearSelection();

            DistCodeLoadGridView.DataSource = null;
            DistCodeLoadGridView.DataBind();

            FromRouteLoadGridView.DataSource = null;
            FromRouteLoadGridView.DataBind();

            ToRouteLoadGridView.DataSource = null;
            ToRouteLoadGridView.DataBind();

            FromRouteLoad();
            DistCodeLoadGrid();

            DistSearch.Value = DistDrp.SelectedItem.ToString();
        }
        protected void FromRouteDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToRouteDrp.ClearSelection();

            FromRouteLoadGridView.DataSource = null;
            FromRouteLoadGridView.DataBind();

            ToRouteLoadGridView.DataSource = null;
            ToRouteLoadGridView.DataBind();

            DistCodeLoadGridView.DataSource = null;
            DistCodeLoadGridView.DataBind();

            ToRouteLoad();
            FromRouteLoadGrid();

            FromRouteSearch.Value = FromRouteDrp.SelectedItem.ToString();
        }

        protected void ToRouteDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            FromRouteLoadGridView.DataSource = null;
            FromRouteLoadGridView.DataBind();

            ToRouteLoadGrid();

            ToRouteSearch.Value = ToRouteDrp.SelectedItem.ToString();
        }
        #endregion

        #region DistLoad
        public void DistLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "DistLoad");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@FromRouteId", "");
                cmd1.Parameters.AddWithValue("@ToRouteId", "");
                cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                cmd1.Parameters.AddWithValue("@FromUrCode", "");
                cmd1.Parameters.AddWithValue("@Search", "");
                cmd1.Parameters.AddWithValue("@Status", "");
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

        #region DistCodeLoadGrid
        public void DistCodeLoadGrid()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "DistLoadGrid");
                cmd1.Parameters.AddWithValue("@DistCode", DistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromRouteId", "");
                cmd1.Parameters.AddWithValue("@ToRouteId", "");
                cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                cmd1.Parameters.AddWithValue("@FromUrCode", "");
                cmd1.Parameters.AddWithValue("@Search", "");
                cmd1.Parameters.AddWithValue("@Status", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

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

        #region FromRouteLoad
        public void FromRouteLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "FromRouteLoad");
                cmd1.Parameters.AddWithValue("@DistCode", DistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromRouteId", "");
                cmd1.Parameters.AddWithValue("@ToRouteId", "");
                cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                cmd1.Parameters.AddWithValue("@FromUrCode", "");
                cmd1.Parameters.AddWithValue("@Search", "");
                cmd1.Parameters.AddWithValue("@Status", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                FromRouteDrp.DataSource = resdt;
                FromRouteDrp.DataTextField = resdt.Columns["RouteName"].ToString();
                FromRouteDrp.DataValueField = resdt.Columns["RouteId"].ToString();
                FromRouteDrp.DataBind();
                FromRouteDrp.Items.Insert(0, new ListItem("Route", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region FromRouteLoadGrid
        public void FromRouteLoadGrid()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "FromRouteLoadGrid");
                cmd1.Parameters.AddWithValue("@DistCode", DistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromRouteId", FromRouteDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ToRouteId", "");
                cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                cmd1.Parameters.AddWithValue("@FromUrCode", "");
                cmd1.Parameters.AddWithValue("@Search", "");
                cmd1.Parameters.AddWithValue("@Status", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                FromRouteLoadGridView.DataSource = resdt;
                FromRouteLoadGridView.DataBind();
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region ToRouteLoad
        public void ToRouteLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "ToRouteLoad");
                cmd1.Parameters.AddWithValue("@DistCode", DistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromRouteId", FromRouteDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ToRouteId", "");
                cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                cmd1.Parameters.AddWithValue("@FromUrCode", "");
                cmd1.Parameters.AddWithValue("@Search", "");
                cmd1.Parameters.AddWithValue("@Status", "");

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                ToRouteDrp.DataSource = resdt;
                ToRouteDrp.DataTextField = resdt.Columns["RouteName"].ToString();
                ToRouteDrp.DataValueField = resdt.Columns["RouteId"].ToString();
                ToRouteDrp.DataBind();
                ToRouteDrp.Items.Insert(0, new ListItem("Route", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region ToRouteLoadGrid
        public void ToRouteLoadGrid()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "ToRouteLoadGrid");
                cmd1.Parameters.AddWithValue("@DistCode", DistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromRouteId", FromRouteDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ToRouteId", ToRouteDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                cmd1.Parameters.AddWithValue("@FromUrCode", "");
                cmd1.Parameters.AddWithValue("@Search", "");
                cmd1.Parameters.AddWithValue("@Status", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                ToRouteLoadGridView.DataSource = resdt;
                ToRouteLoadGridView.DataBind();
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region Submit_Click
        protected void Submit_Click(object sender, EventArgs e)
        {
            List<string> checkedRtrs = new List<string>();
            try
            {
                foreach (GridViewRow row in ToRouteLoadGridView.Rows)
                {
                    HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                    if (chkBox != null && chkBox.Checked)
                    {
                        anyCheckboxSelected = true;
                    }
                }

                if (DistDrp.SelectedValue == "")
                {
                    showToast("Please select Distributor", "toast-danger");
                    return;
                }
                else if (FromRouteDrp.SelectedValue == "")
                {
                    showToast("Please select From Route", "toast-danger");
                    return;
                }
                else if (!anyCheckboxSelected)
                {
                    showToast("Please select any Retailer", "toast-danger");
                    return;
                }
                else if (ToRouteDrp.SelectedValue == "")
                {
                    showToast("Please select To Route", "toast-danger");
                    return;
                }

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                foreach (GridViewRow row in ToRouteLoadGridView.Rows)
                {
                    HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                    if (chkBox != null && chkBox.Checked)
                    {
                        string rtr = row.Cells[2].Text;
                        string urCode = row.Cells[4].Text;

                        SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                        cmd1.Parameters.AddWithValue("@ActionType", "SubmitClick");
                        cmd1.Parameters.AddWithValue("@DistCode", DistDrp.SelectedValue);
                        cmd1.Parameters.AddWithValue("@FromRouteId", FromRouteDrp.SelectedValue);
                        cmd1.Parameters.AddWithValue("@ToRouteId", ToRouteDrp.SelectedValue);
                        cmd1.Parameters.AddWithValue("@FromRtrCode", rtr);
                        cmd1.Parameters.AddWithValue("@FromUrCode", urCode);
                        cmd1.Parameters.AddWithValue("@Search", "");
                        cmd1.Parameters.AddWithValue("@Status", "");

                        cmd1.ExecuteNonQuery();

                        cmd1.CommandTimeout = 60000;

                    }
                }

                showToast("Data Updated...", "toast-success");
                con.Close();

                ClearForm();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region btnFetch_Click
        protected void btnFetch_Click(object sender, EventArgs e)
        {
            Fetch();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "updatePanels", "$('#UpdatePanelGrid').trigger('change');", true);
            UpdatePanelFetch.Update();
            UpdatePanelGrid.Update();
        }
        
        protected void Fetch()
        {
            try
            {
                string status = "";
                if (rbActive.Checked)
                {
                    status = "1";
                }
                else if (rbInactive.Checked)
                {
                    status = "0";
                }
                else if (SearchTxt.Text == "" && status == "")
                {
                    showToast("Please select atleast Active/InActive option to fetch data", "toast-danger");
                    return;
                }
                else
                {
                    showToast("Please select Active/InActive option", "toast-danger");
                    return;
                }

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "FetchClick");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@FromRouteId", "");
                cmd1.Parameters.AddWithValue("@ToRouteId", "");
                cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                cmd1.Parameters.AddWithValue("@FromUrCode", "");
                cmd1.Parameters.AddWithValue("@Search", SearchTxt.Text);
                cmd1.Parameters.AddWithValue("@Status", status);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                SearchModalGrid.DataSource = resdt;
                SearchModalGrid.DataBind();
                con.Close();
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
            DistDrp.ClearSelection();
            FromRouteDrp.ClearSelection();
            ToRouteDrp.ClearSelection();

            DistCodeLoadGridView.DataSource = null;
            DistCodeLoadGridView.DataBind();

            FromRouteLoadGridView.DataSource = null;
            FromRouteLoadGridView.DataBind();

            ToRouteLoadGridView.DataSource = null;
            ToRouteLoadGridView.DataBind();

            foreach (GridViewRow row in FromRouteLoadGridView.Rows)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                if (chk != null)
                {
                    chk.Checked = false;
                }
            }

            DistSearch.Value = string.Empty;
            FromRouteSearch.Value = string.Empty;
            ToRouteSearch.Value = string.Empty;
        }
        #endregion


    }
}