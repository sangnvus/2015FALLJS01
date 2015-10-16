"use strict";

app.controller('PublicProfileController', function ($scope, userpublicinfo, $filter) {
    $scope.UserBasicInfo = userpublicinfo.data.Data;

    $scope.UserBasicInfo.CreatedDate = new Date($filter('date')($scope.UserBasicInfo.CreatedDate, "yyyy-MM-dd"));
    $scope.UserBasicInfo.LastLogin = new Date($filter('date')($scope.UserBasicInfo.LastLogin, "yyyy-MM-dd"));
});