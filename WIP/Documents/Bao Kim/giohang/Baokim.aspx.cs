using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Baokim : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                string tr = Request.QueryString["url_guide"].ToString();
                Response.Redirect(tr);
            }
            catch
            {
                Response.Redirect("http://google.com");
            }
            
        }
    }
}