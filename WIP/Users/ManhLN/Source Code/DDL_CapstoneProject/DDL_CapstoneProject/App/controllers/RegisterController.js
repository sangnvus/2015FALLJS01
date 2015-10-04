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
}).directive('passwordMatch', [function () {
    return {
        restrict: 'A',
        scope: true,
        require: 'ngModel',
        link: function (scope, elem, attrs, control) {
            var checker = function () {

                //lấy giá trị của trường mật khẩu
                var e1 = scope.$eval(attrs.ngModel);

                //lấy giá trị của xác nhận mật khẩu
                var e2 = scope.$eval(attrs.passwordMatch);
                return e1 == e2;
            };
            scope.$watch(checker, function (n) {

                //thiết lập form control
                control.$setValidity("unique", n);
            });
        }
    };
}]);