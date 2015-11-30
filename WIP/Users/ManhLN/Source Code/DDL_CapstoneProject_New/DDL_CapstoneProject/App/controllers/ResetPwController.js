"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "DDLService", 'DDLDirective', 'angular-loading-bar', 'blockUI','oitozero.ngSweetAlert']);

app.controller('ResetPwController', function ($rootScope, $scope, $location, $window, UserService, CommmonService) {
    // Todo here.
    $scope.Error = null;

    $scope.Message = null;
    $scope.IsSent = false;
    $scope.IsWrongCode = false;
    // Submit email to reset.
    $scope.submit = function () {

        var promisePost = UserService.resetPasswordCode($scope.Email);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    //$location.path("/register_success").replace();
                    $scope.Message = "Mã code được gửi đến email của bạn";
                    $scope.IsSent = true;
                    $scope.IsWrongCode = false;
                    $scope.Error = null;
                } else if (result.data.Status === "error") {
                    var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    if (a) {
                        $scope.Error = result.data.Message;
                    }
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }

    // Submit reset code.
    $scope.submitCode = function () {
        var promisePost = UserService.resetPassword($scope.Email,$scope.Code);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    //$location.path("/register_success").replace();
                    $scope.Message = "Mật khẩu mới đã được gửi đến email của bạn!";
                    $scope.IsSent = false;
                    $scope.Code = null;
                    $scope.IsWrongCode = false;
                    $scope.Error = null;
                } else if (result.data.Status === "error") {
                    if (result.data.Type === "wrong-code") {
                        $scope.IsWrongCode = true;
                    } else {
                        var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        if (a) {
                            $scope.Error = result.data.Message;
                        }
                    }
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }
});