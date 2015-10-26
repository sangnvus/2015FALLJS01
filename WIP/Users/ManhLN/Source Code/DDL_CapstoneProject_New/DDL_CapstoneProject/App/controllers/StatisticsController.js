"use strict";

app.controller('StatisticsController',
    function ($scope, projectSucesedCount, categoryStatistic, projectTopList, ProjectService, UserTopList, UserService) {
        var projectData = projectSucesedCount.data.Data;
        $scope.projectSucesedCount = projectData.SuccesedCount;
        $scope.totalFunded = projectData.TotalFunded;
        $scope.backingUserCount = projectData.BackingUserCount;
        $scope.userBackmuchCount = projectData.UserBackmuchCount;
        $scope.numberOfBacking = projectData.NumberOfBacking;

        $scope.categoryList = categoryStatistic.data.Data.GetAllCategories;

        $scope.projectTopList = projectTopList.data.Data;
        $scope.selectedCategoryIDTopProject = "All";
        $scope.getTopProjectList = function (selectedid) {
            ProjectService.getProjectTop(selectedid).then(
                function (projectlist) {
                    $scope.projectTopList = projectlist.data.Data;
                }
            );
        }

        $scope.UserTopBackList = UserTopList.data.Data.UserTopBack;
        $scope.selectedCategoryIDTopUserBack = "All";
        $scope.getUserTopBack = function (selectedid) {
            UserService.getUserTop(selectedid).then(
                function (UserTopBackList) {
                    $scope.UserTopBackList = UserTopBackList.data.Data.UserTopBack;
                }
            );
        }

        $scope.UserTopFundList = UserTopList.data.Data.UserTopFund;
        $scope.selectedCategoryIDTopUserFunded = "All";
        $scope.getUserTopFund = function (selectedid) {
            UserService.getUserTop(selectedid).then(
                function (UserTopFundList) {
                    $scope.UserTopFundList = UserTopFundList.data.Data.UserTopFund;
                }
            );
        }


        $scope.chartData = categoryStatistic.data.Data.GetCategoryProjectStatistic;
        $scope.chart_options = {
            data: $scope.chartData,
            xkey: 'Name',
            ykeys: ['CategorySuccessFunded', 'CategoryFailFunded'],
            labels: ['Số tiền ủng hộ dự án thành công', 'Số tiền ủng hộ dự án không thành công']
        };
    });
