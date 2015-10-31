"use strict";

app.controller('PaymentProjectController', function ($scope, $rootScope, $sce, $route, $location, toastr, CommmonService, ProjectService, rewardPkgs, usereditinfo, project) {
    // Get rewardPkgs
    $scope.RewardPkgs = rewardPkgs.data.Data;
    // Get Project basic information
    $scope.Project = project.data.Data;

    // Get backingdata form backing page
    $scope.BackData = ProjectService.getBack();

    // Get user basic information
    $scope.UserBasicInfo = usereditinfo.data.Data;

    // Initial backing data record
    $scope.backingData = {};

    $scope.backingData.RewardPKgID = $scope.BackData.RewardPKgID;
    $scope.backingData.PledgeAmount = $scope.BackData.PledgeAmount;
    $scope.backingData.Quantity = $scope.BackData.Quantity;

    for (var i = 0; i < $scope.RewardPkgs.length; i++) {
        if ($scope.RewardPkgs[i].RewardPkgID == $scope.BackData.RewardPKgID) {
            $scope.Packet = $scope.RewardPkgs[i];
        }
    }

    if ($scope.BackData.PledgeAmount == null) {
        $location.path("/project/back/" + $route.current.params.code).replace();
    }

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') === 0;
    }

    //$scope.enableSubmit = false;

    //if ($scope.UserBasicInfo.Addres != null && $scope.UserBasicInfo.FullName != null && $scope.UserBasicInfo.Email != null && $scope.UserBasicInfo.ContactNumber != null) {
    //    $scope.enableSubmit = true;
    //}

    $scope.submit = function () {
        $scope.backingData.ProjectCode = $scope.BackData.ProjectCode;
        $scope.backingData.Address = $scope.UserBasicInfo.Addres;
        $scope.backingData.Email = $scope.UserBasicInfo.Email;
        $scope.backingData.PhoneNumber = $scope.UserBasicInfo.ContactNumber;

        var promisePost = ProjectService.backingProject($scope.backingData);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Bạn đã ủng hộ dự án thành công!');
                    $location.path("/project/detail/" + result.data.Data).replace();
                } else {
                    //CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }

});