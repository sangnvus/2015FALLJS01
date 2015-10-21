"use strict";

app.controller('EditPasswordController', function ($scope, $sce, userpublicinfo, UserService, toastr) {
    $scope.userPass = userpublicinfo.data.Data;
    $scope.rePassword = null;
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
                    toastr.error('Mật khẩu hiện tại sai');
                }
            })
    }

    // $scope.Error = null;
});