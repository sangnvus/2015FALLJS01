"use strict";

app.controller('BackedHistoryProjectController', function ($scope, projects, UserService, ProjectService, DTOptionsBuilder, DTColumnDefBuilder, $rootScope, CommmonService, toastr) {
    $scope.ListBackedProjectHistory = projects.data.Data;
    getBackedUserInfo();

    // Define table
    $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withBootstrap()
        .withOption('order', [3, 'desc']);

    // Define column 
    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(5).notSortable()
    ];

    $scope.exportExcel = function () {

        var list = [];
        var promise = UserService.GetBackingForUserExport();
        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    list = $scope.Backer = result.data.Data;
                    list.unshift(['Mã dự án',
                        'Tên dự án',
                        'Mã gói',
                        'Mô tả gói',
                        'Ngày chuyển giao',
                        'Mã ủng hộ',
                        'Số tiền ủng hộ',
                        'Số lượng',
                        'Mô tả ủng hộ',
                        'Ngày ủng hộ',
                        'Người ủng hộ',
                        'Tên đăng nhập',
                        'Email',
                        'Địa chỉ',
                        'SĐT']);
                    var CurrentUsername = $rootScope.UserInfo.UserName;
                    CommmonService.exportExcel(list, "Lịch Sử Ủng Hộ - " + CurrentUsername);
                } else {
                    var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    if (a) {
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    }
                }
            },
                function (error) {
                    toastr.error('Lỗi');
                });

    }

    function getBackedUserInfo(){
        var promiseGet = UserService.getBackedUserInfo();     
            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.BackedUser = result.data.Data;
                    } else {
                        CommmonService.checkError(result.data.Type);
                        $scope.Error = result.data.Message;
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
    }

    $scope.getBackingInfo =  function(projectCode) {
        var promiseGet = ProjectService.backingInfo(projectCode);
        promiseGet.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.BackingInfo = result.data.Data;

                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }

});


