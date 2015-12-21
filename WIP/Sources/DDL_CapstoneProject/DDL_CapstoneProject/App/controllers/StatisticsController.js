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

        var chartData = categoryStatistic.data.Data.GetCategoryProjectStatistic;
        var name = [];
        var fundedSucess = [];
        var fundedFail = [];
        for (var i in chartData) {
            name.push(chartData[i].Name);
            fundedSucess.push(chartData[i].CategorySuccessFunded);
            fundedFail.push(chartData[i].CategoryFailFunded)
        }

        var ctx = document.getElementById("myChart").getContext("2d");
        var data = {
            labels: name,
            datasets: [
                {
                    label: "Số tiền ủng hộ dự án thành công",
                    fillColor: "#97bbcd",
                    data: fundedSucess
                },
                {
                    label: "Số tiền ủng hộ dự án không thành công",
                    fillColor: "#f7464a",
                    //highlightFill: "rgba(151,187,205,0.75)",\
                    data: fundedFail
                }
            ]
        };

        var options = {
            multiTooltipTemplate: function (label) {
                return (label.datasetLabel + ': ' + label.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")) + "₫";
            },
            scaleLabel: function (label) {
                return (label.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")) + "₫";
            }
        };
        var myBarChart = new Chart(ctx).Bar(data, options);

        $scope.getRank = function (fundingGoal) {
            if (fundingGoal <= 50000000)
                return "Hạng D";
            if (fundingGoal <= 100000000 && fundingGoal > 50000000)
                return "Hạng C";
            if (fundingGoal <= 500000000 && fundingGoal > 100000000)
                return "Hạng B";
            else return "Hạng A";
        }


    });
