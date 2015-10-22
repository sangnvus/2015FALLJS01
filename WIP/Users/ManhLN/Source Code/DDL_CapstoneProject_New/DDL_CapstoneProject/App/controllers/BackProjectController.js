"use strict";

app.controller('BackProjectController', function ($scope, $route, $rootScope, $location, toastr, CommmonService, ProjectService, rewardPkgs) {
    // Get rewardPkgs
    $scope.RewardPkgs = rewardPkgs.data.Data;

    // Back data
    $scope.BackData = {};

    $scope.BackData.Quantity = 1;

    $scope.indexFlag = 1;

    $scope.model = 1;

    $scope.test = function (index) {
        $scope.indexFlag = index;
        $scope.BackData.Quantity = 1;
        $scope.BackData.PledgeAmount = $scope.RewardPkgs[index - 1].PledgeAmount;
    }

    $scope.back = function (index) {
        if (index == 'free') {
            $scope.BackData.RewardPKgID = "nonRewardPKgs";
            $scope.BackData.Quantity = 1;
        } else {
            $scope.BackData.RewardPKgID = $scope.RewardPkgs[index].RewardPkgID;
        }

        $scope.BackData.ProjectCode = $route.current.params.code;

        var promisePost = ProjectService.addBack($scope.BackData);

        if ($rootScope.UserInfo.IsAuthen == true) {
            $location.path("/project/payment/" + $route.current.params.code).replace();
        } else {
            console.log("chua login");
        }
    }

});