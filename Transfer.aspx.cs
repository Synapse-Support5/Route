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

namespace Route
{
    public partial class Transfer : System.Web.UI.Page
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
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region SelectTypeTransferButtons
        protected void RouteTransferBtn_Click(object sender, EventArgs e)
        {
            RouteTransferBtn_Submit();
        }
        protected void RetTransferBtn_Click(object sender, EventArgs e)
        {
            RetTransferBtn_Submit();
        }
        public void RouteTransferBtn_Submit()
        {
            RouteTransferBtn.CssClass = "btn btn-info form-control";
            RetTransferBtn.CssClass = "btn btn-outline-info form-control";

            routeTransferDiv.Visible = true;
            retailerTransferDiv.Visible = false;

            

            RouteFromDistLoad();
        }
        public void RetTransferBtn_Submit()
        {
            RouteTransferBtn.CssClass = "btn btn-outline-info form-control";
            RetTransferBtn.CssClass = "btn btn-info form-control";

            routeTransferDiv.Visible = false;
            retailerTransferDiv.Visible = true;

            RouteTransExistGridView.DataSource = null;
            RouteTransExistGridView.DataBind();

            RouteTransSplitGridView.DataSource = null;
            RouteTransSplitGridView.DataBind();

            RetFromDistLoad();
        }
        #endregion

        #region SelectedIndexChanged
        protected void FromDistDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            RouteToDistLoad();
        }
        protected void TypeDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = TypeDrp.SelectedValue;
            if (val == "Existing")
            {
                GetRouteTransExistGridView();
            }
            else
            {
                GetRouteTransSplitGridView();
            }
        }
        #endregion

        #region RouteFromDistLoad
        public void RouteFromDistLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RouteTransFromDistLoad");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                FromDistDrp.DataSource = resdt;
                FromDistDrp.DataTextField = resdt.Columns["DistNm"].ToString();
                FromDistDrp.DataValueField = resdt.Columns["DistCode"].ToString();
                FromDistDrp.DataBind();
                FromDistDrp.Items.Insert(0, new ListItem("From Distributor", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region RouteToDistLoad
        public void RouteToDistLoad()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RouteTransToDistLoad");
                cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                ToDistDrp.DataSource = resdt;
                ToDistDrp.DataTextField = resdt.Columns["DistNm"].ToString();
                ToDistDrp.DataValueField = resdt.Columns["DistCode"].ToString();
                ToDistDrp.DataBind();
                ToDistDrp.Items.Insert(0, new ListItem("To Distributor", ""));
                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region GetRouteTransExistGridView
        public void GetRouteTransExistGridView()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RouteTransExistGrid");
                cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                if (resdt.Rows.Count > 0)
                {
                    RouteTransExistGridView.DataSource = resdt;
                    RouteTransExistGridView.DataBind();

                    RouteTransSplitGridView.DataSource = null;
                    RouteTransSplitGridView.DataBind();
                }
                else
                {
                    RouteTransExistGridView.DataSource = null;
                    RouteTransExistGridView.DataBind();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region GetRouteTransSplitGridView
        public void GetRouteTransSplitGridView()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RouteTransSplitGrid");
                cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                if (resdt.Rows.Count > 0)
                {
                    RouteTransExistGridView.DataSource = null;
                    RouteTransExistGridView.DataBind();

                    RouteTransSplitGridView.DataSource = resdt;
                    RouteTransSplitGridView.DataBind();
                }
                else
                {
                    RouteTransSplitGridView.DataSource = null;
                    RouteTransSplitGridView.DataBind();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion




        #region RetFromDistLoad
        public void RetFromDistLoad()
        {
            try
            {
                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                //SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                //cmd1.CommandType = CommandType.StoredProcedure;
                //cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                //cmd1.Parameters.AddWithValue("@ActionType", "FromDistLoad");
                //cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                //cmd1.ExecuteNonQuery();

                //cmd1.CommandTimeout = 6000;

                //SqlDataAdapter da = new SqlDataAdapter(cmd1);
                //resdt.Rows.Clear();
                //da.Fill(resdt);
                //ToDistDrp.DataSource = resdt;
                //ToDistDrp.DataTextField = resdt.Columns["DistNm"].ToString();
                //ToDistDrp.DataValueField = resdt.Columns["DistCode"].ToString();
                //ToDistDrp.DataBind();
                //ToDistDrp.Items.Insert(0, new ListItem("Dist. Code", ""));
                //con.Close();
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

        protected void RouteTransferSubmit_Click(object sender, EventArgs e)
        {
            showToast("Toast is working fine...", "toast-success");
        }
    }
}