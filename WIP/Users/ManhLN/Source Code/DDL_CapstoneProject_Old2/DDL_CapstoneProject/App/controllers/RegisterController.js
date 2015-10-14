"use strict";

app.controller('RegisterController', function ($rootScope,$scope,$location,$window, UserService) {
    // Todo here.
    $scope.Error = null;
    // Submit User model to create new account
    $scope.submit = function () {
        if ($)
            $scope.User.Password = $.md5($scope.Password);

        var promisePost = UserService.register($scope.User);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    $location.path("/register_success").replace();
                } else if (result.data.Status === "error") {
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }
});