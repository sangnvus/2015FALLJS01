"use strict";

app.controller('BackProjectController', function ($scope, $route, $rootScope, $location, toastr, CommmonService, ProjectService, rewardPkgs, project) {
    // Get rewardPkgs
    $scope.RewardPkgs = rewardPkgs.data.Data;
    // Get Project basic information
    $scope.Project = project.data.Data;

    // Initial Back data
    $scope.BackData = {};

    // Set min quantity
    $scope.BackData.Quantity = 1;

    // Flag to check select reward
    $scope.indexFlag = 1;

    // Init selected reward
    $scope.model = 1;

    // Select or not select a reward
    var checkSelect = false;

    $scope.rewardSelected = $route.current.params.reward;
    if ($scope.rewardSelected != null) {
        $scope.model = $scope.rewardSelected;
        $scope.indexFlag = $scope.rewardSelected;
    }
    // Detech selected rewards
    $scope.selectReward = function (index) {
        checkSelect = true;
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

        if (checkSelect === false) {
            $scope.BackData.PledgeAmount = $scope.RewardPkgs[0].PledgeAmount;
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