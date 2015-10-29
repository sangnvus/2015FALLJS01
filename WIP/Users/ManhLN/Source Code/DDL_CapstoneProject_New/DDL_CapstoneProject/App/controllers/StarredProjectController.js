//"use strict";

app.controller('StarredProjectController', function ($scope, project, toastr, ProjectService) {
    $scope.ListStarredProject = project.data.Data;

    $scope.deleteReminded = function (index) {
        var promiseDeleteReminded = ProjectService.deleteReminded($scope.ListStarredProject[index].ProjectID);

        promiseDeleteReminded.then(
            function (result) {
                
                if (result.data.Status === "success") {
                    $scope.ListStarredProject.splice(index, 1);
                    toastr.success('Xóa thành công!', 'Thành công!');
                } else {
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    $scope.length = $scope.ListStarredProject.length;
});