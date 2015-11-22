
        $(document).ready(function () {
            $('a.login-window').click(function () {

                //Getting the variable's value from a link 
                var loginBox = $(this).attr('href');

                //Fade in the Popup 
                $(loginBox).fadeIn(1200);

                //Set the center alignment padding + border see css style 
                var popMargTop = ($(loginBox).height() + 24) / 2;
                var popMargLeft = ($(loginBox).width() + 24) / 2;

                $(loginBox).css({
                    'margin-top': -popMargTop,
                    'margin-left': -popMargLeft
                });

                // Add the mask to body 
                $('body').append('<div id="mask"></div>');
                $('#mask').fadeIn(1000);

                //$('#A2').css("display", "none");
                var a = $(this).attr('id');
                $('#hidden').val(a);
                if (a == "B5" || a == "B21" || a == "B22" || a == "B23" || a == "B20") {
                    $('#A1').css("display", "none");
                }
//                if (a == "B4" || a == "B6" || a == "B8" || a == "B9" || a == "B10" || a == "B11" || a == "B15" || a == "B14" || a == "B19" || a == "B20" || a == "B25" || a == "B26" || a == "B27") {
//                    $('#A2').css("display", "none");
//                }
                if (a!="B7" && a!="B2" && a!="B16") {
                    $('#A2').css("display", "none");
                }
                if (a == "B6" || a == "B9" || a == "B10" || a == "B14" || a == "B15" || a == "B16" || a == "B19" || a == "B20" || a == "B21" || a == "B25" || a == "B27") {
                    $('#A3').css("display", "none");
                }
                if (a == "B3" || a == "B9" || a == "B5" || a == "B8" || a == "B10" || a == "B11" || a == "B12" || a == "B14" || a == "B15" || a == "B16" || a == "B18" || a == "B19" || a == "B22" || a == "B23" || a == "B25" || a == "B27") {
                    $('#A4').css("display", "none");
                }

                //=================================================

                $('#A1').click(function () {
                    var idm = 0;
                    if (a == "B1") {
                        idm = 15;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B2") {
                        idm = 60;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B3") {
                        idm = 64;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B4") {
                        idm = 112;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B7") {
                        idm = 101;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B8") {
                        idm = 63;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B9") {
                        idm = 115;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B11") {
                        idm = 66;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B10") {
                        idm = 94;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B12") {
                        idm = 105;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B13") {
                        idm = 61;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B14") {
                        idm = 96;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B15") {
                        idm = 106;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B16") {
                        idm = 124;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B17") {
                        idm = 102;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B18") {
                        idm = 98;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B19") {
                        idm = 97;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B24") {
                        idm = 62;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B25") {
                        idm = 114;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B26") {
                        idm = 91;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B27") {
                        idm = 113;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    }
                });

                //------------------------------------------------------------------

                $('#A2').click(function () {
                    var idm = 0;
                    if (a == "B1") {
                        idm = 99;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B2") {
                        idm = 36;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B3") {
                        idm = 54;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B5") {
                        idm = 82;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B7") {
                        idm = 32;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B12") {
                        idm = 84;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B13") {
                        idm = 65;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B16") {
                        idm = 125;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B17") {
                        idm = 81;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B18") {
                        idm = 80;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B21") {
                        idm = 49;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B22") {
                        idm = 45;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B23") {
                        idm = 56;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B24") {
                        idm = 47;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    }
                });

                //----------------------------------------------------

                $('#A3').click(function () {
                    var idm = 0;
                    if (a == "B1") {
                        idm = 1;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B2") {
                        idm = 34;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B3") {
                        idm = 52;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B4") {
                        idm = 77;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B5") {
                        idm = 70;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B6") {
                        idm = 50;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B7") {
                        idm = 9;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B8") {
                        idm = 59;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B12") {
                        idm = 57;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B13") {
                        idm = 42;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B17") {
                        idm = 73;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B18") {
                        idm = 67;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B23") {
                        idm = 55;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B22") {
                        idm = 43;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B24") {
                        idm = 46;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B26") {
                        idm = 40;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    }

                });
                $('#A4').click(function () {
                    var idm = 0;
                    if (a == "B1") {
                        idm = 2;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B2") {
                        idm = 35;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B4") {
                        idm = 78;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B6") {
                        idm = 51;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B7") {
                        idm = 100;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B13") {
                        idm = 90;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B17") {
                        idm = 75;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B20") {
                        idm = 72;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B21") {
                        idm = 48;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B24") {
                        idm = 74;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    } else if (a == "B26") {
                        idm = 41;
                        $(this).attr("href", "Pay.aspx?payment_method_id=" + idm);
                    }

                });
                return false;
            });

            // When clicking on the button close or the mask layer the popup closed 
            $('a.close, #mask').live('click', function () {
                $('#mask , .login-popup').fadeOut(400, function () {
                    $('#mask').remove();
                    $('#A4').css("display", "block");
                    $('#A2').css("display", "block");
                    $('#A3').css("display", "block");
                    $('#A1').css("display", "block");
                });
                return false;
            });
        });  
