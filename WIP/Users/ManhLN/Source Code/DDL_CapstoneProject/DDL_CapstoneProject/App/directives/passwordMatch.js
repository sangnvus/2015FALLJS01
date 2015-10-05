"use strict";

directive.directive('passwordMatch', [function () {
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
}])