"use strict";

app.controller('EditProfileController', function ($scope, $location,$window, AccountService) {

    function loadCurrentUserData() {
        var promiseGetProfile = AccountService.getCurrentAccount();
        promiseGetProfile.then(
            function (result) {
                $scope.User = result.data;
                $scope.User.Gender = $scope.User.Gender.toString();
            },
            function (error) {
                if (error.status == 401) {
                    $window.location.href = "http://localhost:14069/Account/Login";
                } else {
                    $scope.error = error;
                }
            });
    }
    loadCurrentUserData();

    $scope.Success = null;
    $scope.Error = null;
    $scope.save = function () {

        var promisePut = AccountService.EditAccount($scope.User.Id, $scope.User);

        promisePut.then(
            function (result) {
                if (result.status == "200") {
                    $scope.Success = result.data;
                    $scope.EditProfileForm.$setPristine();
                }
            },
            function (error) {
                if (error.data.Message != null) {
                    $scope.Error = error.data.Message;
                } else {
                    $scope.Error = error.data;
                }
                
            });
    }

    $scope.removeMessage = function () {
        $scope.Success = null;
        $scope.Error = null;
    }
});