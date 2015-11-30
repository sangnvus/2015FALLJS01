"use strict";

app.controller('AdminBackingDetailController',
    function ($scope, $rootScope, backing, AdminProjectService,
    CommmonService) {
        // Get project list
        $scope.Project = backing.data.Data;

    });