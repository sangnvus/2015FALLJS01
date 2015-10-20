"use strict";

app.controller('BackProjectController', function ($scope, $rootScope, $location, toastr, CommmonService, ProjectService, rewardPkgs) {
    // Get rewardPkgs
    $scope.RewardPkgs = rewardPkgs.data.Data;

    $scope.save = function () {
        $scope.Project.CategoryId = $scope.selectedOption.CategoryID;

        var promisePost = ProjectService.createProject($scope.Project);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Bạn đã khởi tạo dự án thành công!', 'Thành công!');
                    $location.path("/project/edit/" + result.data.Data).replace();
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }

});