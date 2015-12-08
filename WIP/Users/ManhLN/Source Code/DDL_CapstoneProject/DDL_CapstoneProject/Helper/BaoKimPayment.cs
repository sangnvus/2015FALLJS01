using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Specialized;

/**
 *	
 *		Phiên bản: 0.1   
 *		Tên lớp: BaoKimPayment
 *		Chức năng: Tích hợp thanh toán qua baokim.vn cho các merchant site có đăng ký API
 *						- Xây dựng URL chuyển thông tin tới baokim.vn để xử lý việc thanh toán cho merchant site.
 *						- Xác thực tính chính xác của thông tin đơn hàng được gửi về từ baokim.vn.
 *		
 */
class BaoKimPayment
{
    //URL checkout của BaoKim.vn
    private string baokim_url = "http://kiemthu.baokim.vn/payment/customize_payment/order";

    //Mã merchant site
    //private string merchant_id = "20033";	//Thay bằng mã merchant site bạn đã đăng ký trên BaoKim.vn
    private string merchant_id = "647";	//Thay bằng mã merchant site bạn đã đăng ký trên BaoKim.vn

    //Mật khẩu bảo mật
    //private string secure_pass = "b6eb222debf4bdc0";	//Thay bằng mật khẩu giao tiếp giữa website của bạn với BaoKim.vn
    private string secure_pass = "ae543c080ad91c23";	//Thay bằng mật khẩu giao tiếp giữa website của bạn với BaoKim.vn

    /**
     * Hàm thực hiện việc mã hóa, tạo khóa trên đường dẫn
     * @param messages xâu gốc
     * @return kết quả mã hóa
     * @throws Exception
     */
    private string GetMD5Hash(String input)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();

        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }

        String md5String = s.ToString();
        return md5String;
    }

    /**
     * Hàm xây dựng url chuyển đến BaoKim.vn thực hiện thanh toán, trong đó có tham số mã hóa (còn gọi là public key)
     * @param $order_id				Mã đơn hàng
     * @param $business 			Email tài khoản người bán
     * @param $total_amount			Giá trị đơn hàng
     * @param $shipping_fee			Phí vận chuyển
     * @param $tax_fee				Thuế
     * @param $order_description	Mô tả đơn hàng
     * @param $url_success			Url trả về khi thanh toán thành công
     * @param $url_cancel			Url trả về khi hủy thanh toán
     * @param $url_detail			Url chi tiết đơn hàng
     * @return url cần tạo
     */
    public String createRequestUrl(String order_id, String business, String total_amount, String shipping_fee, String tax_fee, String order_description, String url_success, String url_cancel, String url_detail)
    {
        Hashtable order_params = new Hashtable();
        order_params.Add("merchant_id", this.merchant_id);
        order_params.Add("order_id", order_id);
        order_params.Add("business", business);
        order_params.Add("total_amount", total_amount);
        order_params.Add("shipping_fee", shipping_fee);
        order_params.Add("tax_fee", tax_fee);
        order_params.Add("order_description", order_description);
        order_params.Add("url_success", url_success);
        order_params.Add("url_cancel", url_cancel);
        order_params.Add("url_detail", url_detail);

        //Sắp xếp các tham số theo key để tiến hành mã hóa
        ICollection keyCollection = order_params.Keys;
        string[] keys = new string[keyCollection.Count];
        keyCollection.CopyTo(keys, 0);
        Array.Sort(keys);

        //Mã hóa tạo checksum
        String str_combined = this.secure_pass;
        foreach (string key in keys)
        {
            Object value = order_params[key];
            str_combined += value.ToString();
        }
        String checksum = this.GetMD5Hash(str_combined).ToUpper();


        //Tạo url redirect
        String redirect_url = this.baokim_url;

        if (redirect_url.IndexOf("?") == -1)
        {
            redirect_url += "?";
        }
        else if (redirect_url.Substring(redirect_url.Length - 1, 1) != "?" && redirect_url.IndexOf("&") == -1)
        {
            redirect_url += "&";
        }

        String url_params = "";
        foreach (string key in keys)
        {
            Object value = order_params[key];
            if (url_params == "")
                url_params += key.ToString() + "=" + HttpContext.Current.Server.UrlEncode(value.ToString());
            else
                url_params += "&" + key.ToString() + "=" + HttpContext.Current.Server.UrlEncode(value.ToString());
        }
        url_params += "&checksum=" + checksum;

        return redirect_url + url_params;
    }

    /**
     * Hàm thực hiện xác minh tính chính xác thông tin trả về từ BaoKim.vn
     * @param $_GET chứa tham số trả về trên url
     * @return true nếu thông tin là chính xác, false nếu thông tin không chính xác
     */
    public Boolean verifyResponseUrl(NameValueCollection get_params)
    {
        //Sắp xếp các phần tử trong mảng tham số trả về theo key để mã hóa
        ICollection keyCollection = get_params.Keys;
        string[] keys = new string[keyCollection.Count];
        keyCollection.CopyTo(keys, 0);
        Array.Sort(keys);

        string str_combined = this.secure_pass;
        string checksum = "";
        foreach (string key in keys)
        {
            if (String.Compare(key, "checksum", true) != 0)
            {
                Object value = get_params[key];
                str_combined += value.ToString();
            }
            else
            {
                checksum = get_params[key].ToString();
            }
        }

        //Mã hóa tạo check sum, so sánh với checksum gửi về từ BaoKim.vn
        string verify_checksum = this.GetMD5Hash(str_combined);

        if (String.Compare(verify_checksum, checksum, true) == 0)
            return true;

        return false;
    }
}