﻿"use strict";

app.controller('LoginController', function ($scope, $location, AccountService) {
    $scope.Error = null;
    $scope.originalPw = null;
    $scope.save = function () {

        $scope.originalPw = $scope.User.Password
        $scope.User.Password = $.md5($scope.User.Password);
        var promiseLogin = AccountService.Login($scope.User);

        promiseLogin.then(
            function (result) {
                if (result.status == 200) {
                    ////$location.path(result.data).replace();
                    //$stateProvider.go(result.data);
                    $window.location.reload("/");
                }
            },
            function (error) {
                $scope.Error = error.data;
                $scope.User.Password = $scope.originalPw;
            });
    }
});