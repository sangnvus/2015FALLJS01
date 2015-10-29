"use strict";

app.controller('RegisterController', function ($rootScope, $scope, $location, $window, UserService, CommmonService,toastr) {
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
                    var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    if (a) {
                        $scope.Error = result.data.Message;
                    }
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi');
            });
    }
});