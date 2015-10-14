"use strict";
directive.directive('timelinerDirective', function () {
    return function (scope, element, attrs) {
        var id = angular.element(element).id;
        $(function () {
            $().timelinr({
                containerDiv: '#'+id,
            });
        });
    };
});

//angular.module('myApp', [])
//.directive('myRepeatDirective', function () {
//    return function (scope, element, attrs) {
//        angular.element(element).css('color', 'blue');
//        if (scope.$last) {
//            window.alert("im the last!");
//        }
//    };
//})