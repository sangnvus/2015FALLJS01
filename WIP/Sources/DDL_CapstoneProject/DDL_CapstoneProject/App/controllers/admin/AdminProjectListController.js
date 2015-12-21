"use strict";

app.controller('AdminProjectListController',
    function ($scope, $rootScope, toastr, listproject, AdminProjectService,
    CommmonService, DTOptionsBuilder, DTColumnDefBuilder) {

        // Get project list
        $scope.ProjectList = listproject.data.Data;

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [1, 'desc'])
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(8).notSortable()
        ];

        $scope.exportExcel = function () {
            var promise = AdminProjectService.AdminGetAllProjectDetail();
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        var list = result.data.Data;
                        list.unshift([
                            'Mã dự án',
                            'Ngày tạo',
                            'Tiêu đề',
                            'Đã gây vốn được',
                            'Mục tiêu gây vốn',
                            'Ngày hết hạn',
                            'Trạng thái dự án',
                            'Gây vốn',
                            'Thời hạn',
                            'Lượt ủng hộ',
                            'Danh mục',
                            'Địa chỉ',
                            'Lượt chỉnh sửa',
                            'Tên người tạo',
                            'tài khoản người tạo']);
                        CommmonService.exportExcel(list, "Danh sách dự án");
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });

        }
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