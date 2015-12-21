"use strict";

app.controller('AdminBackingListController',
    function ($scope, $sce, $rootScope, toastr, listBacking, AdminUserService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder) {
        //Todo here.

        $scope.exportExcel = function () {

            var list = [];
            var promise = AdminUserService.getListBackingFullInfor();
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
                        CommmonService.exportExcel(list, "Danh sách ủng hộ - Quản trị viên");
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });

        }

        $scope.ListBacking = listBacking.data.Data;

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }

        // Function check string startwith 'http'
        $scope.checkHTTP = function (input) {
            var lowerStr = (input + "").toLowerCase();
            return lowerStr.indexOf('http') == 0;
        }
        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [4, 'desc'])
        //.withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(0).notSortable(),
            DTColumnDefBuilder.newColumnDef(5).notSortable()
        ];

        $scope.GetBacker = function (userName, backingID) {
            var promise = AdminUserService.getBackker(userName, backingID);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Backer = result.data.Data;
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

    });