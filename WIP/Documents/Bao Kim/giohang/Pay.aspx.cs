using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
//using vn.baokim.kiemthu;// dùng cho kiểm thử
using vn.baokim.www;// đây là khi đưa lên public

public partial class Pay : System.Web.UI.Page
{
    protected void callDoPaymentPro(int pmi)
    {

            pmi = int.Parse(this.Request.QueryString["payment_method_id"]);
            string tm = DateTime.Now.ToString();
            PaymentInfoRequest url = new PaymentInfoRequest();
            url.api_username = SessionKey.apiuser;
            url.api_password = SessionKey.apipass;
            url.bank_payment_method_id =  pmi.ToString();
            url.bk_seller_email = SessionKey.Business;
            url.currency_code = "VND";
            url.escrow_timeout = "";
            url.extra_fields_value = "";
            url.merchant_id = SessionKey.merchantid;
            url.order_description = "";
            url.order_id = tm;
            url.payer_email = Session["email_payer"].ToString();
            url.payer_message = "";
            url.payer_name = "test";
            url.payer_phone_no = Session["phone"].ToString();
            url.payment_mode = "1";
            url.shipping_address = "";
            url.shipping_fee = "";
            url.tax_fee = "";
            url.total_amount = Session["price_bk"].ToString();
            url.url_return = "";
            //url.url_return = "http://localhost:52996/WebSite3/Baokim.aspx";
            BKPaymentProService2 bk=new BKPaymentProService2();
            PaymentInfoResponse baokim = new PaymentInfoResponse();
             baokim=bk.DoPaymentPro(url);
             string link = baokim.url_redirect;
             if (baokim.error_code != "0") { throw new Exception(baokim.error_message); }
             else
             {
                 Response.Redirect(baokim.url_redirect);
             }
             
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["price_bk"].ToString() == "")
            {
                Literal1.Text = "<script>alert('số tiền phải lớn hơn 10.000 VND!');window.location = 'http://localhost/';</script>";
            }
            else if (Session["email_payer"].ToString() == "")
            {
                Literal1.Text = "<script>alert('Bạn chưa nhập Email ');window.location = 'http://localhost/';</script>;";
            }
            else if (Session["phone"].ToString() == "")
            {
                Literal1.Text = "<script>alert('Bạn chưa nhập Số Điện Thoại!');window.location = 'http://localhost/';</script>;";
            }
            else
            {
                
                    int pmi = int.Parse(this.Request.QueryString["payment_method_id"]);
                    callDoPaymentPro(pmi);
                    
            
            }
        }
    }
}