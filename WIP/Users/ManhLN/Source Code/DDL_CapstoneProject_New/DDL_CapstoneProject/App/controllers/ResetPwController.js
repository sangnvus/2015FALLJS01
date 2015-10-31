"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "DDLService", 'DDLDirective', 'angular-loading-bar', 'blockUI','oitozero.ngSweetAlert']);

app.controller('ResetPwController', function ($rootScope, $scope, $location, $window, UserService, CommmonService) {
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
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }
});