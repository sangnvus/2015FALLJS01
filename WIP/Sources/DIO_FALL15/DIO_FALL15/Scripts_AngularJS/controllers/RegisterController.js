"use strict";

app.controller('RegisterController', function($scope, AccountService) {

    $scope.Gender = 1;

    $scope.save = function () {

        var promisePost = AccountService.post($scope.User);

        promisePost.then(
            function (result) {
                alert("Sucessfully!");
            },
            function (error) {
                $scope.error = 'failure loading User', error;
            });
    }
});