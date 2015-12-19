"use strict";

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
                            function(result) {
                                if (result.data.Status === "success") {
                                    $scope.userList = result.data.Data;
                                } else {
                                    var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                                    if (a) {
                                        $scope.Error = result.data.Message;
                                        toastr.error($scope.Error, 'Lỗi');
                                    }
                                }
                            },
                            function(error) {
                                toastr.error('Lỗi');
                            });
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

        // Alert admin before change status
        $scope.warning = function (username,index) {
            SweetAlert.swal({
                title: "Yêu cầu xác nhận",
                text: "Bạn có chắc chắn muốn thay đổi trạng thái của người dùng này",
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