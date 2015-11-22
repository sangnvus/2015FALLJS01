<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckOut.aspx.cs" Inherits="CheckOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    
    <link href="StyleSheet2.css" rel="stylesheet" type="text/css" />
   <script src="SpryAssets/SpryTabbedPanels.js" type="text/javascript"></script>
    <link href="SpryAssets/SpryTabbedPanels.css" rel="stylesheet" type="text/css" />
   <script src="jquery.js" type="text/javascript"></script>
   <script src="jquery.js" type="text/javascript"></script>
    <style type="text/css">
        .title_type
        {
            font-family: Tahoma;
            font-size: 13px;
            color: #0072BC;
            cursor: pointer;
            width: 606px;
            padding: 3px 0px 3px 0px;
            border-bottom: solid #004787 thin;
        }
        
        .content_bank
        {
            background: transparent;
            width: 663px;
            padding: 5px;
        }
        
        .bank
        {
            display: inline;
            margin: 5px;
        }
        .style1
        {
            width: 206px;
        }
        .style2
        {
            width: 258px;
        }
        .sopin1-bk1
        {
            width: 180px;
            height: 37px;
            padding: 20px 0 0 10px;
            float: left;
            font-family: Tahoma,Geneva,sans-serif;
            font-size: 14px;
            font-weight: bold;
            text-align: center;
            color: #3B6298;
            font-weight: bold;
        }
         .sopin2-bk1
        {
	        background:url(images/input.png) no-repeat;
	        width:453px;
	        height:45px;
	        margin-top:10px;
	        float: left;
	        padding:2px 0 0 2px;
        }
        .sopin2-bk1 input
        {
	        width:344px;
	        height:27px;
	        border: 1px solid #FFFFFF;
	        color:#3B6298;
	        font-size:16px;
	        font-weight:bold;
            ! padding-top:1px;
        }
        .bk_logo
        {
            display:block;
            padding-bottom: 20px; border: solid #8EA2B5 thin; padding-top: 15px; padding-left: 10px;
           }   
            .btn_bk {
            border: #0C6 2px groove !important;

        border: #0C6 1px groove;
        }
        .btn_bk:hover {


        border: #0C6 5px groove !important;
        }
    </style>
    <script src="JScript.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <div id="cover">
        <div id="form_thongtin" class="form_ttthanhtoan" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td align="center" colspan="3">
                        <div id="Div1" class="title" runat="server">
                            <h1>
                                Thông Tin Thanh Toán</h1>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="style1" align="right">
                        &nbsp; Email
                    </td>
                    <td class="style2">
                        &nbsp;
                        <asp:TextBox ID="tbxEmail" runat="server" Width="233px"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbxEmail"
                            ErrorMessage="Not null" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style1" align="right">
                        &nbsp; Phone
                    </td>
                    <td class="style2">
                        &nbsp;
                        <asp:TextBox ID="tbxPhone" runat="server" Width="233px"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbxPhone"
                            ErrorMessage="Not null" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style1" align="right">
                        Số Tiền
                    </td>
                    <td class="style2">
                        &nbsp;
                        <asp:TextBox ID="tbxMoney" runat="server" Width="233px"></asp:TextBox>
                    </td>
                    <td>
                        <%--  &nbsp;&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="tbxMoney" ErrorMessage="Not null" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;
                    </td>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;
                    </td>
                    <td class="style2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnTiepTuc" runat="server" Text="Tiếp Tục" OnClick="btnTiepTuc_Click" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div id="title_htthanhtoan" runat="server">
            <div class="title">
                <h1>
                    Các Hình Thức Thanh Toán</h1>
            </div>
            <div id="tab_menu">
                <div id="TabbedPanels1" class="TabbedPanels">
                    <ul class="TabbedPanelsTabGroup">
                        <li class="TabbedPanelsTab" tabindex="0"><div style="float: left;"><img src="images/trongnuoc.png"/></div><div>Ngân Hàng Trong Nước</div></li>
                        <li class="TabbedPanelsTab" tabindex="0"><div style="float: left;"><img src="images/visa.png"/></div><div>Thẻ quốc tế</div></li>
                         <li class="TabbedPanelsTab" tabindex="0"><div style="float: left;"><img src="images/baokim.png"/></div><div>Sử dụng tài khoản Bảo Kim</div></li>
                        <%--<li class="TabbedPanelsTab" tabindex="0">Chuyển qua ATM</li>--%>
                        <%--<li class="TabbedPanelsTab" tabindex="0">Tài khoản Bảo Kim</li>--%>
                    </ul>
                    <div class="TabbedPanelsContentGroup" style="width: 650px; height:auto;">
                        <div class="TabbedPanelsContent">
                            <div id="divBanks-1" class="content_bank">  
                                <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="TabbedPanelsContent">
                            <div id="divBanks-2" class="content_bank">
                                <asp:Literal ID="Literal3" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="TabbedPanelsContent">
                            <div id="divBanks-3" class="content_bank">
                                <asp:ImageButton ID="Baokim_Paymnet" runat="server" ImageUrl="https://www.baokim.vn/application/themes/baokim/img/img_new/_logo.png" ToolTip="Click vào đây để sang Bảo Kim thanh toán" OnClick="Baokim_Paymnet_click" />
                            </div>
                        </div>
                        <div class="TabbedPanelsContent">
                            <div id="divBanks-4" class="content_bank">
                                <div class="bank">
                                    <a id="hrefBank-2" href="Pay.aspx?payment_method_id=2>">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/40_1259006007.jpg" title="Ngân hàng TMCP Ngoại thương - Chuyển khoản tại máy ATM"></a></div>
                                <div class="bank">
                                    <a id="hrefBank-35" href="Pay.aspx?payment_method_id=35">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/35_1258746886.jpg" title="Ngân hàng Kỹ thương Việt Nam - Chuyển khoản tại máy ATM"></a></div>
                                <div class="bank">
                                    <a id="hrefBank-41" href="Pay.aspx?payment_method_id=41">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/70_1258747652.jpg" title="Ngân hàng Công thương Việt Nam - Chuyển khoản tại máy ATM"></a></div>
                                <div class="bank">
                                    <a id="hrefBank-51" href="Pay.aspx?payment_method_id=51">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/5_1258747469.jpg" title="Ngân hàng Đầu tư và Phát triển Việt Nam - Chuyển khoản tại máy ATM"></a></div>
                                <div class="bank">
                                    <a id="hrefBank-74" href="Pay.aspx?payment_method_id=74">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/38_1258924253.jpg" title="Ngân hàng Quốc Tế - Chuyển khoản tại máy ATM"></a></div>
                                <div class="bank">
                                    <a id="hrefBank-75" href="Pay.aspx?payment_method_id=75">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/a472a10baec675cdbeed43ccf7507ae1.jpg"
                                            title="Ngân Hàng TMCP Xăng Dầu - PG Bank - Chuyển khoản tại máy ATM"></a></div>
                                <div class="bank">
                                    <a id="hrefBank-78" href="Pay.aspx?payment_method_id=78">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/12016d01c9923f58d70537ffe361ad99.png"
                                            title="Ngân hàng Nông nghiệp và Phát triển Nông thôn Việt Nam - Chuyển khoản tại máy ATM"></a></div>
                                <div class="bank">
                                    <a id="hrefBank-90" href="Pay.aspx?payment_method_id=90">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/bfbe6befecdd7a2d5378fbbd9ebc5efd.jpg"
                                            title="Ngân hàng Quân Đội - Chuyển khoản tại máy ATM"></a></div>
                                <div class="bank">
                                    <a id="hrefBank-100" href="Pay.aspx?payment_method_id=100">
                                        <img src="http://sandbox.baokim.vn/application/uploads/banks/621595849564cac2ba3b8243c3976781.jpg"
                                            title="Ngân hàng Đông Á - Chuyển khoản tại máy ATM"></a></div>
                            </div>
                        </div>
                        <div class="TabbedPanelsContent">
                            <div id="divBanks-5" class="content_bank">
                                <div class="sopin1-bk1">
                                    Email tài khoản Bảo Kim:
                                </div>
                                <div class="sopin2-bk1">
                                    <asp:TextBox ID="tbxMail" ValidationGroup="group1" ClientIDMode="Static" runat="server"></asp:TextBox>
                                </div>
                                <div class="bk_logo" style="padding-bottom: 20px; border: solid #8EA2B5 thin; padding-top: 15px;
                                    padding-left: 220px;">
                                    <asp:ImageButton ID="btnBaokim" runat="server" ImageUrl="https://www.baokim.vn/application/themes/baokim/img/img_new/_logo.png"
                                        ValidationGroup="2" ToolTip="Thanh toán qua Bảo Kim" OnClick="btnBaokim_Click"
                                        class="btn_bk" />
                                    <br />
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var TabbedPanels1 = new Spry.Widget.TabbedPanels("TabbedPanels1");
    </script>
    </form>
    <div id="login-box" class="login-popup" style="width:655px;">
    <input type="hidden" id="hidden" value="" />
        <div style="text-align: right; padding-right: 5px;"><a href="#" class="close"><img src="images/close.png" class="btn_close" title="Close Window" alt="Close" /></a></div>
        <div class="head"></div> 
          <form method="post" class="signin" action="#"> 
                <fieldset class="textbox">
                <a id="A3" href="#">
                        <img class="img-b" src="images/tienmat.png" title="Thanh toán online bằng thẻ ATM"  alt=""/>
                    </a>
                    <a id="A4" href="#">
                        <img class="img-b" src="images/ATM_offline.png" title="Thanh toán online bằng thẻ ATM"  alt=""/>
                    </a>
                    <a id="A1" href="#">
                        <img class="img-b" src="images/ATM_h.png" title="Thanh toán online bằng thẻ ATM"  alt=""/>
                    </a>                    
                    <a id="A2" class="img-b" href="#">
                        <img src="images/Banking_h.png" title="Thanh toán online bằng thẻ ATM" alt=""/>
                    </a>
                </fieldset> 
          </form> 
         
</div>   
</body>
</html>

<script type="text/javascript" language="javascript">
    $(function () {
        $('.checkall').click(function () {
            var a = $('.checkall').attr("checked");
            alert(a);
            if (a) {
                $('.check').attr("checked", true);
            } else {
                $('.check').attr("checked", false);
            }
        });
        $('#DeleteAll').click(function () {
            var a = $(this).attr('id');
            jConfirm('Bạn có muốn xóa bản ghi này không?', 'Thông báo', function (r) {
                if (r) {
                    var numberOfChecked = $('input:checkbox:checked').length;
                    if (numberOfChecked == 0) {
                        jAlert('Bạn chưa chọn bản ghi nào để xóa', 'Thông Báo', function () { });
                    } else {
                        $('#listadv').submit();
                    }

                }
            });

        });
    });
    
</script>