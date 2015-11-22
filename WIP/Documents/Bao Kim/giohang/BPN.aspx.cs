using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Common.Logging;
using System.Net;
using System.Text;
using System.IO;

public partial class BPN : System.Web.UI.Page
{
   // ILog log = LogManager.GetLogger("BPN");
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            //Chay test thi su dung sandbox
            //string strSandbox = "http://sandbox.baokim.vn/bpn/verify";

            //Dia chi chay that
            string strLive = "https://www.baokim.vn/bpn/verify";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strLive);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);
            //log.Info("strRequest: " + strRequest);
            req.ContentLength = strRequest.Length;

            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadLine();
            streamIn.Close();

            //log.Info("strResponse: " + strResponse);

            if (strResponse == "VERIFIED")
            {

                string orderID = Request["order_id"];//mã đơn hàng đã thanh toán
                string transactionID = Request["transaction_id"];
                string transaction_status = Request["transaction_status"];//trạng thái thanh toán
                string ngaythanhtoan =DateTime.Now.ToString();
                string status="Đã Thanh Toán";
                if (transaction_status=="4")
                {
                    //cập nhật trạng thái đơn hàng
                    //string sql = "update tenbang set madon='" + orderID + "',donhangbaokim='" + transactionID + "',ngaygio='" + ngaythanhtoan + "',trangthai='" + status + "'";
                }
                
            }
            else if (strResponse == "INVALID")
            {
                //thong tin khong hop le. Log lai de theo doi                       
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            //log.Error(ex);
        }  
    }
}
