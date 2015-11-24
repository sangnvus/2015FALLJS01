﻿"use strict";

app.controller('AdminUserListController',
    function ($scope, $rootScope, toastr, listuser, AdminUserService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder, SweetAlert) {
        //Todo here.
        $scope.userList = listuser.data.Data;

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [0, 'asc'])
        .withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(0).notSortable(),
            DTColumnDefBuilder.newColumnDef(-1).notSortable()
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

        // Alert admin before change status
        $scope.warning = function (username,index) {
            SweetAlert.swal({
                title: "Bạn vừa thay đổi tình trạng hoạt động của người dùng!",
                text: "Tình trạng hoạt động của người dùng sẽ bị thay đổi!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có!",
                cancelButtonText: "Không!",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //SweetAlert.swal("Edited!", "Project's basic has been edited.", "success");
                        $scope.activeUser(username, index);
                    } else {
                        //SweetAlert.swal("Cancelled", "Project's basic is safe :)", "error");
                    }
                });
        };
    });