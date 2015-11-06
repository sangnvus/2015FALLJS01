"use strict";

app.controller('BackProjectController', function ($scope, $route, $rootScope, $location, toastr, CommmonService, ProjectService, rewardPkgs, project) {
    // Get rewardPkgs
    $scope.RewardPkgs = rewardPkgs.data.Data;
    // Get Project basic information
    $scope.Project = project.data.Data;
    // Initial Back data
    $scope.BackData = {};
    // check error
    $scope.isError = false;

    // Get reward selected
    var rewardSelected = $route.current.params.reward;
    if (rewardSelected != null) {
        $scope.model = parseInt(rewardSelected);
        $scope.BackData.PledgeAmount = $scope.RewardPkgs[parseInt(rewardSelected) - 1].PledgeAmount;
    } else {
        // Init selected reward
        $scope.model = 1;
        $scope.BackData.PledgeAmount = $scope.RewardPkgs[0].PledgeAmount;
    }

    // Set min quantity
    $scope.BackData.Quantity = 1;

    // Flag to check select reward
    $scope.indexFlag = 1;

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
        if ($scope.RewardPkgs[index].Type === 'limited'
            && ($scope.BackData.Quantity > ($scope.RewardPkgs[index].Quantity - $scope.RewardPkgs[index].CurrentQuantity))) {
            $scope.isError = true;
        } else if ($scope.RewardPkgs[index].Type !== 'no reward' && ($scope.BackData.Quantity < 1 || $scope.BackData.Quantity == null || $scope.BackData.Quantity === '')) {
            $scope.isError = true;
        } else if ($scope.BackData.PledgeAmount < $scope.RewardPkgs[index].PledgeAmount * $scope.BackData.Quantity || $scope.BackData.PledgeAmount == null || $scope.BackData.PledgeAmount === '') {
            $scope.isError = true;
        } else {
            $scope.isError = false;
        }

        if (!$scope.isError) {
            $scope.BackData.RewardPKgID = $scope.RewardPkgs[index].RewardPkgID;

            $scope.BackData.ProjectCode = $route.current.params.code;

            //if (checkSelect === false && $scope.BackData.PledgeAmount == undefined) {
            //    $scope.BackData.PledgeAmount = $scope.RewardPkgs[0].PledgeAmount;
            //}

            var promisePost = ProjectService.addBack($scope.BackData);

            if ($rootScope.UserInfo.IsAuthen == true) {
                $location.path("/project/payment/" + $route.current.params.code).replace();
            } else {
                CommmonService.checkError("not-authen", $rootScope.BaseUrl);
            }
        }

    }

});