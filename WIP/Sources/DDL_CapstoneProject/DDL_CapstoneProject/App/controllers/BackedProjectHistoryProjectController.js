"use strict";

app.controller('BackedProjectHistoryController', function ($scope, projects) {
    $scope.ListBackedProjectHistory = projects.data.Data;

});