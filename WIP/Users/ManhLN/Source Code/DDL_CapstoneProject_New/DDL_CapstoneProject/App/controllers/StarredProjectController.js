//"use strict";

app.controller('StarredProjectController', function ($scope, project, toastr, ProjectService) {
    $scope.ListStarredProject = project.data.Data;
    //$scope.length = $scope.ListStarredProject.length;
    $scope.length = $scope.ListStarredProject.length;
    $scope.deleteReminded = function (projectID) {
        var promiseDeleteReminded = ProjectService.deleteReminded(projectID);

        promiseDeleteReminded.then(
            function (result) {
                
                if (result.data.Status === "success") {                  
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
});