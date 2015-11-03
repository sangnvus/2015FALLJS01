"use strict";

app.controller('AdminUserListController',
    function ($scope, $rootScope, toastr, listuser, AdminUserService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder) {
        //Todo here.
        $scope.userList = listuser.data.Data;

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [0, 'asc'])
        .withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(5).notSortable()
        ];

        $scope.activeUser = function (username,index) {
            var promise = AdminUserService.changeUserStatus(username);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        toastr.success("Thành công!");
                        var promiseTable = AdminUserService.getUserlist();
                        promiseTable.then(
                            function (result) {
                                $scope.userList = result.data.Data;
                            }
                        )
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }


    });