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
    public partial class TestPage : System.Web.UI.Page
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
            }
        }

        protected void btnFetch_Click(object sender, EventArgs e)
        {
            FetchFn();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "updatePanels", "$('#UpdatePanelGrid').trigger('change');", true);
            UpdatePanelFetch.Update();
            UpdatePanelGrid.Update();
        }

        protected void FetchFn()
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

                if (string.IsNullOrEmpty(SearchTxt.Text) && string.IsNullOrEmpty(status))
                {
                    SearchModalGrid.DataSource = null;
                    SearchModalGrid.DataBind();
                    return;
                }

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd1 = new SqlCommand("SP_Route_BeatReailgnment", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", "");
                cmd1.Parameters.AddWithValue("@ActionType", "FetchClick");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@FromRouteId", "");
                cmd1.Parameters.AddWithValue("@ToRouteId", "");
                cmd1.Parameters.AddWithValue("@FromRtrCode", "");
                cmd1.Parameters.AddWithValue("@FromUrCode", "");
                cmd1.Parameters.AddWithValue("@Search", SearchTxt.Text);
                cmd1.Parameters.AddWithValue("@Status", status);
                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Clear(); // Clear previous data
                da.Fill(resdt);

                SearchModalGrid.DataSource = resdt;
                SearchModalGrid.DataBind();

                con.Close();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }
    }
}