using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using vn.baokim.www;

public partial class CheckOut : System.Web.UI.Page
{
    protected JArray Banks = new JArray();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            title_htthanhtoan.Visible = false;
            load();
        }
        this.ClientScript.RegisterStartupScript(this.GetType(), "init_banks", "$.banks ]= [];", true);
    }

    protected void btnTiepTuc_Click(object sender, EventArgs e)
    {
        Session["email_payer"]= tbxEmail.Text;
        Session["price_bk"]= tbxMoney.Text;
        Session["phone"]= tbxPhone.Text;
        Session["description"]= "";
        form_thongtin.Visible = false;
        title_htthanhtoan.Visible = true;
        

    }
    protected void Baokim_Paymnet_click(object sender, EventArgs e)
    {
        var bk = new BaoKimPayment();
        String order_id = DateTime.Now.ToLongDateString();//mã đơn hàng
        String business= SessionKey.Business;//email bảo kim nhận tiền
        String total_amount=tbxMoney.Text;
        String shipping_fee="";//phí vận chuyển
        String tax_fee="";//thuế khác
        String order_description="";//mo ta đơn hàng
        String url_success = "";//url trả về khi thanh toán thành công
        String url_cancel = "";//url trả về khi thanh toán thất bại
        String url_detail="";
        Response.Redirect(bk.createRequestUrl(order_id, business, total_amount, shipping_fee, tax_fee, order_description, url_success, url_cancel, url_detail));
    }
    protected void btnBaokim_Click(object sender, EventArgs e)
    { 
    
    }
    protected void banks_click(object sender, EventArgs e)
    {

    }
    protected void load()
    {
        string str = "";
        string str1 = "";
        AccountInfoRequest accountinforequest = new AccountInfoRequest();
        accountinforequest.api_password = SessionKey.apipass;
        accountinforequest.api_username = SessionKey.apiuser;
        accountinforequest.merchant_id = SessionKey.merchantid;
        AccountInfoResponse accountinforespone = new AccountInfoResponse();
        BKPaymentProService2 bk=new BKPaymentProService2();
        accountinforespone=bk.GetAccountInfo(accountinforequest);
        if (accountinforespone.error_code == "0")
        {
            PaymentMethod[] paymnet_id = accountinforespone.account_info.payment_methods;

            foreach (var item in paymnet_id)
            {
                string url = item.logo_url;
                string id = item.id;
                string name = item.name;
                /*1 =>"Các loại thẻ ATM trong nước",
                2 =>"Các loại thẻ Tín dụng quốc tế",
                3 =>"Chuyển khoản bằng Internet Banking",
                4 =>"Chuyển khoản bằng máy ATM",
                5 =>"Chuyển khoản tiền mặt tại quầy giao dịch"
                 * */
                if (item.payment_method_type == "1")//so sanh xem nó là hình thức nào có 5 hình thức là atm, interbanking 
                {
                    

                    str = str + " <div class='bank'><a title='"+name+"' href='Pay.aspx?payment_method_id=" + id + "' style='padding-left:5px; padding-bottom:5px;'><img class='img-v' src='" + url + "' /></a></div>";
                    Literal2.Text = str;
                    //... anh lấy nó ra
                }
                else if (item.payment_method_type == "2")
                {
                    str1 = str1 + " <div class='bank'><a title='"+name+"' href='Pay.aspx?payment_method_id=" + id + "' style='padding-left:5px; padding-bottom:5px;'><img class='img-v' src='" + url + "' /></a></div>";
                    Literal3.Text = str1;
                }
                
               
            }
        }
        else
            Literal2.Text = accountinforespone.error_message;       
    }
}