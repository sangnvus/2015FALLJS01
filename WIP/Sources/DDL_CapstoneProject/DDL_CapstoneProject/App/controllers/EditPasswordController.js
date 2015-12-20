"use strict";

app.controller('EditPasswordController', function ($scope, $rootScope, $sce, userpublicinfo, UserService, CommmonService, toastr, SweetAlert) {
    $scope.userPass = userpublicinfo.data.Data;
    $scope.rePassword = null;
    $scope.IsChangePassword = false;

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') === 0;
    }

    $scope.submit = function () {
        if ($) {
            $scope.userPass.NewPassword = $.md5($scope.userPass.NewPassword);
            $scope.userPass.NewPasswordConfirm = $.md5($scope.userPass.NewPasswordConfirm);
            $scope.userPass.CurrentPassword = $.md5($scope.userPass.CurrentPassword);
        }

        var promisePost = UserService.changepassword($scope.userPass);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Thay đổi mật khẩu thành công');
                    $scope.userPass.NewPassword = null;
                    $scope.userPass.NewPasswordConfirm = null;
                    $scope.userPass.CurrentPassword = null;
                    $scope.ChangepassForm.$setPristine(true);
                } else {
                    var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    if (a) {
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    }
                }
            },
            function (error) {
                toastr.error('Lỗi');
            });
    }

    $scope.submitNewPass = function () {
        if ($) {
            $scope.userPass.NewPassword = $.md5($scope.userPass.NewPassword);
        }

        var promisePost = UserService.setNewPass($scope.userPass);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Thiết lập mật khẩu thành công');
                    $scope.userPass.NewPassword = null;
                    $scope.userPass.NewPasswordConfirm = null;
                    $scope.userPass.CurrentPassword = null;
                    $scope.ChangepassForm.$setPristine(true);
                } else {
                    var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    if (a) {
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    }
                }
            },
            function (error) {
                toastr.error('Lỗi');
            });
    }

    $scope.ShowChangePW = function changePass() {
        $scope.IsChangePassword = true;
    }

    $scope.SetPassword = function setPass() {
        if ($scope.IsChangePassword === true) return;
        var promise = UserService.sendCodeChangePassword($scope.userPass.Email);
        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    SweetAlert.swal({
                        title: "Xác thực tài khoản",
                        text: "Hãy nhập mã xác nhận bạn nhận được trong email đăng ký",
                        type: "input",
                        showCancelButton: true,
                        closeOnConfirm: false,
                        confirmButtonText: "Xác nhận",
                        cancelButtonText: "Hủy",
                        confirmButtonColor: "#48c9b0",
                        animation: "slide-from-top",
                        inputPlaceholder: "Mã xác nhận"
                    }, function (inputValue) {
                        if (inputValue !== "") {
                            var promise2 = UserService.checkCodeVerify($scope.userPass.Email, inputValue);
                            promise2.then(
                                function (result) {
                                    if (result.data.Status === "success") {
                                        $scope.IsChangePassword = true;
                                        swal.close();
                                    } else if (result.data.Status === "error") {
                                        var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                                        if (a) {
                                            $scope.Error = result.data.Message;
                                            toastr.error($scope.Error, 'Lỗi');
                                        }
                                    }
                                },
                                function (error) {
                                    $scope.Error = error.data.Message;
                                });
                        } else {
                            toastr.warning('Bạn chưa nhập mã xác nhận');
                        }
                    });
                } else if (result.data.Status === "error") {
                    var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    if (a) {
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    }
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }
    // $scope.Error = null;
});