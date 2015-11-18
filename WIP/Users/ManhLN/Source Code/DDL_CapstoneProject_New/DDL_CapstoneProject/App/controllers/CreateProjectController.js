"use strict";

app.controller('CreateProjectController', function ($scope, $rootScope, $location, toastr, CommmonService, ProjectService, categories, SweetAlert) {
    // Get categories
    $scope.Categories = categories.data.Data;
    // Set selected project category
    $scope.selectedOption = $scope.Categories[0];
    // check error
    $scope.isError = false;
    // Show rule
    $scope.showRule = false;

    var rank;
    var percent;

    $scope.save = function () {
        if ($scope.Project.FundingGoal < 1000000 || $scope.Project.Title.length < 10) {
            $scope.isError = true;
        } else {
            $scope.isError = false;
        }

        if (!$scope.isError) {
            if ($scope.Project.FundingGoal <= 50000000) {
                rank = "D";
                percent = "5";
            } else if ($scope.Project.FundingGoal <= 100000000 && $scope.Project.FundingGoal > 50000000) {
                rank = "C";
                percent = "4";
            } else if ($scope.Project.FundingGoal <= 500000000 && $scope.Project.FundingGoal > 100000000) {
                rank = "B";
                percent = "3";
            } else {
                rank = "A";
                percent = "2";
            }


            SweetAlert.swal({
                title: "Hạng dự án " + rank,
                text: "Với số tiền gây quỹ này chúng tôi sẽ lấy " + percent + "% tổng số tiền gây quỹ được",
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
                } else {
                    //SweetAlert.swal("Cancelled", "Project's timeline is safe :)", "error");

                }
            });
        }
    }

});