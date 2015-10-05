"use strict";
var service = angular.module("DDLService", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "DDLService"]);

app.controller('ResetPwController', function ($rootScope, $scope, $location, $window, UserService) {
    // Todo here.
    $scope.Error = null;

    $scope.Message = null;
    // Submit User model to create new account
    $scope.submit = function () {

        var promisePost = UserService.resetPassword($scope.Email);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    //$location.path("/register_success").replace();
                    $scope.Message = "Mật khẩu mới đã được gửi đến email của bạn!";
                    $scope.Error = null;
                } else if (result.data.Status === "error") {
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }
});