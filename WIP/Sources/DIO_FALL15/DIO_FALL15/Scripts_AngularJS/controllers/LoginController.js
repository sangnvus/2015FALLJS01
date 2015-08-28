"use strict";

var app = angular.module("LoginApp", ["ngRoute"]);
//app.controller('LoginController', function ($scope, $location,$route, AccountService) {
//    $scope.Error = null;
//    $scope.originalPw = null;
//    $scope.save = function () {

//        $scope.originalPw = $scope.User.Password
//        $scope.User.Password = $.md5($scope.User.Password);
//        var promiseLogin = AccountService.Login($scope.User);

//        promiseLogin.then(
//            function (result) {
//                if (result.status == 200) {
//                    $location.path(result.data).reload();
//                }
//            },
//            function (error) {
//                $scope.Error = error.data;
//                $scope.User.Password = $scope.originalPw;
//            });
//    }
//});