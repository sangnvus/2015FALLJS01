"use strict";

directive.directive("ngFileSelect", function ($parse) {
    return {
        link: function ($scope, el, attrs) {
            el.bind("change", function (e) {
                $scope.file = (e.srcElement || e.target).files[0];
                var index = $scope.$eval(attrs.id);
                $scope.getFile($scope.file, index);
            });
        }
    }
})