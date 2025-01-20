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
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Reflection.Emit;
using System.Web.DynamicData;

namespace Route
{
    public partial class Transfer : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable dt = new DataTable();
        DataTable resdt = new DataTable();
        public DataSet ds1 = new DataSet();
        bool anyCheckboxSelected = false;
        string Approval = string.Empty;
        string existingUr = string.Empty;
        string existingRtr = string.Empty;
        int existing = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AccessLoad();

                bool isGridVisible = Session["IsGridVisible"] != null && (bool)Session["IsGridVisible"];
                ToDistExistViewGrid.Visible = isGridVisible;
                View.Text = isGridVisible ? "Hide" : "View";
            }
        }

        #region AccessLoad
        public void AccessLoad()
        {
            try
            {
                ////Session["name"] = "G116036";
                //Session["name"] = Request.ServerVariables["REMOTE_USER"].Substring(6);

                //if (Session["name"].ToString() != "")
                //{
                //    if (con.State == ConnectionState.Closed)
                //    {
                //        con.Open();
                //    }
                //    SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                //    cmd1.CommandType = CommandType.StoredProcedure;
                //    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                //    cmd1.Parameters.AddWithValue("@ActionType", "Session");
                //    cmd1.Parameters.AddWithValue("@DistCode", "");
                //    cmd1.Parameters.AddWithValue("@StateID", "");
                //    cmd1.Parameters.AddWithValue("@AreaID", "");
                //    cmd1.Parameters.AddWithValue("@ZoneID", "");
                //    cmd1.Parameters.AddWithValue("@ToDistCode", "");
                //    cmd1.Parameters.AddWithValue("@RouteCode", "");

                //    cmd1.CommandTimeout = 6000;
                //    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                //    da.Fill(resdt);

                //    if (resdt.Rows.Count > 0)
                //    {
                //        //lblUserName.Text = "User Name > " + resdt.Rows[0][0].ToString() + ": User ID > " + Session["name"].ToString();
                //        lblUserName.Text = "Welcome : " + resdt.Rows[0][0].ToString();
                //        hdnBusinessType.Value = resdt.Rows[0][2].ToString();
                //        hdnRole.Value = resdt.Rows[0][3].ToString();

                //        StateLoad();
                //    }
                //    else
                //    {
                //        Response.Redirect("AccessDeniedPage.aspx");
                //    }
                //    con.Close();
                //}
                //else
                //{
                //    Response.Redirect("AccessDeniedPage.aspx");
                //}


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

                        SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                        cmd1.Parameters.AddWithValue("@ActionType", "Session");
                        cmd1.Parameters.AddWithValue("@DistCode", "");
                        cmd1.Parameters.AddWithValue("@StateID", "");
                        cmd1.Parameters.AddWithValue("@AreaID", "");
                        cmd1.Parameters.AddWithValue("@ZoneID", "");
                        cmd1.Parameters.AddWithValue("@ToDistCode", "");
                        cmd1.Parameters.AddWithValue("@RouteCode", "");
                        cmd1.CommandTimeout = 6000;

                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(resdt);

                        if (resdt.Rows.Count > 0)
                        {
                            lblUserName.Text = "Welcome : " + resdt.Rows[0][0].ToString();
                            hdnBusinessType.Value = resdt.Rows[0][2].ToString();
                            hdnRole.Value = resdt.Rows[0][3].ToString();

                            StateLoad();
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
                showToast("An error occurred: ", "toast-danger");
            }
        }
        #endregion

        #region SelectedIndexChanged
        protected void FromDistDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RouteToDistLoad();
        }
        protected void TypeDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = TypeDrp.SelectedValue;
            if (val == "Existing")
            {
                TypeDrpSelected.Visible = true;
                btnOpenModal.Visible = false;
                GetRouteTransExistGridView();
            }
            else if (val == "Split")
            {
                btnOpenModal.Visible = true;
                TypeDrpSelected.Visible = false;
                GetRouteTransSplitGridView();
            }
            else
            {
                btnOpenModal.Visible = false;
                TypeDrpSelected.Visible = false;
            }



            RouteToDistLoad();
        }

        protected void StateDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            AreaLoad();
        }

        protected void AreaDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZoneLoad();
        }

        protected void ZoneDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToDistDrp.ClearSelection();
            ToDistSearch.Value = string.Empty;
            resdt.Rows.Clear();
            ToDistDrp.DataSource = resdt;
            ToDistDrp.DataBind();
            ToDistDrp.Items.Insert(0, new ListItem("To Distributor", ""));

            RouteFromDistLoad();
        }

        protected void RouteTransToDistDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            RouteTransToDistExistCheck();

            ToDistSearch.Value = ToDistDrp.SelectedItem.ToString();
        }
        #endregion

        #region StateLoad
        public void StateLoad()
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
                cmd1.Parameters.AddWithValue("@ActionType", "StateLoad");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@StateID", "");
                cmd1.Parameters.AddWithValue("@AreaID", "");
                cmd1.Parameters.AddWithValue("@ZoneID", "");
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteCode", "");

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                StateDrp.DataSource = resdt;
                StateDrp.DataTextField = resdt.Columns["StateName"].ToString();
                StateDrp.DataValueField = resdt.Columns["StateId"].ToString();
                StateDrp.DataBind();
                StateDrp.Items.Insert(0, new ListItem("State", ""));
                con.Close();

            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region AreaLoad
        public void AreaLoad()
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
                cmd1.Parameters.AddWithValue("@ActionType", "AreaLoad");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@StateID", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@AreaID", "");
                cmd1.Parameters.AddWithValue("@ZoneID", "");
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteCode", "");

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                AreaDrp.DataSource = resdt;
                AreaDrp.DataTextField = resdt.Columns["AreaName"].ToString();
                AreaDrp.DataValueField = resdt.Columns["AreaId"].ToString();
                AreaDrp.DataBind();
                AreaDrp.Items.Insert(0, new ListItem("Area", ""));
                con.Close();

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
                SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "ZoneLoad");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@StateID", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@AreaID", AreaDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ZoneID", "");
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteCode", "");

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);
                ZoneDrp.DataSource = resdt;
                ZoneDrp.DataTextField = resdt.Columns["DistrictName"].ToString();
                ZoneDrp.DataValueField = resdt.Columns["DistrictId"].ToString();
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
                cmd1.Parameters.AddWithValue("@StateID", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@AreaID", AreaDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ZoneID", ZoneDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteCode", "");

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
                cmd1.Parameters.AddWithValue("@StateID", StateDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@AreaID", AreaDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ZoneID", ZoneDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteCode", "");

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
                cmd1.Parameters.AddWithValue("@StateID", "");
                cmd1.Parameters.AddWithValue("@AreaID", "");
                cmd1.Parameters.AddWithValue("@ZoneID", "");
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteCode", "");

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
                cmd1.Parameters.AddWithValue("@ActionType", "RouteTransSplitModal");
                cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@StateID", "");
                cmd1.Parameters.AddWithValue("@AreaID", "");
                cmd1.Parameters.AddWithValue("@ZoneID", "");
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteCode", "");

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                if (resdt.Rows.Count > 0)
                {
                    RouteTransExistGridView.DataSource = null;
                    RouteTransExistGridView.DataBind();

                    //RouteTransSplitGridView.DataSource = resdt;
                    //RouteTransSplitGridView.DataBind();

                    RouteSplitTransModal.DataSource = resdt;
                    RouteSplitTransModal.DataBind();
                }
                else
                {
                    //RouteTransSplitGridView.DataSource = null;
                    //RouteTransSplitGridView.DataBind();

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

        #region View_Click
        protected void View_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the grid is currently visible
                bool isGridVisible = Session["IsGridVisible"] != null && (bool)Session["IsGridVisible"];

                // Toggle visibility
                isGridVisible = !isGridVisible;
                Session["IsGridVisible"] = isGridVisible;

                if (isGridVisible)
                {
                    // Bind the data and show the grid
                    DataTable dt = Session["RouteDataSet"] as DataTable;
                    ToDistExistViewGrid.DataSource = dt;
                    ToDistExistViewGrid.DataBind();

                    // Show grid and scroll to it
                    ToDistExistViewGrid.Visible = true;
                    View.Text = "Hide";
                    ClientScript.RegisterStartupScript(this.GetType(), "ScrollToGrid", "scrollToGrid(true);", true);
                }
                else
                {
                    // Hide the grid
                    ToDistExistViewGrid.DataSource = null;
                    ToDistExistViewGrid.DataBind();

                    ToDistExistViewGrid.Visible = false;
                    View.Text = "View";
                    ClientScript.RegisterStartupScript(this.GetType(), "RemoveFocus", "scrollToGrid(false);", true);
                }
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region RouteTransferSubmit_Click
        protected void RouteTransferSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string state = StateDrp.SelectedValue;
                string area = AreaDrp.SelectedValue;
                string zone = ZoneDrp.SelectedValue;
                string fromDist = FromDistDrp.SelectedValue;
                string typeDrp = TypeDrp.SelectedValue;
                string toDist = ToDistDrp.SelectedValue;
                List<string> checkedRoutes = new List<string>();
                List<string> messages = new List<string>();

                if (TypeDrp.SelectedValue == "Existing")
                {
                    foreach (GridViewRow row in RouteTransExistGridView.Rows)
                    {
                        HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                        if (chkBox != null && chkBox.Checked)
                        {
                            anyCheckboxSelected = true;
                        }
                    }
                }
                else if (TypeDrp.SelectedValue == "Split")
                {
                    foreach (GridViewRow row in RouteTransSplitRetailerGrid.Rows)
                    {
                        HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                        if (chkBox != null && chkBox.Checked)
                        {
                            anyCheckboxSelected = true;

                        }
                    }
                }

                if (state == "")
                {
                    showToast("Please select State", "toast-danger");
                    return;
                }
                else if (area == "")
                {
                    showToast("Please select Area", "toast-danger");
                    return;
                }
                else if (zone == "")
                {
                    showToast("Please select Zone", "toast-danger");
                    return;
                }
                else if (fromDist == "")
                {
                    showToast("Please select From Distributor", "toast-danger");
                    return;
                }
                else if (typeDrp == "")
                {
                    showToast("Please select Transfer Type", "toast-danger");
                    return;
                }
                else if (!anyCheckboxSelected)
                {
                    showToast("Please select any Route/Retailer to Transfer", "toast-danger");
                    return;
                }
                else if (toDist == "")
                {
                    showToast("Please select To Distributor", "toast-danger");
                    return;
                }

                ConfirmationDialog();

                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}

                //if (TypeDrp.SelectedValue == "Existing")
                //{
                //    foreach (string routeCode in checkedRoutes)
                //    {
                //        //SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                //        //cmd1.CommandType = CommandType.StoredProcedure;
                //        //cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                //        //cmd1.Parameters.AddWithValue("@ActionType", "RouteTransferBtnClick");
                //        //cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                //        //cmd1.Parameters.AddWithValue("@StateID", "");
                //        //cmd1.Parameters.AddWithValue("@AreaID", "");
                //        //cmd1.Parameters.AddWithValue("@ZoneID", "");
                //        //cmd1.Parameters.AddWithValue("@ToDistCode", ToDistDrp.SelectedValue);
                //        //cmd1.Parameters.AddWithValue("@RouteCode", routeCode);


                //        //cmd1.CommandTimeout = 60000;

                //        //using (SqlDataReader reader = cmd1.ExecuteReader())
                //        //{
                //        //    while (reader.Read())
                //        //    {
                //        //        // Collect each response message in the list
                //        //        string message = reader[0].ToString();
                //        //        if (!string.IsNullOrEmpty(message))
                //        //        {
                //        //            messages.Add(message);
                //        //        }
                //        //    }
                //        //}

                //    }
                //}
                //else if (TypeDrp.SelectedValue == "Split")
                //{
                //    foreach (GridViewRow row in RouteTransSplitRetailerGrid.Rows)
                //    {
                //        HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                //        if (chkBox != null && chkBox.Checked)
                //        {
                //            string routeCode = row.Cells[1].Text;
                //            checkedRoutes.Add(routeCode);

                //            string urCode = row.Cells[4].Text;

                //            string rtrCode = row.Cells[3].Text;

                //            anyCheckboxSelected = true;


                //            foreach (string routeCodes in checkedRoutes)
                //            {
                //                //string checkedurcs = checkedRoutes.ToString();

                //                //SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer_SplitCase", con);
                //                //cmd1.CommandType = CommandType.StoredProcedure;
                //                //cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                //                //cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                //                //cmd1.Parameters.AddWithValue("@ToDistCode", ToDistDrp.SelectedValue);
                //                //cmd1.Parameters.AddWithValue("@RouteCode", routeCodes);
                //                //cmd1.Parameters.AddWithValue("@UrCode", urCode);
                //                //cmd1.Parameters.AddWithValue("@RtrCode", rtrCode);

                //                //cmd1.CommandTimeout = 60000;

                //                //using (SqlDataReader reader = cmd1.ExecuteReader())
                //                //{
                //                //    while (reader.Read())
                //                //    {
                //                //        // Collect each response message in the list
                //                //        string message = reader[0].ToString();
                //                //        if (!string.IsNullOrEmpty(message))
                //                //        {
                //                //            messages.Add(message);
                //                //        }
                //                //    }
                //                //}

                //            }
                //        }
                //    }
                //}

                //con.Close();

                ////string script = "showMessagesWithDelay(" + Newtonsoft.Json.JsonConvert.SerializeObject(messages) + ");";
                ////ClientScript.RegisterStartupScript(this.GetType(), "showMessages", script, true);

                //ClearForm();

                //ProceedTransfer();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region ProceedTransfer
        public void ProceedTransfer()
        {
            try
            {
                List<string> checkedRoutes = new List<string>();
                List<string> messages = new List<string>();

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                if (TypeDrp.SelectedValue == "Existing")
                {
                    foreach (GridViewRow row in RouteTransExistGridView.Rows)
                    {
                        HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                        if (chkBox != null && chkBox.Checked)
                        {
                            string routeCode = row.Cells[1].Text;
                            checkedRoutes.Add(routeCode);

                            anyCheckboxSelected = true;


                            foreach (string routeCodee in checkedRoutes)
                            {
                                SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer_Existing_NewLogic", con);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                                cmd1.Parameters.AddWithValue("@ActionType", "RouteTransferBtnClick");
                                cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                                cmd1.Parameters.AddWithValue("@StateID", "");
                                cmd1.Parameters.AddWithValue("@AreaID", "");
                                cmd1.Parameters.AddWithValue("@ZoneID", "");
                                cmd1.Parameters.AddWithValue("@ToDistCode", ToDistDrp.SelectedValue);
                                cmd1.Parameters.AddWithValue("@RouteCode", routeCodee);


                                cmd1.CommandTimeout = 60000;

                                using (SqlDataReader reader = cmd1.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        // Collect each response message in the list
                                        string message = reader[0].ToString();
                                        if (!string.IsNullOrEmpty(message))
                                        {
                                            messages.Add(message);
                                        }
                                    }
                                }

                            }
                        }
                    }

                }
                else if (TypeDrp.SelectedValue == "Split")
                {
                    DataTable dtforParam = new DataTable();
                    dtforParam.Columns.Add("RouteCode", typeof(string));
                    dtforParam.Columns.Add("UrCode", typeof(string));
                    dtforParam.Columns.Add("RtrCode", typeof(string));

                    foreach (GridViewRow row in RouteTransSplitRetailerGrid.Rows)
                    {
                        HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                        if (chkBox != null && chkBox.Checked)
                        {
                            string routeCode = row.Cells[1].Text;
                            string urCode = row.Cells[4].Text;
                            string rtrCode = row.Cells[3].Text;

                            dtforParam.Rows.Add(routeCode, urCode, rtrCode);

                        }
                    }

                    SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer_SplitCase_NewLogic", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                    cmd1.Parameters.AddWithValue("@ToDistCode", ToDistDrp.SelectedValue);
                    cmd1.Parameters.AddWithValue("@DTRetailerRouteDetails", dtforParam);

                    cmd1.CommandTimeout = 60000;

                    using (SqlDataReader reader = cmd1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Collect each response message in the list
                            string message = reader[0].ToString();
                            if (!string.IsNullOrEmpty(message))
                            {
                                messages.Add(message);
                            }
                        }
                    }
                }

                con.Close();

                string script = "showMessagesWithDelay(" + Newtonsoft.Json.JsonConvert.SerializeObject(messages) + ");";
                ClientScript.RegisterStartupScript(this.GetType(), "showMessages", script, true);

                ClearForm();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        //#region RetFromDistLoad
        //public void RetFromDistLoad()
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        showToast("An error occurred: " + ex.Message, "toast-danger");
        //    }
        //}
        //#endregion

        #region SelectBtn_Click
        protected void SelectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> checkedRoutes = new List<string>();

                foreach (GridViewRow row in RouteSplitTransModal.Rows)
                {
                    HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                    if (chkBox != null && chkBox.Checked)
                    {
                        string routeCode = row.Cells[0].Text;
                        checkedRoutes.Add(routeCode);
                        anyCheckboxSelected = true;
                    }
                }

                string routes = string.Join(",", checkedRoutes);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RouteTransSplitGrid");
                cmd1.Parameters.AddWithValue("@DistCode", FromDistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@StateID", "");
                cmd1.Parameters.AddWithValue("@AreaID", "");
                cmd1.Parameters.AddWithValue("@ZoneID", "");
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteCode", routes);

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                ds1.Tables.Clear();
                da.Fill(ds1);

                if (ds1.Tables.Count > 0)
                {
                    RouteTransSplitGridView.DataSource = ds1.Tables[0];
                    RouteTransSplitGridView.DataBind();

                    RouteTransSplitRetailerGrid.DataSource = ds1.Tables[1];
                    RouteTransSplitRetailerGrid.DataBind();
                }
                else
                {
                    RouteTransSplitGridView.DataSource = null;
                    RouteTransSplitGridView.DataBind();

                    RouteTransSplitRetailerGrid.DataSource = null;
                    RouteTransSplitRetailerGrid.DataBind();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }

        }
        #endregion

        #region ConfirmationDialog
        public void ConfirmationDialog()
        {
            List<string> checkedRoutes = new List<string>();

            DataTable dtforParam = new DataTable();
            dtforParam.Columns.Add("RouteCode", typeof(string));
            dtforParam.Columns.Add("UrCode", typeof(string));
            dtforParam.Columns.Add("RtrCode", typeof(string));

            try
            {

                if (TypeDrp.SelectedValue == "Existing")
                {
                    foreach (GridViewRow row in RouteTransExistGridView.Rows)
                    {
                        HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                        if (chkBox != null && chkBox.Checked)
                        {
                            string routeCode = row.Cells[1].Text;
                            //checkedRoutes.Add(routeCode);

                            existingUr = "";
                            existingRtr = "";

                            dtforParam.Rows.Add(routeCode, existingUr, existingRtr);

                            //anyCheckboxSelected = true;
                            existing = 1;

                            //SP_Route_Transfer_RetailerExistingAnotherDBR(routeCode, existing);
                        }
                    }
                }
                else if (TypeDrp.SelectedValue == "Split")
                {
                    foreach (GridViewRow row in RouteTransSplitRetailerGrid.Rows)
                    {
                        HtmlInputCheckBox chkBox = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                        if (chkBox != null && chkBox.Checked)
                        {
                            string routeCode = row.Cells[1].Text;
                            //checkedRoutes.Add(routeCode);

                            existingUr = row.Cells[4].Text;
                            existingRtr = row.Cells[3].Text;

                            dtforParam.Rows.Add(routeCode, existingUr, existingRtr);

                            //anyCheckboxSelected = true;
                            existing = 0;


                        }
                    }
                }

                //string routecode = string.Join(",", checkedRoutes);

                SP_Route_Transfer_RetailerExistingAnotherDBR(dtforParam);

            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region SP_Route_Transfer_RetailerExistingAnotherDBR
        public void SP_Route_Transfer_RetailerExistingAnotherDBR(DataTable dtforParam)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer_RetailerExistingAnotherDBR", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                cmd1.Parameters.AddWithValue("@ActionType", "RetailerExistingAnotherDBR");
                cmd1.Parameters.AddWithValue("@FromDistCode", FromDistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@ToDistCode", "");
                cmd1.Parameters.AddWithValue("@RouteDetails", dtforParam);
                cmd1.Parameters.AddWithValue("@Distid", "");
                cmd1.Parameters.AddWithValue("@Existing", existing);

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Rows.Clear();
                da.Fill(resdt);

                if (resdt.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#alertModal').modal('show');", true);
                    AlertModalGrid.DataSource = resdt;
                    AlertModalGrid.DataBind();

                    DBR.Text = FromDistDrp.SelectedValue;
                    DBR2.Text = FromDistDrp.SelectedValue;
                    //RouteTransSplitRetailerGrid.DataSource = null;
                    //RouteTransSplitRetailerGrid.DataBind();
                }
                else
                {
                    ProceedTransfer();
                    FromDistDrp.ClearSelection();
                    ToDistDrp.ClearSelection();
                    ToDistSearch.Value = string.Empty;
                }

                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region InActiveRetailerExistingAnotherDBR
        public void InActiveRetailerExistingAnotherDBR()
        {
            try
            {
                DataTable dtforParam1 = new DataTable();
                dtforParam1.Columns.Add("RouteCode", typeof(string));
                dtforParam1.Columns.Add("UrCode", typeof(string));
                dtforParam1.Columns.Add("RtrCode", typeof(string));

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                foreach (GridViewRow row in AlertModalGrid.Rows)
                {
                    string rtrCode = row.Cells[2].Text;
                    string urCode = row.Cells[4].Text;
                    string distId = row.Cells[5].Text;
                    dtforParam1.Rows.Add("", urCode, rtrCode);

                    SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer_RetailerExistingAnotherDBR", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "InActiveRetailerExistingAnotherDBR");
                    cmd1.Parameters.AddWithValue("@FromDistCode", "");
                    cmd1.Parameters.AddWithValue("@ToDistCode", "");
                    cmd1.Parameters.AddWithValue("@RouteDetails", dtforParam1);
                    cmd1.Parameters.AddWithValue("@Distid", distId);
                    cmd1.Parameters.AddWithValue("@Existing", 0);

                    cmd1.ExecuteNonQuery();
                    dtforParam1.Rows.Clear();
                    dtforParam1.AcceptChanges();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region RetailerExceptionLog
        public void RetailerExceptionLog()
        {
            try
            {
                string inActivatedDistcode = FromDistDrp.SelectedValue;
                string inActivatedToDistcode = ToDistDrp.SelectedValue;

                foreach (GridViewRow row in AlertModalGrid.Rows)
                {
                    string inActivatedRtrCode = row.Cells[2].Text;
                    string inActivatedRtrName = row.Cells[3].Text;
                    string inActivatedUrCode = row.Cells[4].Text;
                    string remarks = $"User Approved to Transfer from {inActivatedDistcode} to {inActivatedToDistcode}";//@From to @Todist

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd1 = new SqlCommand("SP_Route_Transfer_Exceptional_Log", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@session_Name", Session["name"].ToString());
                    cmd1.Parameters.AddWithValue("@ActionType", "ExceptionLog");
                    cmd1.Parameters.AddWithValue("@InActivatedDistCode", inActivatedDistcode);
                    cmd1.Parameters.AddWithValue("@InActivatedRtrCode", inActivatedRtrCode);
                    cmd1.Parameters.AddWithValue("@InActivatedRtrName", inActivatedRtrName);
                    cmd1.Parameters.AddWithValue("@InActivatedUrcode", inActivatedUrCode);
                    cmd1.Parameters.AddWithValue("@Remarks", remarks);

                    cmd1.ExecuteNonQuery();

                    con.Close();
                }
                FromDistDrp.ClearSelection();
                ToDistDrp.ClearSelection();
                ToDistSearch.Value = string.Empty;
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        #endregion

        #region ProceedBtn_Click
        protected void ProceedBtn_Click(object sender, EventArgs e)
        {
            InActiveRetailerExistingAnotherDBR();
            ProceedTransfer();
            RetailerExceptionLog();

            RouteTransSplitRetailerGrid.DataSource = null;
            RouteTransSplitRetailerGrid.DataBind();
        }
        #endregion

        #region CancelBtn_Click
        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            return;
        }
        #endregion

        #region ToastNotification
        private void showToast(string message, string styleClass)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showToast", $"showToast('{message}', '{styleClass}');", true);
        }

        #endregion

        #region ClearForm
        private void ClearForm()
        {
            StateDrp.ClearSelection();
            AreaDrp.ClearSelection();
            ZoneDrp.ClearSelection();
            //FromDistDrp.ClearSelection();
            TypeDrp.ClearSelection();
            //ToDistDrp.ClearSelection();

            TypeDrpSelected.Text = string.Empty;
            TypeDrpSelected.Visible = false;
            btnOpenModal.Visible = false;

            RouteTransExistGridView.DataSource = null;
            RouteTransExistGridView.DataBind();

            RouteTransSplitGridView.DataSource = null;
            RouteTransSplitGridView.DataBind();

            ToDistExistViewGrid.DataSource = null;
            ToDistExistViewGrid.DataBind();

            //RouteTransSplitRetailerGrid.DataSource = null;
            //RouteTransSplitRetailerGrid.DataBind();

            RouteTransSplitRetailerGrid.DataSource = null;
            RouteTransSplitRetailerGrid.DataBind();

            btnDivSplit.Visible = false;
            btnDivSingle.Visible = true;

            foreach (GridViewRow row in RouteSplitTransModal.Rows)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)row.FindControl("CheckBox1");
                if (chk != null)
                {
                    chk.Checked = false;
                }
            }
        }

        #endregion

        #region Helpers
        public void RouteTransToDistExistCheck()
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
                cmd1.Parameters.AddWithValue("@ActionType", "ViewExistinfToDistRouteDetails");
                cmd1.Parameters.AddWithValue("@DistCode", "");
                cmd1.Parameters.AddWithValue("@StateID", "");
                cmd1.Parameters.AddWithValue("@AreaID", "");
                cmd1.Parameters.AddWithValue("@ZoneID", "");
                cmd1.Parameters.AddWithValue("@ToDistCode", ToDistDrp.SelectedValue);
                cmd1.Parameters.AddWithValue("@RouteCode", "");

                cmd1.ExecuteNonQuery();

                cmd1.CommandTimeout = 6000;

                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                resdt.Clear();
                da.Fill(resdt);

                Session["RouteDataSet"] = resdt;

                if (resdt.Rows.Count > 0)
                {
                    btnDivSplit.Visible = true;
                    btnDivSingle.Visible = false;

                    View.ToolTip = $"View {ToDistDrp.SelectedValue} Route Details";
                }
                else
                {
                    btnDivSplit.Visible = false;
                    btnDivSingle.Visible = true;

                    ToDistExistViewGrid.DataSource = null;
                    ToDistExistViewGrid.DataBind();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                showToast("An error occurred: " + ex.Message, "toast-danger");
            }
        }
        protected void RouteTransExistGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Create a new row
                GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                // Create a new cell with a colspan that matches the number of columns you want to span
                TableCell headerCell = new TableCell
                {
                    Text = $"From Distributor, {FromDistDrp.SelectedItem.Text} Active Routes and Route Details",
                    ColumnSpan = RouteTransExistGridView.Columns.Count, // Set colspan to the total number of columns
                    HorizontalAlign = HorizontalAlign.Center,
                    CssClass = "font-weight-bold", // Optional: Add CSS class for styling
                    BackColor = System.Drawing.ColorTranslator.FromHtml("#b3d6ff"),
                    ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
                };

                // Add the cell to the row
                headerRow.Cells.Add(headerCell);

                // Insert the row at the top of the GridView
                RouteTransExistGridView.Controls[0].Controls.AddAt(0, headerRow);
            }
        }

        protected void RouteTransSplitRetailerGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Create a new row
                GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                // Create a new cell with a colspan that matches the number of columns you want to span
                TableCell headerCell = new TableCell
                {
                    Text = $"Retailer details for the selected Routes",
                    ColumnSpan = RouteTransSplitRetailerGrid.Columns.Count, // Set colspan to the total number of columns
                    HorizontalAlign = HorizontalAlign.Center,
                    CssClass = "font-weight-bold", // Optional: Add CSS class for styling
                    BackColor = System.Drawing.ColorTranslator.FromHtml("#b3d6ff"),
                    ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
                };

                // Add the cell to the row
                headerRow.Cells.Add(headerCell);

                // Insert the row at the top of the GridView
                RouteTransSplitRetailerGrid.Controls[0].Controls.AddAt(0, headerRow);
            }
        }
        protected void RouteTransSplitGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Create a new row
                GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                // Create a new cell with a colspan that matches the number of columns you want to span
                TableCell headerCell = new TableCell
                {
                    Text = $"From Distributor, {FromDistDrp.SelectedItem.Text} Active Routes and Route Details",
                    ColumnSpan = RouteTransSplitGridView.Columns.Count, // Set colspan to the total number of columns
                    HorizontalAlign = HorizontalAlign.Center,
                    CssClass = "font-weight-bold", // Optional: Add CSS class for styling
                    BackColor = System.Drawing.ColorTranslator.FromHtml("#b3d6ff"),
                    ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
                };

                // Add the cell to the row
                headerRow.Cells.Add(headerCell);

                // Insert the row at the top of the GridView
                RouteTransSplitGridView.Controls[0].Controls.AddAt(0, headerRow);
            }
        }

        protected void ToDistExistViewGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Create a new row
                GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                // Create a new cell with a colspan that matches the number of columns you want to span
                TableCell headerCell = new TableCell
                {
                    Text = $"To Distributor, {ToDistDrp.SelectedItem.Text} existing Routes and Route Details",
                    ColumnSpan = ToDistExistViewGrid.Columns.Count, // Set colspan to the total number of columns
                    HorizontalAlign = HorizontalAlign.Center,
                    CssClass = "font-weight-bold", // Optional: Add CSS class for styling
                    BackColor = System.Drawing.ColorTranslator.FromHtml("#b3d6ff"),
                    ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
                };

                // Add the cell to the row
                headerRow.Cells.Add(headerCell);

                // Insert the row at the top of the GridView
                ToDistExistViewGrid.Controls[0].Controls.AddAt(0, headerRow);
            }
        }
        #endregion
    }
}