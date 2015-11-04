"use strict";

app.controller('AdminProjectDashboardController',
    function ($scope, $rootScope, toastr, listproject, basicInfo, AdminProjectService,
    CommmonService, DTOptionsBuilder, DTColumnDefBuilder) {

        // Get project list
        $scope.ProjectList = listproject.data.Data;
        // Get basic info
        $scope.BasicInfo = basicInfo.data.Data;

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [0, 'desc'])
        .withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(7).notSortable()
        ];

        $scope.save = function () {
            //$scope.Project.CategoryId = $scope.selectedOption.CategoryID;

            //var promisePost = ProjectService.createProject($scope.Project);

            //promisePost.then(
            //    function (result) {
            //        if (result.data.Status === "success") {
            //            toastr.success('Bạn đã khởi tạo dự án thành công!', 'Thành công!');
            //            $location.path("/project/edit/" + result.data.Data).replace();
            //        } else {
            //            CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
            //            $scope.Error = result.data.Message;
            //            toastr.error($scope.Error, 'Lỗi!');
            //        }
            //    },
            //    function (error) {
            //        $scope.Error = error.data.Message;
            //        toastr.error($scope.Error, 'Lỗi!');
            //    });
        }

    });