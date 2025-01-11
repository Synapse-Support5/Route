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
    public partial class Home : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"].ToString());
        DataTable resdt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
        }

    }
}