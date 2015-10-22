"use strict";

app.controller('PublicProfileController', function ($scope,$sce, userpublicinfo, $filter) {
    $scope.UserBasicInfo = userpublicinfo.data.Data;

    //$scope.UserBasicInfo.CreatedDate = new Date($filter('date')($scope.UserBasicInfo.CreatedDate, "yyyy-MM-dd"));
    //$scope.UserBasicInfo.LastLogin = new Date($filter('date')($scope.UserBasicInfo.LastLogin, "yyyy-MM-dd"));
    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') === 0;
    }
});