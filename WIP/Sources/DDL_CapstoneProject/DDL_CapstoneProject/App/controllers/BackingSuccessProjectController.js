"use strict";

app.controller('BackingSuccessProjectController', function ($scope, $route, $rootScope, $location, toastr, CommmonService, ProjectService, backingData) {
    // Get backing detail
    $scope.BackingData = backingData.data.Data;

    if ($scope.BackingData.Description == 'undefined' || $scope.BackingData.Description == null) {
        $scope.BackingData.Description = '';
    }


});