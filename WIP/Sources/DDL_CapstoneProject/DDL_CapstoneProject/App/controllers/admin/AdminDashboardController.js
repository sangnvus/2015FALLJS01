"use strict";

app.controller('AdminDashBoardController', function ($scope, $rootScope, $sce, toastr, basicInfo, listcategory, topBackers, AdminUserService, AdminProjectService,
    CommmonService, DTOptionsBuilder, DTColumnDefBuilder) {
    // Get basic info
    $scope.BasicInfo = basicInfo.data.Data;
    // Get category list
    $scope.Categories = {
        Labels: [],
        Data: []
    };
    // Get top backers
    $scope.TopBacker = topBackers.data.Data;

    // Pass data to donut_Cate chart
    for (var i = 0; i < listcategory.data.Data.length; i++) {
        $scope.Categories.Labels.push(listcategory.data.Data[i].Name);
        $scope.Categories.Data.push(listcategory.data.Data[i].ProjectCount);
    };

    // Donut chart for rank project
    $scope.donut_Rank = {
        Labels: ["Rank A", "Rank B", "Rank C", "Rank D"],
        Data: [$scope.BasicInfo.RankA, $scope.BasicInfo.RankB, $scope.BasicInfo.RankC, $scope.BasicInfo.RankD]
    };

    // Donut chart for success rate project
    $scope.donut_Rate = {
        Labels: ["Thành công", "Thất bại"],
        Data: [$scope.BasicInfo.TotalSucceed, $scope.BasicInfo.TotalFail]
    };

    // Get recent user
    $scope.getRecentUser = function () {
        var promise = AdminUserService.getRecentUser();

        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.RecentUser = result.data.Data;
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
    };
    $scope.getRecentUser();

    // Get top project
    $scope.getTopProject = function () {
        var promise = AdminProjectService.getTopProject();

        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.TopProject = result.data.Data;
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
    };
    $scope.getTopProject();
});