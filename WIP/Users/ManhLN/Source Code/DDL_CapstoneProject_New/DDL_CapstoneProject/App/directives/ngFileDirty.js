directive.directive('ngFileDirty', function () {
    return {
        require: '^form',
        transclude: true,
        link: function ($scope, elm, attrs, formCtrl) {
            elm.on('change', function () {
                formCtrl.$setDirty();
                $scope.$apply();
            });
        }
    }
})